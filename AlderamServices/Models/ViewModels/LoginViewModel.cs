using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlderamServices.Models.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }

    public class LoginTokenViewModel
    {
        public string Token { get; set; }
    }

}