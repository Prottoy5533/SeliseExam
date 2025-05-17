using EmployeeManage.Domain.Common;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using EmployeeManagement.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManage.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UnitOfWork(
            ApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<int> CommitAsync()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                var affectedRows = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return affectedRows;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
                await _context.DisposeAsync();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
