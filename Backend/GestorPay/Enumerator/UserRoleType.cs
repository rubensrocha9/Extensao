using System.ComponentModel;

namespace GestorPay.Enumerator
{
    public enum UserRoleType : byte
    {
        [Description("Admin")]
        Admin = 1,

        [Description("Usuário")]
        User = 2,
    }
}
