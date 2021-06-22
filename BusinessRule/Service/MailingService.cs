using System.Collections.Generic;
using BusinessRule.Model;

namespace BusinessRule.Service
{
    public class MailingService:IMailingService
    {
        private readonly List<Mail> _mails;

        public MailingService()
        {
            _mails = new List<Mail>();
        }
        public void SendMail(Mail mail)
        {
            _mails.Add(mail);
        }

        public List<Mail> GetSentMailsSent()
        {
            return _mails;
        }
    }
}