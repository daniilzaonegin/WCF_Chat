using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    [CallbackBehavior(
        ConcurrencyMode=ConcurrencyMode.Single,
        UseSynchronizationContext = false)]
    public partial class MainWindow : Window, ChatServiceReference.ChatServiceLibCallback
    {
        private ChatServiceReference.ChatServiceLibClient _messageService = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void NotifyUserJoinedTheConversation(string userName)
        {
            Dispatcher.Invoke(()=> 
            {
                string user = userName.ToString().ToUpper();
                WriteMessage(String.Format("[{0}] has joined the conversation", userName));
            });
        }

        public void NotifyUserLeftTheConversation(string userName)
        {
            Dispatcher.Invoke(()=>
            {
                WriteMessage(String.Format("[{0}] has left the conversation", userName));
            });
        }

        public void NotifyUserOfMessage(string userName, string userMessage)
        {
            Dispatcher.Invoke(()=>
            {
                WriteMessage(String.Format("[{0}]:{1}", userName, userMessage));
            });
        }
        private void WriteMessage(string message)
        {
            string format = txtMessageLog.Text.Length > 0 ? "\r\n{0} {1}" : "{0} {1}";
            txtMessageLog.AppendText(String.Format(format, DateTime.Now.ToShortTimeString(), message));
 //           txtMessageLog.Text = String.Format(format, this.txtMessageLog.Text,
   //                 DateTime.Now.ToShortTimeString(), message);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _messageService = new ChatServiceReference.ChatServiceLibClient(new InstanceContext(this), "WSDualHttpBinding_ChatServiceLib");

            btnJoin.IsEnabled = false;
            btnSend.IsEnabled = false;
            btnLeave.IsEnabled = false;
            btnExit.IsEnabled = true;
            txtMessageOutbound.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_messageService!=null)
            {
                _messageService.LeaveTheConversation(txtName.Text);
                _messageService.Close();
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtName.Text!=String.Empty)
            {
                btnJoin.IsEnabled = true;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            _messageService.LeaveTheConversation(txtName.Text);
            Close();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            _messageService.ReceiveMessageAsync(txtName.Text, null, txtMessageOutbound.Text);
            //_messageService.ReceiveMessage(txtName.Text, null, txtMessageOutbound.Text);
        }

        private void btnLeave_Click(object sender, RoutedEventArgs e)
        {
            _messageService.LeaveTheConversationAsync(txtName.Text);
//            _messageService.LeaveTheConversation(txtName.Text);
            btnJoin.IsEnabled = true;
            btnSend.IsEnabled = false;
            btnLeave.IsEnabled = false;
            txtMessageOutbound.IsEnabled = false;
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            //_messageService.JoinTheConversation(txtName.Text);
            _messageService.JoinTheConversationAsync(txtName.Text);
            btnJoin.IsEnabled = false;
            btnSend.IsEnabled = true;
            btnLeave.IsEnabled = true;
            txtMessageOutbound.IsEnabled = true;
        }
    }
}
