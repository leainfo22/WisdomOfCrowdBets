using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomOfCrowndBets.Core.DTO
{
    public class Email
    {
        public string? toEmail          {get;set;}
        public string? emailSubject     {get;set;}
        public string? emailBody        {get;set;}
        public string? fromEmail        {get;set;}
        public string? smtpServer         {get; set;}
        public int smtpPort { get; set;}

    }
}
