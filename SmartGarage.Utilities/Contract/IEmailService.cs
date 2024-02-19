﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Utilities.Contract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, byte[] attactchment = null);
    }
}
