using GestorPay.Context;
using GestorPay.Enumerator;
using GestorPay.Helper;
using GestorPay.Models.DTOs;
using GestorPay.Models.DTOs.Auth;
using GestorPay.Models.DTOs.User;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GestorPay.Models.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IRepository _repository;
        private readonly EmailBody _emailBody;
        private readonly IEmailService _emailService;
        private readonly HelperValidation _validation;
        private readonly INotificationService _notificationService;

        public AuthService(
            EmailBody emailBody,
            IConfiguration config,
            IRepository repository,
            IEmailService emailService,
            HelperValidation validation,
            INotificationService notificationService
            )
        {
            _config = config;
            _repository = repository;
            _emailBody = emailBody;
            _validation = validation;
            _emailService = emailService;
            _notificationService = notificationService;
        }


        public async Task<ReadAuthDto> Authenticate(CreateAuthDto createAuth)
        {
            if (createAuth == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthError, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var authEntityCompany = await _repository.Select<Company>()
                .Where(p => p.Email == createAuth.Email && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (authEntityCompany == null)
            {
                var employee = await _repository.Select<Employee>()
                    .Where(p => p.Email == createAuth.Email && p.Status != EmployeeStatusType.Disconnected &&
                                !p.IsRemoved)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthError, HttpStatusCode.NotFound);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                var authEmployee = await _repository.Select<AuthEmployee>()
                    .Where(p => p.AuthEmployeeId == employee.Id)
                    .FirstOrDefaultAsync();

                if (authEmployee == null)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.SendEmailError, HttpStatusCode.NotFound);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                if (!employee.IsEmailConfirmed)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthEmailConfirmed, HttpStatusCode.BadRequest);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                if (!PasswordHasher.VerifyPassword(createAuth.Password, authEmployee.Password))
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthError, HttpStatusCode.BadRequest);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                var newRefreshToken = await CreateRefreshToken();

                authEmployee.Token = CreateJwtEmployee(employee);
                authEmployee.RefreshToken = newRefreshToken;
                authEmployee.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

                await _repository.SaveAsync();

                return new ReadAuthDto
                {
                    CompanyId = employee.CompanyId,
                    EmployeeId = employee.Id,
                    RefreshToken = authEmployee.RefreshToken,
                    Token = authEmployee.Token,
                    Role = employee.Role
                };
            }
            else
            {
                if (!authEntityCompany.IsEmailConfirmed)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthEmailConfirmed, HttpStatusCode.BadRequest);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                if (!PasswordHasher.VerifyPassword(createAuth.Password, authEntityCompany.Auth.Password))
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthError, HttpStatusCode.BadRequest);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                var newRefreshToken = await CreateRefreshToken();

                authEntityCompany.Auth.Token = CreateJwt(authEntityCompany, authEntityCompany.IsCompany);
                authEntityCompany.Auth.RefreshToken = newRefreshToken;
                authEntityCompany.Auth.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

                await _repository.SaveAsync();

                return new ReadAuthDto
                {
                    CompanyId = authEntityCompany.Auth.Id,
                    RefreshToken = authEntityCompany.Auth.RefreshToken,
                    Token = authEntityCompany.Auth.Token,
                    Role = authEntityCompany.Role
                };
            }
        }

        public async Task Register(CreateUserDto createUser)
        {
            if (createUser == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.RegisterError, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            if (await _validation.CheckEmailExist(createUser.Email))
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.RegisterEmailError, HttpStatusCode.BadRequest);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            if (await _validation.CheckDocumentExist(createUser.DocumentNumber))
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.RegisterDocumentNumberError, HttpStatusCode.BadRequest);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var companyRegister = new AuthCompany();
            companyRegister.Password = PasswordHasher.HashPassword(createUser.Password);
            companyRegister.Token = "";
            companyRegister.RefreshToken = "";
            companyRegister.RegistrationLinkValidator = true;

            companyRegister.Company = new Company
            {
                Name = createUser.CompanyName,
                DocumentNumber = createUser.DocumentNumber,
                Email = createUser.Email,
                IsEmailConfirmed = false,
                Role = UserRoleType.Admin,
                IsCompany = true,
                IsRemoved = false
            };

            _repository.Insert(companyRegister);

            await _repository.SaveAsync();

            await SendEmailConfirmation(companyRegister);
        }

        public async Task RegisterEmployee(int companyId, int id, CreateEmployeeUserDTO createUser)
        {
            if (createUser == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.RegisterError, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var employee = await _repository.Select<Employee>()
                .Include(p => p.AuthEmployee)
                .Where(p => p.CompanyId == companyId &&
                            p.Id == id &&
                            p.Email == createUser.Email &&
                            p.DocumentNumber == createUser.DocumentNumber &&
                            !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.DocumentsRegisterEmployeeNotMatch, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var employeeValidationLink = await _repository.Select<AuthEmployee>()
                .Where(p => p.AuthEmployeeId == id)
                .FirstOrDefaultAsync();

            if (employeeValidationLink != null)
            {
                if (!employeeValidationLink.RegistrationLinkValidator)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthEmpoyeeAlreadyRegistred, HttpStatusCode.Conflict);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }
            }

            var employeeRegister = new AuthEmployee();
            employeeRegister.AuthEmployeeId = employee.Id;
            employeeRegister.Password = PasswordHasher.HashPassword(createUser.Password);
            employeeRegister.Token = "";
            employeeRegister.RefreshToken = "";
            employeeRegister.RegistrationLinkValidator = true;

            _repository.Insert(employeeRegister);

            await _repository.SaveAsync();

            await SendEmailConfirmationEmployee(employeeRegister, employee);
        }

        public async Task SendEmailResetPassword(ResetPasswordDTO resetPassword)
        {
            if (resetPassword == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthError, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var authEntity = await _repository.Select<Company>()
                .Where(p => p.Email == resetPassword.Email && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (authEntity == null)
            {
                var authEntityEmployee = await _repository.Select<Employee>()
                    .Where(p => p.Email == resetPassword.Email && !p.IsRemoved)
                    .FirstOrDefaultAsync();

                if (authEntityEmployee == null)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.SendEmailError, HttpStatusCode.NotFound);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                var authEmployee = await _repository.Select<AuthEmployee>()
                    .Where(p => p.AuthEmployeeId == authEntityEmployee.Id)
                    .FirstOrDefaultAsync();

                if (authEmployee == null)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.SendEmailError, HttpStatusCode.NotFound);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                var tokenBytes = RandomNumberGenerator.GetBytes(64);
                var emailToken = Convert.ToBase64String(tokenBytes);
                authEmployee.EmailToken = emailToken;
                authEmployee.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
                var subjectEmail = "Redefinir Senha";
                string? from = _config["EmailSettings:From"];
                var emailModel = new Email(resetPassword.Email, subjectEmail, _emailBody.EmailStringBody(resetPassword.Email, emailToken));

                _emailService.SendEmail(emailModel);
                await _repository.SaveAsync();
            }
            else
            {
                var tokenBytes = RandomNumberGenerator.GetBytes(64);
                var emailToken = Convert.ToBase64String(tokenBytes);
                authEntity.Auth.EmailToken = emailToken;
                authEntity.Auth.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
                var subjectEmail = "Redefinir Senha";
                string? from = _config["EmailSettings:From"];
                var emailModel = new Email(resetPassword.Email, subjectEmail, _emailBody.EmailStringBody(resetPassword.Email, emailToken));

                _emailService.SendEmail(emailModel);
                await _repository.SaveAsync();
            }
        }

        public async Task ResetPassword(ResetPasswordDTO resetPassword)
        {
            var authEntity = await _repository.Select<Company>()
                .Where(p => p.Email == resetPassword.Email && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (authEntity == null)
            {
                var authEntityEmployee = await _repository.Select<Employee>()
                    .Where(p => p.Email == resetPassword.Email && !p.IsRemoved)
                    .FirstOrDefaultAsync();

                if (authEntityEmployee == null)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthError, HttpStatusCode.BadRequest);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                var authEmployee = await _repository.Select<AuthEmployee>()
                    .Where(p => p.AuthEmployeeId == authEntityEmployee.Id)
                    .FirstOrDefaultAsync();

                if (authEmployee == null)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.SendEmailError, HttpStatusCode.NotFound);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                var tokenCode = authEmployee.EmailToken;
                DateTime emailTokenExpiry = authEmployee.ResetPasswordExpiry;

                if (tokenCode != resetPassword.EmailToken || emailTokenExpiry < DateTime.Now)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthInvalidLink, HttpStatusCode.BadRequest);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                authEmployee.Password = PasswordHasher.HashPassword(resetPassword.NewPassword);

                await _repository.SaveAsync();
            }
            else
            {
                var tokenCode = authEntity.Auth.EmailToken;
                DateTime emailTokenExpiry = authEntity.Auth.ResetPasswordExpiry;

                if (tokenCode != resetPassword.EmailToken || emailTokenExpiry < DateTime.Now)
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthInvalidLink, HttpStatusCode.BadRequest);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                if (!await _validation.IsPasswordStrong(resetPassword.NewPassword))
                {
                    var validationMessage = _notificationService.GetValidationMessage(ValidationType.StrongPassword, HttpStatusCode.BadRequest);
                    throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
                }

                authEntity.Auth.Password = PasswordHasher.HashPassword(resetPassword.NewPassword);

                await _repository.SaveAsync();
            }
        }

        public async Task ConfirmEmail(ConfirmEmailDto confirmEmail)
        {
            var EmailCheck = await _repository.Select<Company>()
                      .Where(p => p.Id == confirmEmail.CompanyId &&
                                   !p.IsRemoved &&
                                   p.Auth.EmailToken == confirmEmail.EmailToken)
                      .FirstOrDefaultAsync();

            if (EmailCheck == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            if (EmailCheck.IsEmailConfirmed && !EmailCheck.Auth.RegistrationLinkValidator)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthAlreadyConfirmed, HttpStatusCode.Conflict);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            EmailCheck.IsEmailConfirmed = true;
            EmailCheck.Auth.RegistrationLinkValidator = false;

            await _repository.SaveAsync();
        }

        public async Task ConfirmEmployeeEmail(ConfirmEmailDto confirmEmail)
        {

            var EmailCheck = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == confirmEmail.CompanyId &&
                            !p.IsRemoved &&
                            p.Id == confirmEmail.EmployeeId)
                .FirstOrDefaultAsync();

            if (EmailCheck == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var employeeValidation = await _repository.Select<AuthEmployee>()
                .Where(p => p.AuthEmployeeId == EmailCheck.Id &&
                            p.EmailToken == confirmEmail.EmailToken)
                .FirstOrDefaultAsync();

            if (EmailCheck.IsEmailConfirmed && !employeeValidation.RegistrationLinkValidator)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.AuthAlreadyConfirmed, HttpStatusCode.Conflict);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            EmailCheck.IsEmailConfirmed = true;
            employeeValidation.RegistrationLinkValidator = false;

            await _repository.SaveAsync();
        }

        private async Task SendEmailConfirmation(AuthCompany companyRegister)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            companyRegister.EmailToken = emailToken;
            var subjectEmail = "Confirmar Email do Cadastro";
            string? from = _config["EmailSettings:From"];
            var emailModel = new Email(companyRegister.Company.Email, subjectEmail, _emailBody.StringBodyConfirmEmailCompany(emailToken, companyRegister.Company.Id));

            _emailService.SendEmail(emailModel);
            await _repository.SaveAsync();
        }

        private async Task SendEmailConfirmationEmployee(AuthEmployee employeeRegister, Employee employee)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            employeeRegister.EmailToken = emailToken;
            var subjectEmail = "Confirmar Email do Cadastro";
            string? from = _config["EmailSettings:From"];
            var emailModel = new Email(employee.Email, subjectEmail, _emailBody.StringBodyConfirmEmailEmployee(emailToken, employee.CompanyId, employee.Id));

            _emailService.SendEmail(emailModel);
            await _repository.SaveAsync();
        }

        private string CreateJwt(IAuthenticatable authEntity, bool isCompany)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, authEntity.Role.GetDescription()),
                new Claim(ClaimTypes.Name, authEntity.Name),
                new Claim("companyId", authEntity.Id.ToString()),
                new Claim("is_company", isCompany.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokens = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokens);

            return jwt;
        }

        private string CreateJwtEmployee(Employee authEntity, bool isCompany = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, authEntity.Role.GetDescription()),
                new Claim(ClaimTypes.Name, authEntity.Name),
                new Claim("companyId", authEntity.CompanyId.ToString()),
                new Claim("employeeId", authEntity.Id.ToString()),
                new Claim("is_company", isCompany.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokens = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokens);

            return jwt;
        }

        private async Task<string> CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _repository.Select<AuthCompany>()
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return await CreateRefreshToken();
            }
            return refreshToken;
        }
    }
}
