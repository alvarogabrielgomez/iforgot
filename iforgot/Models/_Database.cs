using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace iforgot.Models
{
    public class _Database : DbContext
    {

        public _Database() : base("IFORGOT")
        {
            Configuration.LazyLoadingEnabled = false;

        }

        public DbSet<pwdReset> pwdReset { get; set; }

        public DbSet<users> users { get; set; }

        public DbSet<newUsers> newUsers { get; set; }
    }
}