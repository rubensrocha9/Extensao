using System.ComponentModel;

namespace GestorPay.Enumerator
{
    public enum EmployeeStatusType : byte
    {
        [Description("Ativo")]
        Active = 1,

        [Description("Desligado")]
        Disconnected = 2
    }
}
