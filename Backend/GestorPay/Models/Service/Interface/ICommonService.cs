using GestorPay.Models.DTOs;

namespace GestorPay.Models.Service.Interface
{
    public interface ICommonService
    {
        Task<List<EnumeratorQueryResult>> GetEnumValuesAsync(Type enumType);
    }
}
