using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single,
    InstanceContextMode = InstanceContextMode.PerCall)]
    public class ChatServiceLib : IMessageInboundService
    {
        private static List<IMessageServiceCallBack> _callbackList = new List<IMessageServiceCallBack>();

        public ChatServiceLib()
        {

        }

        public int JoinTheConversation(string userName)
        {
            IMessageServiceCallBack registeredUser = 
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallBack>();
            Console.WriteLine($"User {userName} joined the conversation");
            if (!_callbackList.Contains(registeredUser))
            {
                _callbackList.Add(registeredUser);
            }
            Console.WriteLine("Sending notification to all users...");
            _callbackList.ForEach((callback) =>
            {
               callback.NotifyUserJoinedTheConversation(userName);
            });
            Console.WriteLine("Notification is sent.");
            Console.WriteLine($"Users count: {_callbackList.Count}");
            return _callbackList.Count;
        }

        public int LeaveTheConversation(string userName)
        {
            Console.WriteLine($"User {userName} is going to leave the conversation!");

            IMessageServiceCallBack registeredUser =
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallBack>();

            if(_callbackList.Contains(registeredUser))
            {
                _callbackList.Remove(registeredUser);
                Console.WriteLine($"User {userName} has left the conversation!");
            }
            _callbackList.ForEach((serviceCallBack) => serviceCallBack.NotifyUserLeftTheConversation(userName));

            Console.WriteLine($"Other users notified");

            return _callbackList.Count;
        }

        public void ReceiveMessage(string userName, List<string> addressList, string userMessage)
        {
            Console.WriteLine($"Sending message from {userName} to others");
            _callbackList.ForEach((serviceCallBack) => serviceCallBack.NotifyUserOfMessage(userName, userMessage));
            Console.WriteLine($"Message is sent!");
        }
    }
}
