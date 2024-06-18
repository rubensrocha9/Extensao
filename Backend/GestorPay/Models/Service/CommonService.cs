using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;

namespace GestorPay.Models.Service
{
    public class CommonService : ICommonService
    {
        public Task<List<EnumeratorQueryResult>> GetEnumValuesAsync(Type enumType)
        {
            return Task.FromResult((from enumName in Enum.GetNames(enumType)
                                    select new EnumeratorQueryResult
                                    {
                                        Name = enumName,
                                        Value = Enum.Parse(enumType, enumName),
                                        Description = ((Enum)Enum.Parse(enumType, enumName)).GetDescription()
                                    }).ToList());
        }
    }
}
