﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MegaProject.Data.Entities;

namespace MegaProject.Data.EntityFramework.Configuration
{
    internal class CustomersConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomersConfiguration()
        {
            HasKey(c => c.Id).Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnName("CustomerID");
            Property(c => c.ContactName).HasMaxLength(50).IsRequired();
            Property(c => c.Email).HasMaxLength(100).IsRequired();
            Property(c => c.CompanyName).HasMaxLength(150);
            Property(c => c.Address).HasMaxLength(200);
            Property(c => c.PostalCode).HasMaxLength(10);
            Property(c => c.ContactTitle).HasMaxLength(50);
            Property(c => c.City).HasMaxLength(50);
            Property(c => c.Region).HasMaxLength(50);
            Property(c => c.Country).HasMaxLength(50);
            Property(c => c.Phone).HasMaxLength(50);
            Property(c => c.Fax).HasMaxLength(50);

            HasMany(c => c.Orders).WithRequired(o => o.Customer).WillCascadeOnDelete(true);
            
            // ignore NH id field
            Ignore(c => c.Int32Id);
        }
    }
}
