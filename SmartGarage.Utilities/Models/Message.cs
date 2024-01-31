﻿using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Utilities.Models
{
    public class Message
    {
        public MailboxAddress To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public Message(string to, string subject, string body)
        {
            this.To = new MailboxAddress("Smart Garage", to);
            this.Subject = subject;
            this.Body = body;
        }
    }
}
