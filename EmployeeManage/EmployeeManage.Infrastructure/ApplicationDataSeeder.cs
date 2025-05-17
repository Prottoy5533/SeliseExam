using EmployeeManage.Domain.Common;
using EmployeeManage.Domain.Entities.Users;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManage.Infrastructure
{
    public class ApplicationDataSeeder : IApplicationDataSeeder
    {
        private readonly ApplicationDbContext _context;
        public ApplicationDataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> SeedAsync()
        {
            try
            {
                if (!_context.Users.Any())
                {
                    await Reseed(_context, "[Users]");

                    _context.Users.AddRange(CreateSeedUsers());
                    await _context.SaveChangesAsync();
                }

                return (true, "Data seeded successfully");
            }
            catch (Exception ex)
            {
                return (true, $"Data Seed Error: {ex.Message}");
            }
        }

        private async Task Reseed(ApplicationDbContext context, string tableName, int seedStart = 0)
        {
            await context.Database.ExecuteSqlRawAsync($@"DBCC CHECKIDENT ('{tableName}', RESEED, {seedStart})");
        }

        private static List<User> CreateSeedUsers()
        {
            return new List<User>
        {
            new User { Email = "admin@demo.com", PasswordHash = "Admin123!" },
            new User {Email = "manager@demo.com", PasswordHash = "Manager123!"},
            new User {Email = "employee@demo.com", PasswordHash = "Employee123!"}
        };
        }

    }




}
