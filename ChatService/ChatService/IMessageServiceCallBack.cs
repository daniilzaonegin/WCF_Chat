using System.ServiceModel;

namespace ChatService
{
    public interface IMessageServiceCallBack
    {

        [OperationContract(IsOneWay = true)]
        void NotifyUserJoinedTheConversation(string userName);

        [OperationContract(IsOneWay = true)]
        void NotifyUserOfMessage(string userName, string userMessage);

        [OperationContract(IsOneWay = true)]
        void NotifyUserLeftTheConversation(string userName);
    }
}