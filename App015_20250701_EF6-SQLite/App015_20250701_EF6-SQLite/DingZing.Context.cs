﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App015_20250701_EF6_SQLite
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mainEntitiesDingZing : DbContext
    {
        public mainEntitiesDingZing()
            : base("name=mainEntitiesDingZing")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Defect> Defects { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
    }
}
