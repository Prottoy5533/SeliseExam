using EmployeeManagement.Application.Common.Extensions;
using EmployeeManagement.Application.Common.Helper;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using EmployeeManagement.Application.Feature.Common;
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

namespace EmployeeManagement.Application.Feature.User.Query;

public static class GetUsers
{
    public record GetUsersQuery(int PageNumber = 1,
        int PageSize = 10,
        string SortBy = "FullName",     
        string SortDirection = "asc") : IRequest<PagedResponse<GetUsersResponse>>;

    public record GetUsersResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int TeamId { get; set; }
        public int Role { get; set; }
        
    }

    public class GetUsersHandler(IUserRepository userRepository)
        : IRequestHandler<GetUsersQuery, PagedResponse<GetUsersResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<PagedResponse<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _userRepository.GetPaginatedAsync(
                request.PageNumber,
                request.PageSize,
                request.SortBy,
                request.SortDirection
            );

            var responseData = users.Adapt<List<GetUsersResponse>>();

            return new PagedResponse<GetUsersResponse>(
                responseData,
                request.PageNumber,
                request.PageSize,
                totalCount
            );
        }
    }
}


