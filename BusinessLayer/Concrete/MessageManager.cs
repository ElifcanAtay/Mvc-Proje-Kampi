using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public Message GetById(int id)
        {
            return _messageDal.Get(x => x.MessageID == id);
        }

        public List<Message> GetListInbox(string mail)
        {
            return _messageDal.List(x => x.ReceiverMail == mail).Where(x => x.isDraft == false).ToList();
        }

        public List<Message> GetListSendbox(string mail)
        {
            return _messageDal.List(x => x.SenderMail == mail).Where(x => x.isDraft == false).ToList();
        }
        public List<Message> GetListDraft(string mail)
        {
            return _messageDal.List(x => x.SenderMail == mail);
        }

        public void MessageAdd(Message message)
        {
            _messageDal.Insert(message);
        }

        public void MessageDelete(Message message)
        {
            throw new NotImplementedException();
        }

        public void MessageUpdate(Message message)
        {
            _messageDal.Update(message);
        }
        public List<Message> GetListUnRead(string mail)
        {
            return _messageDal.List(x => x.ReceiverMail == mail).Where(y => y.IsRead == false).ToList();
        }
        public List<Message> GetList(string mail)
        {
            return _messageDal.List(x => x.ReceiverMail == mail).Where(y => y.IsRead == true).ToList();
        }

    }
}
