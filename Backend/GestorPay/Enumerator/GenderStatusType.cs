using System.ComponentModel;

namespace GestorPay.Enumerator
{
    public enum GenderStatusType : byte
    {
        [Description("Masculino")]
        Male = 1,

        [Description("Feminino")]
        Female = 2
    }
}
