using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iforgot.Models
{
    [Table(name: "users")]
    public class users: EntityBase
    {
        [Key]
        public int user_id { get; set; }
        public bool active { get; set; }
        public users(){ active = false; }
        public string user_email { get; set; }

        public string user_name { get; set; }

        public string user_pwd { get; set; }
    }

    public abstract class EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime user_createdAt { get; set; }
    }
}