using GestorPay.Enumerator;
using GestorPay.Models.Service.Interface;
using System.Net;
using System.Reflection;
using System.Resources;

namespace GestorPay.Helper.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ResourceManager _resourceManager;
        public NotificationService()
        {
            _resourceManager = new ResourceManager("GestorPay.Models.Helper.Notification.ValidationMessages", Assembly.GetExecutingAssembly());
        }

        public (string Message, HttpStatusCode StatusCode) GetValidationMessage(ValidationType validationType, HttpStatusCode customStatusCode)
        {
            string key = Enum.GetName(typeof(ValidationType), validationType);
            string message = _resourceManager.GetString(key);

            return (message, customStatusCode);
        }
    }
}
