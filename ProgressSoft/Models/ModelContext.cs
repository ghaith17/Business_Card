using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Data.Entity;
using System.Linq;

namespace ProgressSoft.Models
{
    public class ModelContext:DbContext
    {
        public ModelContext():base("modelContext")
        {

        }
        public DbSet<BusinessCard> BusinessCards { get; set; } // My domain models

    }
}