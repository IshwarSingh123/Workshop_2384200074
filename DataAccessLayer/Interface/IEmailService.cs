﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface IEmailService
    {
        bool SendEmail(string to, string subject, string body);
    }
}
