using Contracts;
using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ChatService : IChat
    {
        static CancellationTokenSource _source = new CancellationTokenSource();
        private CancellationToken _token = _source.Token;
        private IChatDuplexCallback _callback = OperationContext.Current.GetCallbackChannel<IChatDuplexCallback>();

        
        public void AddMassage(UserMassage massage)
        {
            using (var db=new DBContext())
            {
                db.UserMassages.Add(massage);
            }
            

            StartSendingData();
        }

        public void StartSendingData()
        {
            Console.WriteLine("User asked for data");
            Task.Run((Action)SendMassage, _token);
        }
        private void SendMassage()
        {
            using (var db = new DBContext())
            {
                _callback.GetMassage(db.UserMassages.First());
            }
        }
    }
}
