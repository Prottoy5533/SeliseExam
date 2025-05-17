using Asp.Versioning.Builder;
using Carter;
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
using static EmployeeManagement.Application.Feature.User.UpdateUser;

namespace EmployeeManagement.Application.Feature.User;

public static class UpdateUser
{
    public record UpdateUserCommand : IRequest<UpdateUserResponse>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;
        public UpdateUserCommandValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(p => p.Id)
                .NotNull();

            RuleFor(p => p.FullName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(p => p.Email)
                .NotNull();

            RuleFor(p => p.RoleId)
                .NotNull();


            RuleFor(x => new { x.Id, x.Email, }).CustomAsync(async (property, context, cancellationToken) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var user = await userRepository.GetAsync(property.Id);
                if (user is null)
                {
                    context.AddFailure("User not found");
                }
                var isuserEmailExist = await userRepository
                    .AnyAsync(r => r.Id != property.Id && r.Email == property.Email);
                if (isuserEmailExist)
                {
                    context.AddFailure("this email already exist");
                }

            });
        }
    }
    public record UpdateUserResponse : BaseResponse<int>
    {

    }

    public class UpdateUserCommandHandler(IUnitOfWork unitOfWork,
        IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.Id);
            request.Adapt(user);
            await _userRepository.EditAsync(user);
            var affectedRows = await _unitOfWork.CommitAsync();
            return new UpdateUserResponse
            {
                Id = affectedRows > 0 ? user.Id : 0,
                IsSuccess = affectedRows > 0,
                Message = AppConstants.CrudResult
                    .Message(affectedRows, AppConstants.Entity.User, AppConstants.CrudOperation.Edit)
            };
        }
    }
}

public class UpdateUserEndpoints : BaseCarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        base.AddRoutes(app);

        app.MapPut(EndpointConstants.User, async (UpdateUserCommand command, ISender sender) =>
        {
            var response = await sender.Send(command);
            return Results.Ok(response);
        })
        .WithApiVersionSet(ApiVersionSet);
    }
}
