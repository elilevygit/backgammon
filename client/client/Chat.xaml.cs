using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace client
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        int _id;
        public List<UserMessage> messages = new List<UserMessage>();
        public List<UserMessage> MyMessages = new List<UserMessage>();
        
        public Chat(int id)
        {
            _id = id;
            InitializeComponent();

            refresh();
        }

        private void sendmassage_Click(object sender, RoutedEventArgs e)
        {


            UserMessage massage=new UserMessage();
            massage.message = Textmessage.Text;
            massage.receiver = new User() { ID = _id };
            ((MainWindow)System.Windows.Application.Current.MainWindow).sendmassage(massage);
            MyMessages.Add(massage);
            refresh();
        }
        public void refresh()
        {
            listmassage.ItemsSource = null;
            listmassage.ItemsSource = messages;
            mymassage.ItemsSource = null;
            mymassage.ItemsSource = MyMessages;
            
        }
        void App_Exit(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
     
        }
    }
}
