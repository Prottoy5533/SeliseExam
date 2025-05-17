using EmployeeManage.Domain.Entities.Teams;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManage.Infrastructure.Repositories
{
    public class TeamRepository(ApplicationDbContext context)
: BaseRepository<Team>(context), ITeamRepository
    {
    }
   
}
