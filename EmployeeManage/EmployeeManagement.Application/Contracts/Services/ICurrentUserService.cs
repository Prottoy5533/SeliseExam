using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Roles { get; }
    }

}
