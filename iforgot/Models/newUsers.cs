using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iforgot.Models
{
    [Table(name: "newUsers")]
    public class newUsers
    {
        [Key]
        public int newUser_id { get; set; }

        public string newUser_email { get; set; }

        public string newUser_token { get; set; }

        public int newUser_expires { get; set; }

        [NotMapped]
        public string newUser_pwd { get; set; }
        [NotMapped]
        public string newUser_pwdRepeat { get; set; }
        [NotMapped]
        public string newUser_name { get; set; }

    }
}