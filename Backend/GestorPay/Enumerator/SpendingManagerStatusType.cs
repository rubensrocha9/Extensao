using System.ComponentModel;

namespace GestorPay.Enumerator
{
    public enum SpendingManagerStatusType : byte
    {
        [Description("Ativo")]
        Active = 1,

        [Description("Desativado")]
        Deleted = 2
    }
}
