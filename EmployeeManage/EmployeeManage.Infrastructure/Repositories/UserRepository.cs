using EmployeeManage.Domain.Entities.Users;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
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
        public async Task<(List<User>, int)> GetPaginatedAsync(int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            var query = _context.Users.AsQueryable();

            // Sorting (dynamic using System.Linq.Dynamic.Core)
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var direction = sortDirection?.ToLower() == "desc" ? "descending" : "ascending";
                query = query.OrderBy($"{sortBy} {direction}"); // Requires: using System.Linq.Dynamic.Core
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }
    }


    
}
