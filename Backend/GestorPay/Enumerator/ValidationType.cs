namespace GestorPay.Enumerator
{
    public enum ValidationType : int
    {
        CompanyNotFound = 1,
        EmployeeNotFound = 2,
        EmployeeUpdateImageError = 3,
        EmployeeDeleteImageError = 4,
        AuthError = 5,
        AuthEmailConfirmed = 6,
        AuthNotFound = 7,
        AuthAlreadyConfirmed = 8,
        AuthSuccess = 9,
        AuthInvalidLink = 10,
        RegisterError = 11,
        RegisterEmailError = 12,
        SendEmailError = 13,
        SpendingManagerNotFound = 14,
        EmployeePositionNotFound = 15,
        PositionInUse = 16,
        RegisterDocumentNumberError = 17,
        StrongPassword = 18,
        DocumentsRegisterEmployeeNotMatch = 19,
        AuthEmpoyeeAlreadyRegistred = 20,
        FileNotAccpeted = 21,
    }
}
