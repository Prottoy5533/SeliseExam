using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManage.Domain.Entities.Tasks;

namespace EmployeeManage.Infrastructure.Configurations.Task
{
    public class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Tasks.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Tasks.Task> builder)
        {
            builder.ToTable("TaskItems");

            builder.HasKey(x => x.Id);
            builder.Property(p => p.Description)
                .IsRequired(true);
         
            builder.Property(p => p.Status)
                .IsRequired(true);

            
        }
    }
   
}
