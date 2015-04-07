using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.WEB.Logic
{
    public class Users
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string RoleMegjelenes { get; set; }

        [NotMapped]
        public List<ListItem> RoleList { get; set; }
    }
}