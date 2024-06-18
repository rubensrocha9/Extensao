namespace GestorPay.Models.Helper
{
    public class EmailBody
    {
        private readonly IConfiguration _configuration;

        public EmailBody(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EmailStringBody(string email, string emailToken)
        {
            string baseUrl = _configuration["AppSettings:BaseUrl"];

            return $@"<html>
            <head></head>
            <body>
                <table
                  cellspacing=""0""
                  border=""0""
                  cellpadding=""0""
                  width=""100%""
                  bgcolor=""#f2f3f8""
                  style=""
                    @import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700);
                    font-family: 'Open Sans', sans-serif;
                  ""
                >
                  <tr>
                    <td>
                      <table
                        style=""background-color: #f2f3f8; max-width: 670px; margin: 0 auto""
                        width=""100%""
                        border=""0""
                        align=""center""
                        cellpadding=""0""
                        cellspacing=""0""
                      >
                        <tr>
                          <td style=""height: 80px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td style=""text-align: center"">
                            <a href="""" title=""logo"" target=""_blank"">
                              <img
                                width=""280""
                                
                                title=""GestorPay""
                                alt=""""
                              />
                            </a>
                          </td>
                        </tr>
                        <tr>
                          <td style=""height: 20px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td>
                            <table
                              width=""95%""
                              border=""0""
                              align=""center""
                              cellpadding=""0""
                              cellspacing=""0""
                              style=""
                                max-width: 670px;
                                background: #fff;
                                border-radius: 3px;
                                text-align: center;
                                -webkit-box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                                -moz-box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                                box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                              ""
                            >
                              <tr>
                                <td style=""height: 40px"">&nbsp;</td>
                              </tr>
                              <tr>
                                <td style=""padding: 0 35px"">
                                  <h1
                                    style=""
                                      color: #1e1e2d;
                                      font-weight: 500;
                                      margin: 0;
                                      font-size: 32px;
                                      font-family: 'Rubik', sans-serif;
                                    ""
                                  >
                                    Você solicitou a redefinição de senha
                                  </h1>
                                  <span
                                    style=""
                                      display: inline-block;
                                      vertical-align: middle;
                                      margin: 29px 0 26px;
                                      border-bottom: 1px solid #cecece;
                                      width: 100px;
                                    ""
                                  ></span>
                                  <p
                                    style=""
                                      color: #455056;
                                      font-size: 15px;
                                      line-height: 24px;
                                      margin: 0;
                                    ""
                                  >
                                    Não podemos simplesmente enviar a você sua senha antiga. Um
                                    link exclusivo para redefinir sua senha foi gerado. 
                                    Para redefinir sua senha, clique no link a seguir e
                                    siga as instruções. O link expira em 15 minutos.
                                  </p>
                                  <a
                                    href=""{baseUrl}/reset-password?email={email}&code={emailToken}""
                                    style=""
                                      background: #8392ac;
                                      text-decoration: none !important;
                                      font-weight: 500;
                                      margin-top: 35px;
                                      color: #fff;
                                      text-transform: uppercase;
                                      font-size: 14px;
                                      padding: 10px 24px;
                                      display: inline-block;
                                      border-radius: 50px;
                                    ""
                                    >Redefinir Senha</a
                                  >
                                </td>
                              </tr>
                              <tr>
                                <td style=""height: 40px"">&nbsp;</td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                        <tr>
                          <td style=""height: 20px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td style=""text-align: center"">
                            <p
                              style=""
                                font-size: 14px;
                                color: rgba(69, 80, 86, 0.7411764705882353);
                                line-height: 18px;
                                margin: 0 0 20px 0;
                              ""
                            >
                              &copy; <strong>GestorPay</strong>
                            </p>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
             </body>
            </html>";
        }

        public string StringBodyConfirmEmailCompany(string emailToken, int companyId)
        {
            string baseUrl = _configuration["AppSettings:BaseUrl"];

            return $@"<html>
            <head></head>
            <body>
                <table
                  cellspacing=""0""
                  border=""0""
                  cellpadding=""0""
                  width=""100%""
                  bgcolor=""#f2f3f8""
                  style=""
                    @import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700);
                    font-family: 'Open Sans', sans-serif;
                  ""
                >
                  <tr>
                    <td>
                      <table
                        style=""background-color: #f2f3f8; max-width: 670px; margin: 0 auto""
                        width=""100%""
                        border=""0""
                        align=""center""
                        cellpadding=""0""
                        cellspacing=""0""
                      >
                        <tr>
                          <td style=""height: 80px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td style=""text-align: center"">
                            <a href="""" title=""logo"" target=""_blank"">
                              <img
                                width=""280""
                                
                                title=""GestorPay""
                                alt=""""
                              />
                            </a>
                          </td>
                        </tr>
                        <tr>
                          <td style=""height: 20px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td>
                            <table
                              width=""95%""
                              border=""0""
                              align=""center""
                              cellpadding=""0""
                              cellspacing=""0""
                              style=""
                                max-width: 670px;
                                background: #fff;
                                border-radius: 3px;
                                text-align: center;
                                -webkit-box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                                -moz-box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                                box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                              ""
                            >
                              <tr>
                                <td style=""height: 40px"">&nbsp;</td>
                              </tr>
                              <tr>
                                <td style=""padding: 0 35px"">
                                  <h1
                                    style=""
                                      color: #1e1e2d;
                                      font-weight: 500;
                                      margin: 0;
                                      font-size: 32px;
                                      font-family: 'Rubik', sans-serif;
                                    ""
                                  >
                                    Confirme seu Cadastro
                                  </h1>
                                  <span
                                    style=""
                                      display: inline-block;
                                      vertical-align: middle;
                                      margin: 29px 0 26px;
                                      border-bottom: 1px solid #cecece;
                                      width: 100px;
                                    ""
                                  ></span>
                                  <p
                                    style=""
                                      color: #455056;
                                      font-size: 15px;
                                      line-height: 24px;
                                      margin: 0;
                                    ""
                                  >
                                    Click no botão a baixo para confirmar seu cadastro.
                                  </p>
                                  <a
                                    href=""{baseUrl}/confirm-email?&code={emailToken}&companyid={companyId}""
                                    style=""
                                      background: #8392ac;
                                      text-decoration: none !important;
                                      font-weight: 500;
                                      margin-top: 35px;
                                      color: #fff;
                                      text-transform: uppercase;
                                      font-size: 14px;
                                      padding: 10px 24px;
                                      display: inline-block;
                                      border-radius: 50px;
                                    ""
                                    >Confirmar Cadastro</a
                                  >
                                </td>
                              </tr>
                              <tr>
                                <td style=""height: 40px"">&nbsp;</td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                        <tr>
                          <td style=""height: 20px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td style=""text-align: center"">
                            <p
                              style=""
                                font-size: 14px;
                                color: rgba(69, 80, 86, 0.7411764705882353);
                                line-height: 18px;
                                margin: 0 0 20px 0;
                              ""
                            >
                              &copy; <strong>GestorPay</strong>
                            </p>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
             </body>
            </html>";
        }

        public string StringBodyConfirmEmailEmployee(string emailToken, int companyId, int employeeId)
        {
            string baseUrl = _configuration["AppSettings:BaseUrl"];

            return $@"<html>
            <head></head>
            <body>
                <table
                  cellspacing=""0""
                  border=""0""
                  cellpadding=""0""
                  width=""100%""
                  bgcolor=""#f2f3f8""
                  style=""
                    @import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700);
                    font-family: 'Open Sans', sans-serif;
                  ""
                >
                  <tr>
                    <td>
                      <table
                        style=""background-color: #f2f3f8; max-width: 670px; margin: 0 auto""
                        width=""100%""
                        border=""0""
                        align=""center""
                        cellpadding=""0""
                        cellspacing=""0""
                      >
                        <tr>
                          <td style=""height: 80px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td style=""text-align: center"">
                            <a href="""" title=""logo"" target=""_blank"">
                              <img
                                width=""280""
                                
                                title=""GestorPay""
                                alt=""""
                              />
                            </a>
                          </td>
                        </tr>
                        <tr>
                          <td style=""height: 20px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td>
                            <table
                              width=""95%""
                              border=""0""
                              align=""center""
                              cellpadding=""0""
                              cellspacing=""0""
                              style=""
                                max-width: 670px;
                                background: #fff;
                                border-radius: 3px;
                                text-align: center;
                                -webkit-box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                                -moz-box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                                box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                              ""
                            >
                              <tr>
                                <td style=""height: 40px"">&nbsp;</td>
                              </tr>
                              <tr>
                                <td style=""padding: 0 35px"">
                                  <h1
                                    style=""
                                      color: #1e1e2d;
                                      font-weight: 500;
                                      margin: 0;
                                      font-size: 32px;
                                      font-family: 'Rubik', sans-serif;
                                    ""
                                  >
                                    Confirme seu Cadastro
                                  </h1>
                                  <span
                                    style=""
                                      display: inline-block;
                                      vertical-align: middle;
                                      margin: 29px 0 26px;
                                      border-bottom: 1px solid #cecece;
                                      width: 100px;
                                    ""
                                  ></span>
                                  <p
                                    style=""
                                      color: #455056;
                                      font-size: 15px;
                                      line-height: 24px;
                                      margin: 0;
                                    ""
                                  >
                                    Click no botão a baixo para confirmar seu cadastro.
                                  </p>
                                  <a
                                    href=""{baseUrl}/confirm-email?&code={emailToken}&companyid={companyId}&employeeid={employeeId}""
                                    style=""
                                      background: #8392ac;
                                      text-decoration: none !important;
                                      font-weight: 500;
                                      margin-top: 35px;
                                      color: #fff;
                                      text-transform: uppercase;
                                      font-size: 14px;
                                      padding: 10px 24px;
                                      display: inline-block;
                                      border-radius: 50px;
                                    ""
                                    >Confirmar Cadastro</a
                                  >
                                </td>
                              </tr>
                              <tr>
                                <td style=""height: 40px"">&nbsp;</td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                        <tr>
                          <td style=""height: 20px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td style=""text-align: center"">
                            <p
                              style=""
                                font-size: 14px;
                                color: rgba(69, 80, 86, 0.7411764705882353);
                                line-height: 18px;
                                margin: 0 0 20px 0;
                              ""
                            >
                              &copy; <strong>GestorPay</strong>
                            </p>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
             </body>
            </html>";
        }

        public string StringBodyEndRegisterEmployee(string emailToken, int companyId, int employeeId, string companyName)
        {
            string baseUrl = _configuration["AppSettings:BaseUrl"];

            return $@"<html>
            <head></head>
            <body>
                <table
                  cellspacing=""0""
                  border=""0""
                  cellpadding=""0""
                  width=""100%""
                  bgcolor=""#f2f3f8""
                  style=""
                    @import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700);
                    font-family: 'Open Sans', sans-serif;
                  ""
                >
                  <tr>
                    <td>
                      <table
                        style=""background-color: #f2f3f8; max-width: 670px; margin: 0 auto""
                        width=""100%""
                        border=""0""
                        align=""center""
                        cellpadding=""0""
                        cellspacing=""0""
                      >
                        <tr>
                          <td style=""height: 80px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td style=""text-align: center"">
                            <a href="""" title=""logo"" target=""_blank"">
                              <img
                                width=""280""
                                
                                title=""GestorPay""
                                alt=""""
                              />
                            </a>
                          </td>
                        </tr>
                        <tr>
                          <td style=""height: 20px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td>
                            <table
                              width=""95%""
                              border=""0""
                              align=""center""
                              cellpadding=""0""
                              cellspacing=""0""
                              style=""
                                max-width: 670px;
                                background: #fff;
                                border-radius: 3px;
                                text-align: center;
                                -webkit-box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                                -moz-box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                                box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.06);
                              ""
                            >
                              <tr>
                                <td style=""height: 40px"">&nbsp;</td>
                              </tr>
                              <tr>
                                <td style=""padding: 0 35px"">
                                  <h1
                                    style=""
                                      color: #1e1e2d;
                                      font-weight: 500;
                                      margin: 0;
                                      font-size: 32px;
                                      font-family: 'Rubik', sans-serif;
                                    ""
                                  >
                                    Pré Cadastro realizado.
                                  </h1>
                                  <span
                                    style=""
                                      display: inline-block;
                                      vertical-align: middle;
                                      margin: 29px 0 26px;
                                      border-bottom: 1px solid #cecece;
                                      width: 100px;
                                    ""
                                  ></span>
                                  <p
                                    style=""
                                      color: #455056;
                                      font-size: 15px;
                                      line-height: 24px;
                                      margin: 0;
                                    ""
                                  >
                                    Foi realizado um pré cadastro pela <span style=""font-size: 18px; font-weight: bold;"">""{companyName}""</span>,
                                    para ter acesso ao seu perfil finalize seu cadastro, clique no botão a seguir e
                                    siga as instruções.
                                  </p>
                                  <a
                                    href=""{baseUrl}/register-employee?&code={emailToken}&companyid={companyId}&employeeid={employeeId}""
                                    style=""
                                      background: #8392ac;
                                      text-decoration: none !important;
                                      font-weight: 500;
                                      margin-top: 35px;
                                      color: #fff;
                                      text-transform: uppercase;
                                      font-size: 14px;
                                      padding: 10px 24px;
                                      display: inline-block;
                                      border-radius: 50px;
                                    ""
                                    >Finalizar Cadastro</a
                                  >
                                </td>
                              </tr>
                              <tr>
                                <td style=""height: 40px"">&nbsp;</td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                        <tr>
                          <td style=""height: 20px"">&nbsp;</td>
                        </tr>
                        <tr>
                          <td style=""text-align: center"">
                            <p
                              style=""
                                font-size: 14px;
                                color: rgba(69, 80, 86, 0.7411764705882353);
                                line-height: 18px;
                                margin: 0 0 20px 0;
                              ""
                            >
                              &copy; <strong>GestorPay</strong>
                            </p>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
             </body>
            </html>";
        }
    }
}
