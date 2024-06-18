using GestorPay.Enumerator;
using System.Net;

namespace GestorPay.Models.Service.Interface
{
    public interface INotificationService
    {
        (string Message, HttpStatusCode StatusCode) GetValidationMessage(ValidationType validationType, HttpStatusCode customStatusCode);
    }
}

