﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace WebAppServico
{
    public class AuthHeader : SoapHeader
    {
        public string Token;      

    }

}