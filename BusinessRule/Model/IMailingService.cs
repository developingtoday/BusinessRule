using System.Collections.Generic;

namespace BusinessRule.Model
{
    public interface IMailingService
    {
        void SendMail(Mail mail);
        List<Mail> GetSentMailsSent();
    }
}