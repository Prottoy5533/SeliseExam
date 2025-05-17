using Asp.Versioning.Builder;
using EmployeeManagement.Application.Common.Extensions;
using EmployeeManagement.Application.Common.Helper;
using EmployeeManagement.Application.Feature.User.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EmployeeManagement.Application.Feature.User.Command.CreateUser;
using static EmployeeManagement.Application.Feature.User.Command.UpdateUser;

namespace EmployeeManage.Controllers
{
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

    public class CreateuserEndpoints : BaseCarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            base.AddRoutes(app);

            app.MapPost(EndpointConstants.User, async (CreateUserCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);
                return Results.Ok(response);
            })
            .WithApiVersionSet(ApiVersionSet);
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

}
