using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iforgot.Models
{
    [Table(name: "pwdReset")]
    public class pwdReset
    {
        [Key]
        public int pwdReset_id { get; set; }
        public string pwdReset_email { get; set; }
        public string pwdReset_selector { get; set; }
        public string pwdReset_token { get; set; }
        public int pwdReset_expires { get; set; }

        [NotMapped]
        public string pwdReset_pwd { get; set; }
        [NotMapped]
        public string pwdReset_pwdRepeat { get; set; }

    }
}   