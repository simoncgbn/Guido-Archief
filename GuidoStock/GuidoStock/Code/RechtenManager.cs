using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using GuidoStock.App_Code;
using GuidoStock.Models;
using Microsoft.AspNet.Identity;

namespace GuidoStock.Code
{
    public class RechtenManager
    {
        private static RechtenManager instance;
        private static readonly DBClass _db = new DBClass();
        private List<Recht> rechten; 

        private RechtenManager()
        {
            rechten = _db.GetRechten();
        }

        public bool VerifyAccess(string module, string page, ApplicationUser user, ApplicationUserManager manager)
        {
            var roles = manager.GetRoles(user.Id);
            var gebruiker = _db.GetGebruikerByEmail(user.UserName);
            if (gebruiker == null || gebruiker.IsVerwijderd) return false;
            if (page.ToLower().Contains("add") || page.ToLower().Contains("edit"))
                return rechten.Find(r => r.Module == module && r.Rol == roles[0] && r.Schrijven) != null;
            else
                return rechten.Find(r => r.Module == module && r.Rol == roles[0] && r.Lezen) != null;
        }

        public void UpdateRechten()
        {
            rechten = _db.GetRechten();
        }

        public static RechtenManager Instance => instance ?? (instance = new RechtenManager());
    }
}