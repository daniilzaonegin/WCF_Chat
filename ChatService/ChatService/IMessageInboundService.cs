using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract(Name ="ChatServiceLib",
        Namespace ="ChatService",
        CallbackContract = typeof(IMessageServiceCallBack))]

    public interface IMessageInboundService
    {
        [OperationContract]
        int JoinTheConversation(string userName);

        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string message, List<string> addressList, string userMessage);

        [OperationContract]
        int LeaveTheConversation(string userName);
    }

}
