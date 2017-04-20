using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoDB
{
    class MyDbContext :DbContext
    {
        public MyDbContext() : base("name=XML_to_DB")
        {

        }
        public virtual DbSet<MERCHANT_SO_ENTRY_DETAILS> MERCHANT_DETAILS { get; set; }
        public virtual DbSet<G_CONTRACTS> G_CONTRACTS { get; set; }
        public virtual DbSet<G_ORDER_TYPE> G_ORDER_TYPES { get; set; }
        public virtual DbSet<G_ORDER> G_ORDERS { get; set; }
        public virtual DbSet<G_DEVICE> G_DEVICE { get; set; }
        public virtual DbSet<G_TRANS_GROUP> G_TRANS_GROUPS { get; set; }
        public virtual DbSet<G_POSTING_DATE> G_POSTING_DATES { get; set; }
        public virtual DbSet<G_TRANS_DETAILS> G_TRANS_DETAILS { get; set; }
        public virtual DbSet<G_ENTRY> G_ENTRYS { get; set; }
        //public virtual DbSet<NewDataSet> NewDataSets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MERCHANT_SO_ENTRY_DETAILS>().ToTable("MERCHANT_DETAILS");
            modelBuilder.Entity<G_CONTRACTS>().ToTable("G_CONTRACTS");
            modelBuilder.Entity<G_ORDER_TYPE>().ToTable("G_ORDER_TYPES");
            modelBuilder.Entity<G_ORDER>().ToTable("G_ORDERS");
            modelBuilder.Entity<G_DEVICE>().ToTable("G_DEVICE");
            modelBuilder.Entity<G_TRANS_GROUP>().ToTable("G_TRANS_GROUPS");
            modelBuilder.Entity<G_POSTING_DATE>().ToTable("G_POSTING_DATES");
            modelBuilder.Entity<G_TRANS_DETAILS>().ToTable("G_TRANS_DETAILS");
            modelBuilder.Entity<G_ENTRY>().ToTable("G_ENTRYS");
            //modelBuilder.Entity<NewDataSet>().ToTable("NewDataSets");
            base.OnModelCreating(modelBuilder);
        }
    }
}
