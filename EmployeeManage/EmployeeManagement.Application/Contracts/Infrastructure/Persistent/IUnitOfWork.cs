﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Infrastructure.Persistent
{
   public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
