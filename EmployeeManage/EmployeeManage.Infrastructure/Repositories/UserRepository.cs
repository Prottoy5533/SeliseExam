using EmployeeManage.Domain.Entities.Users;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;
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
        public async Task<(List<User>, int)> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Users.AsQueryable();

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }
    }


    
}
