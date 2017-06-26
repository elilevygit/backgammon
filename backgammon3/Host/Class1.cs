using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace host
{
    
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService : IChat
    {
        
        List<User> onlineusers = new List<User>();
        List<User> offlineusers = new List<User>();
        Dictionary<int, IChatDuplexCallback> Callbackusers = new Dictionary<int, IChatDuplexCallback>();
        Dictionary<string, User> sessions = new Dictionary<string, User>();
        static CancellationTokenSource _source = new CancellationTokenSource();
        private CancellationToken _token = _source.Token;

        IChatDuplexCallback GetCurrentCallback()
        {
            return OperationContext.Current.GetCallbackChannel<IChatDuplexCallback>();
        }
    
        public void AddMessage(UserMessage massage)
        {



            Callbackusers[massage.receiver.ID].GetMessage(massage);

            using (var db = new DBContent())
            {
                db.UserMessages.Add(massage);

                db.SaveChanges();
            }

        }


        public int register(User User)
        {
            using (var db = new DBContent())
            {
                //is  id same ?
                db.Users.Add(new User() { UserName = User.UserName });
                db.SaveChanges();
            }

            return sign_in(User);

        }
        void Channel_Faulted(object sender, EventArgs e)
        {
            Logout((IContextChannel)sender);
        }

        protected void Logout(IContextChannel channel)
        {
            string sessionId = null;

            if (channel != null)
            {
                sessionId = channel.SessionId;
            }


            sign_out(sessions[sessionId]);
            sessions.Remove(sessionId);
           
        }
        public int sign_in(User User)
        {

            if (sessions.ContainsKey(OperationContext.Current.SessionId))
            {
                return sessions[OperationContext.Current.SessionId].ID;
            }
            foreach (var item in sessions)
            {
                if (item.Value.UserName==User.UserName)
                {
                    return -2;
                }
            }
           
          
            OperationContext.Current.Channel.Faulted += new EventHandler(Channel_Faulted);
            OperationContext.Current.Channel.Closed += new EventHandler(Channel_Faulted);

            if (User.UserName == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine("i am not null");
            }

            //this will never append
            if (onlineusers.Exists(e => e.ID == User.ID))
            {
                Console.WriteLine("this will never append");
                return User.ID;
            }
            //

            using (var db = new DBContent())
            {
                if (db.Users.Count() == 0)
                {
                    return -1;
                }

                User tmp = db.Users.Where(e => e.UserName == User.UserName).FirstOrDefault();

                if (tmp == null)
                {
                    return -1;
                }
                
                User.ID = tmp.ID;
            }
            sessions.Add(OperationContext.Current.SessionId, User);
            Console.WriteLine("add  " + User.UserName + "  " + User.ID);

            if (offlineusers.Exists(e => e.ID == User.ID))
            {
                offlineusers.Remove(User);
            }

            onlineusers.Add(User);

            foreach (var item in Callbackusers)
            {
                if (item.Key!=User.ID)
                {
                    item.Value.Getonlineuser(User);
                    item.Value.Removeofflineuser(User);
                }
               
            }

            Callbackusers.Add(User.ID, this.GetCurrentCallback());

            using (var db = new DBContent())
            {
                User sn = db.Users.Find(User.ID);
                User re = db.Users.Find(User.ID);
                if (sn != null)
                {
                    if (re == null)
                    {
                        Console.WriteLine("not passably");

                        return -1;
                    }
                    GetUserInfo(User.ID);
                    return User.ID;

                }
            }

            return -1;

        }


        public void sign_out(User User)
        {
            Console.WriteLine(User.UserName + " is being rmove");
            onlineusers.Remove(User);
            offlineusers.Add(User);
            foreach (var item in Callbackusers)
            {
                if (item.Key != User.ID)
                {
                    Console.WriteLine("upd "+item.Key);
                    Console.WriteLine("corent " + User.ID);

                    item.Value.Getofflineuser(User);
                    item.Value.Removeonlineuser(User);
                    
                }
            }
            Callbackusers.Remove(User.ID);
        }


        public string GetUserInfo(int UserId)
        {
            foreach (User item in offlineusers)
            {
                if (item.ID != UserId)
                {
                    GetCurrentCallback().Getofflineuser(item);

                }
            }
            foreach (User item in onlineusers)
            {
                if (item.ID!=UserId)
                {
                    GetCurrentCallback().Getonlineuser(item);
                }
            }
            return UserId.ToString();
        }





        public bool MoveToPositions(MovePosition mp)
        {
            Callbackusers[mp.receiver.ID].GetPositions(mp);

            return true;
        }


        public bool playrequest(User request,User sender)
        {


            return Callbackusers[request.ID].sendplayrequest(request,sender);
        }


        public bool finishTurn(MovePosition id)
        {
           return Callbackusers[id.receiver.ID].finishTurn(id);
        }
    }
}
