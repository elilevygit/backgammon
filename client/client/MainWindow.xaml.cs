using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public delegate void UserEventHandler(User user);
    public delegate void MovePositionEventHandler(MovePosition user);
    public delegate void UserMessageEventHandler(UserMessage user);



    public class ChatCallback : IChatDuplexCallback
    {
        public event UserMessageEventHandler massageevent;

        public event UserEventHandler offlineusersevent;

        public event UserEventHandler onlineusersevent;
        public event UserEventHandler playrequest;

        public event MovePositionEventHandler Positions;
        public event MovePositionEventHandler finishMove;

        public event UserEventHandler removeofflineuser;

        public event UserEventHandler removeonlineuser;
        public string GetMessage(UserMessage massage)
        {
            massageevent(massage);

            return null;
        }

        public void Getofflineuser(User offlineusers)
        {
            if (offlineusersevent != null)
            {
                offlineusersevent(offlineusers);
            }
        }

        public void Getonlineuser(User onlineusers)
        {
            if (onlineusersevent != null)
            {
                onlineusersevent(onlineusers);
            }
        }
        public void GetPositions(MovePosition mp)
        {
            if (Positions != null)
            {
                Positions(mp);
            }
        }

        public void Removeofflineuser(User user)
        {
            if (removeofflineuser != null)
            {
                removeofflineuser(user);
            }
        }

        public void Removeonlineuser(User user)
        {
            if (removeonlineuser != null)
            {
                removeonlineuser(user);
            }
        }
        public bool sendplayrequest(User request, User sender)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Sure", "רוצה לשחק שש בש עם" + request.UserName + "?", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (playrequest != null)
                {
                    playrequest(sender);
                }
                return true;
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                return false;
            }

            return false;
        }


        public bool finishTurn(MovePosition id)
        {
            if (finishMove!=null)
            {
                finishMove(id);
                return true;
            }
            return false;
        }
    }

    public partial class MainWindow : Window
    {
        public ChatCallback _ChatCallback;
        public User I_am;
        public bool i_game_host = false;
        public List<User> offlineusers = new List<User>();
        public List<User> onlineusers = new List<User>();
        public IChat proxy;
        private Dictionary< int,GameBoard> _backgammonList;
        private Dictionary< int,Chat> _chatList;
        private Sign_in _signin;

        private DuplexChannelFactory<IChat> factory;
        private InstanceContext instanceContext;
        public MainWindow()
        {
            _chatList = new Dictionary< int,Chat>();
            _backgammonList = new Dictionary<int,GameBoard>();
            InitializeComponent();

            _ChatCallback = new ChatCallback();
            instanceContext = new InstanceContext(_ChatCallback);

            _ChatCallback.massageevent += _ChatCallback_massageevent;
            _ChatCallback.offlineusersevent += _ChatCallback_offlineusersevent;
            _ChatCallback.onlineusersevent += _ChatCallback_onlineusersevent;
            _ChatCallback.removeofflineuser += _ChatCallback_removeofflineuser;
            _ChatCallback.removeonlineuser += _ChatCallback_removeonlineuser;
            _ChatCallback.Positions += _ChatCallback_Positions;
            _ChatCallback.finishMove += _ChatCallback_finishMove;
            _ChatCallback.playrequest += _ChatCallback_playrequest;

            factory = new DuplexChannelFactory<IChat>(instanceContext, "ClientEP");

            I_am = new User();

            proxy = factory.CreateChannel();

            _signin = new Sign_in();

            sign_in.Children.Add(_signin);
        }

        void _ChatCallback_finishMove(MovePosition user)
        {
            _backgammonList[user.sender.ID].BackToMyTurn();
        }

        public void movePositions(int[] Positions, int id)
        {


            Task.Run(() =>
          {

              proxy.MoveToPositions(new MovePosition() { sender = I_am, Move = Positions, receiver = new User() { ID = id } });
          });


            refresh();
        }

        public void register(User user)
        {
            I_am.UserName = user.UserName;

            Task.Run(() =>
            {
                try
                {
                    I_am.ID = proxy.sign_in(new User() { UserName = I_am.UserName });
                    if (I_am.ID==-2)
                    {
                        MessageBox.Show("! משתמש קיים כבר נסה אחר" );
                    }
                    if (I_am.ID == -1)
                    {
                        I_am.ID = proxy.register(I_am);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("? השרת עובד" + e.Message.ToString());
                    return;
                }
            });

            refresh();
        }

        public void sendmassage(UserMessage message)
        {
            if (onlineusers.Find(e=>e.ID==message.receiver.ID)==null)
            {
                MessageBox.Show("משתמש לא ברשת");
                return;
            }

            try
            {
                message.sender = I_am;
                proxy.AddMessage(message);
            }
            catch (Exception)
            {
                MessageBox.Show("שגיאה");
                return;
            }
            refresh();
        }

        internal void sendgamerrequest()
        {

            User selected = listonlineusers.SelectedItem as User;


            Task.Run(() =>
            {
                try
                {
                    bool re = proxy.playrequest(selected, I_am);

                    if (!re)
                    {
                        MessageBox.Show("תשובה שלילית");
                    }
                    else
                    {
                        i_game_host = true;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("? השרת עובד" + e.Message.ToString());
                    return;
                }
            });
        }

        internal void sign_out()
        {
            if (I_am.ID != 0)
            {
                proxy.sign_out(I_am);

                refresh();
            }
        }

        private void _ChatCallback_massageevent(UserMessage um)
        {


            if (!_chatList.ContainsKey(um.sender.ID) )
            {
                MessageBox.Show("you receive message from " + um.sender.UserName);

                _chatList.Add(um.sender.ID, new Chat(um.sender.ID) { Title = um.sender.UserName });
                _chatList[um.sender.ID].Show();

            }
            else if (!_chatList[um.sender.ID].Activate())
            {
                _chatList[um.sender.ID].Show();
            }
            else
            {
                MessageBox.Show("you receive message from " + um.sender.UserName);

            }
            _chatList[um.sender.ID].messages.Add(um);
            _chatList[um.sender.ID].refresh();
            refresh();
        }

        private void _ChatCallback_offlineusersevent(User sender)
        {
            offlineusers.Add(sender);
            refresh();
        }

        private void _ChatCallback_onlineusersevent(User sender)
        {
            onlineusers.Add(sender);
            refresh();
        }

        private void _ChatCallback_playrequest(User sender)
        {
            if (!_backgammonList.ContainsKey(sender.ID))
            {
                _backgammonList.Add(sender.ID, new GameBoard( sender.ID) { Title = sender.UserName });
            }

            if (_backgammonList[sender.ID].Activate())
            {
                _backgammonList[sender.ID].Visibility = Visibility.Visible;
            }
            else
            {
                if (i_game_host)
                {
                    MessageBox.Show("i am already host");
                    return;
                }

                _backgammonList[sender.ID].Show();
            }
        }
        private void _ChatCallback_Positions(MovePosition sender)
        {
            _backgammonList[sender.sender.ID].resume_game(sender.Move);
        }

        private void _ChatCallback_removeofflineuser(User sender)
        {
            if (offlineusers.Exists(u => u.ID == sender.ID))
            {
                offlineusers.Remove(offlineusers.Find(e => e.ID == sender.ID));
            }
            refresh();
        }

        private void _ChatCallback_removeonlineuser(User sender)
        {
            if (onlineusers.Exists(u => u.ID == sender.ID))
            {
                onlineusers.Remove(onlineusers.Find(e=>e.ID==sender.ID));
            }
            refresh();
        }
        private void App_Exit(object sender, CancelEventArgs e)
        {
            if (factory.State != CommunicationState.Faulted)
            {
                factory.Close();
            }

            Application.Current.Shutdown();
        }

        private void chat_Click(object sender, RoutedEventArgs e)
        {
             User selected = listonlineusers.SelectedItem as User;
             if (selected==null)
             {
                 return;
             }

             if (_chatList.ContainsKey(selected.ID))
            {
                if (_chatList[selected.ID].Activate())
                {
                    _chatList[selected.ID].Visibility = Visibility.Visible;
                }
                else
                {
                    _chatList[selected.ID].Show();
                }
            }
            else 
            {
                _chatList.Add(selected.ID, new Chat(selected.ID) { Title = selected.UserName });
                _chatList[selected.ID].Show();
            }

             _chatList[selected.ID].listmassage.ItemsSource = null;
             _chatList[selected.ID].listmassage.ItemsSource = _chatList[selected.ID].messages;
            refresh();
        }

        private void game_Click(object sender, RoutedEventArgs e)
        {
            User selected = listonlineusers.SelectedItem as User;

            if (selected==null)
            {
                return;
            }
            if (_backgammonList.ContainsKey(selected.ID))
            {
                if (_backgammonList[selected.ID].Activate())
                {
                    _backgammonList[selected.ID].Visibility = Visibility.Visible;
                }
                else
                {
                    _backgammonList[selected.ID].Show();
                }
                
            }
            else
            {
                _backgammonList.Add(selected.ID, new GameBoard(selected.ID) {Title=selected.UserName });
               

                _backgammonList[selected.ID].Show();
            }

            sendgamerrequest();
        }

        private void Ref_Click(object sender, RoutedEventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            listofflineusers.ItemsSource = null;
            listonlineusers.ItemsSource = null;

            listofflineusers.ItemsSource = offlineusers;
            listonlineusers.ItemsSource = onlineusers;
        }

        internal void finishTurn(int id)
        {
            Task.Run(() =>
            {
                try
                {
                    bool re = proxy.finishTurn( new MovePosition() { sender = I_am , receiver = new User() { ID = id } });

                    if (!re)
                    {
                        MessageBox.Show("נכשל");
                    }
                    else
                    {
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("? השרת עובד" + e.Message.ToString());
                    return;
                }
            });
        }
    }
}