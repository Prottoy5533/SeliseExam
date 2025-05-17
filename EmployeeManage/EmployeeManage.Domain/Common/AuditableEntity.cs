using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManage.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = Constants.DefaultUser;

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
