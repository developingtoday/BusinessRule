using System.Collections.Generic;
using BusinessRule.Model;

namespace BusinessRule.Service
{
    public interface IMailingService
    {
        void SendMail(Mail mail);
        List<Mail> GetSentMailsSent();
    }
}