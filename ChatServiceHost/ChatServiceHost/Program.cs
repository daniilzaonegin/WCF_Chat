using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatService;
using System.ServiceModel;

namespace ChatServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ChatServiceLib));
            Console.WriteLine("Starting chat service...");
            host.Open();
            Console.WriteLine("Chat Service is running. Press any key to stop it...");
            Console.ReadLine();
            host.Close();
        }


    }
}
