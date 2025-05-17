using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManage.Infrastructure.Repositories
{
    public class TaskRepository(ApplicationDbContext context)
: BaseRepository<EmployeeManage.Domain.Entities.Tasks.ProjectTask>(context), ITaskRepository
    {
    }
    
}
