using Asp.Versioning.Builder;
using EmployeeManage.Domain.Entities.Users;
using EmployeeManagement.Application.Common.Extensions;
using EmployeeManagement.Application.Common.Helper;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using EmployeeManagement.Application.Feature.Common;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeManagement.Application.Feature.User.Command.CreateUser;

namespace EmployeeManagement.Application.Feature.User.Command;

public static class CreateUser
{
    public record CreateUserCommand : IRequest<CreateUserResponse>
    {
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int TeamId { get; set; }

    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;
        public CreateUserCommandValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(p => p.FullName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

            RuleFor(p => p.Email)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

            RuleFor(x => new { x.Email }).CustomAsync(async (property, context, cancellationToken) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var isuserEmailExist = await userRepository.AnyAsync(u => u.Email == property.Email);
                if (isuserEmailExist)
                {
                    context.AddFailure("User email already exist");
                }

            });
        }


    }

    public record CreateUserResponse : BaseResponse<int> { }

    public class CreateUserCommandHandler(IUnitOfWork unitOfWork,
        IUserRepository userRepository) : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<EmployeeManage.Domain.Entities.Users.User>();
            await _userRepository.AddAsync(user);
            var affectedRows = await _unitOfWork.CommitAsync();
            return new CreateUserResponse
            {
                Id = affectedRows > 0 ? user.Id : 0,
                IsSuccess = affectedRows > 0,
                Message = AppConstants.CrudResult
                    .Message(affectedRows, AppConstants.Entity.User, AppConstants.CrudOperation.Add)
            };
        }
    }
}


