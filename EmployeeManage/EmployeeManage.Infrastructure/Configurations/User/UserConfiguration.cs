using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeManage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManage.Infrastructure.Configurations.User
{
    public class UserConfiguration : IEntityTypeConfiguration<EmployeeManage.Domain.Entities.Users.User> 
    {
        public void Configure(EntityTypeBuilder<EmployeeManage.Domain.Entities.Users.User> builder) 
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(50); ;
            
        }
    }
}
