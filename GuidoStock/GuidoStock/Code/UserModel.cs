using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GuidoStock.Models;

namespace GuidoStock.Code
{
    public class UserModel
    {
        private ApplicationUser _User;
        private string _Rol;

        public UserModel(ApplicationUser user, string rol)
        {
            User = user;
            Rol = rol;
        }

        public ApplicationUser User
        {
            get { return _User; }
            set { _User = value; }
        }

        public string Rol
        {
            get { return _Rol; }
            set { _Rol = value; }
        }

        public string UserName => _User.Name;
        public string UserId => _User.Id;
    }
}