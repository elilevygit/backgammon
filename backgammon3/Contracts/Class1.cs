using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required,
                    CallbackContract = typeof(IChatDuplexCallback))]
    public interface IChat
    {
        [OperationContract(IsOneWay = true)]
        void AddMessage(UserMessage message);
        [OperationContract]
        bool playrequest(User request,User sender);

         [OperationContract]
        int sign_in(User user);

          [OperationContract]
         int register(User user);
         [OperationContract]
          string GetUserInfo(int userId);
         [OperationContract(IsOneWay = true)]
         void sign_out(User User);
        [OperationContract]
         bool MoveToPositions(MovePosition mp);
        [OperationContract]
        bool finishTurn(MovePosition id);
    }

    [ServiceContract]
    public interface IChatDuplexCallback
    {
        [OperationContract]
        bool sendplayrequest(User request,User sender);
        [OperationContract]
        string GetMessage(UserMessage message);

        [OperationContract(IsOneWay = true)]

        void GetPositions(MovePosition mp);

        [OperationContract(IsOneWay = true)]

        void Getonlineuser(User onlineusers);

        [OperationContract(IsOneWay = true)]

        void Getofflineuser(User offlineusers);
        [OperationContract(IsOneWay = true)]
        void Removeonlineuser(User user);
        [OperationContract(IsOneWay = true)]
        void Removeofflineuser(User user);
         [OperationContract]
        bool finishTurn(MovePosition id);
    }

    [DataContract]
  
    public class User
    {

        public User()
        {
            _Callback = null;
        }
    
        [DataMember]
       public int ID { get; set; }
        [DataMember]
       public string UserName { get; set; }



        [DataMember]
        public static IChatDuplexCallback _Callback;
       
        public IChatDuplexCallback callback
        {
            get { return _Callback; }
            set { _Callback = value; }
        }
        
      
    }
    [DataContract]
  
    public class UserMessage
    {
        [DataMember]
        public int UserMessageID { get; set; }

        [DataMember]
  
        public int senderid { get; set; }

        [DataMember]
        public virtual User sender { get; set; }
       
        [DataMember]
    
        public int receiverid { get; set; }

        [DataMember]
        public virtual User receiver { get; set; }

        [DataMember]
        public string message { get; set; }

    }
    [DataContract]
    public class MovePosition
    {

        [DataMember]
        public int[] Move { get; set; }
        [DataMember]

        public virtual User receiver { get; set; }
        [DataMember]
        public virtual User sender { get; set; }
    }
}
