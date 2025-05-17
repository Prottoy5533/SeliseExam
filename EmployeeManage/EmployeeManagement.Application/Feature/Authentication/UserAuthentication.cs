using Asp.Versioning.Builder;
using Carter;
using EmployeeManagement.Application.Common.Extensions;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using EmployeeManagement.Application.Feature.Authentication;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Feature.Authentication
{
    public static class UserAuthentication
    {
        public record LoginCommand : IRequest<LoginResponse>
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public record LoginResponse
        {
            public bool Authenticated { get; set; }
            public string Email { get; set; } = string.Empty;
            public string AccessToken { get; set; } = string.Empty;
            
        }

        public record AuthUser(int UserId, string Email, string Roles);

        public class LoginCommandValidator : AbstractValidator<LoginCommand>
        {
            public LoginCommandValidator()
            {
                RuleFor(p => p.Email)
                        .NotEmpty();

                RuleFor(p => p.Password)
                    .NotEmpty();
            }
        }

        public class LoginHandler(
            IConfiguration configuration,
            IDataProtectionProvider dataProtectionProvider,
            IUnitOfWork unitOfWork,
     
            IUserRepository userRepository) : IRequestHandler<LoginCommand, LoginResponse>
        {
            private readonly IConfiguration _configuration = configuration;
            private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector("RefreshTokenProtector");
            private readonly IUnitOfWork _unitOfWork = unitOfWork;
            
            private readonly IUserRepository _userRepository = userRepository;

            public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.FirstAsync(u => u.Email.Equals(request.Email));
                if (user is null)
                {
                    return new LoginResponse { Authenticated = false };
                }
                var passwordHasher = new PasswordHasher<string>();
                var passworVerifyResult = passwordHasher.VerifyHashedPassword(request.Email, user.PasswordHash, request.Password);
                if (passworVerifyResult != PasswordVerificationResult.Success)
                {
                    return new LoginResponse { Authenticated = false };
                }


                var affectedRows = await _unitOfWork.CommitAsync();

                return new LoginResponse
                {
                    Authenticated = affectedRows > 0,
                    Email = user.Email,
                    AccessToken = TokenHelper.GenerateJwtToken(_configuration, user.Email,user.RoleId.ToString()),
                    
                };
            }
        }
    }
}

public class UserAuthenticationEndpoints : BaseCarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        base.AddRoutes(app);

        app.MapPost("login", async (UserAuthentication.LoginCommand command, HttpResponse response, ISender sender) =>
        {
            var result = await sender.Send(command);

            if (!result.Authenticated)
                return Results.Unauthorized();

          

            return Results.Ok(result);
        })
        
        .WithApiVersionSet(ApiVersionSet);
    }
}
