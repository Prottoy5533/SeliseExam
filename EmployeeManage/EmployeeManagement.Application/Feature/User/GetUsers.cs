using EmployeeManagement.Application.Common.Extensions;
using EmployeeManagement.Application.Common.Helper;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Feature.User;

public static class GetUsers
{
    public record GetUsersQuery() : IRequest<List<GetUsersResponse>>;

    public record GetUsersResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        
    }

    public class GetUsersHandler(IUserRepository userRepository)
        : IRequestHandler<GetUsersQuery, List<GetUsersResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<List<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAsync();
            return users.Adapt<List<GetUsersResponse>>();
        }
    }
}

public class GetUsersEndpoints : BaseCarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        base.AddRoutes(app);

        app.MapGet(EndpointConstants.User, async (ISender sender) =>
        {
            var query = new GetUsers.GetUsersQuery();
            var result = await sender.Send(query);

            if (result is null)
                return Results.NotFound();

            return Results.Ok(result);
        })
       
       .WithApiVersionSet(ApiVersionSet);
    }
}
