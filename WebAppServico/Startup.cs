﻿using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Owin;

namespace WebAppServico
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);            
        }
    }
}
