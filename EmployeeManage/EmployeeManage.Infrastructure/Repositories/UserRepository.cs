using EmployeeManage.Domain.Entities.Users;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManage.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context)
: BaseRepository<User>(context), IUserRepository
    {
    }
    
}
