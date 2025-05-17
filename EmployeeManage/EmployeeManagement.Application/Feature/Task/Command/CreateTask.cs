using EmployeeManage.Domain.Entities.Users;
using EmployeeManagement.Application.Common.Helper;
using EmployeeManagement.Application.Contracts.Infrastructure.Persistent;
using EmployeeManagement.Application.Contracts.Services;
using EmployeeManagement.Application.Feature.Common;
using FluentValidation;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Feature.Task.Command
{
    public static class CreateTask
    {
        public record CreateTaskCommand : IRequest<CreateTaskResponse>
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public TaskStatus Status { get; set; } = TaskStatus.Created;
            public int AssignedToUserId { get; set; }
            public int TeamId { get; set; }
            public DateTime DueDate { get; set; }
        }

        public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
        {
            private readonly IServiceProvider _serviceProvider;
            public CreateTaskCommandValidator(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;

                RuleFor(p => p.Title)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(p => p.Description)
                    .NotEmpty()
                    .MaximumLength(500);


            }
        }
        public record CreateTaskResponse : BaseResponse<int> { }

        public class CreateTaskCommandHandler(
            IUnitOfWork unitOfWork,
            ITaskRepository taskRepository,
            ICurrentUserService currentUserService)
            : IRequestHandler<CreateTaskCommand, CreateTaskResponse>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;
            private readonly ITaskRepository _taskRepository = taskRepository;
            private readonly ICurrentUserService _currentUser = currentUserService;

            public async Task<CreateTaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
            {
                
                if (!_currentUser.Roles?.Split(',').Contains(Role.Manager.ToString()) ?? true)
                    throw new UnauthorizedAccessException("Only managers can create tasks.");

                var task = request.Adapt<EmployeeManage.Domain.Entities.Tasks.ProjectTask>();
                task.CreatedByUserId = Convert.ToInt16( _currentUser.UserId);

                await _taskRepository.AddAsync(task);
                var affectedRows = await _unitOfWork.CommitAsync();

                return new CreateTaskResponse
                {
                    Id = affectedRows > 0 ? task.Id : 0,
                    IsSuccess = affectedRows > 0,
                    Message = AppConstants.CrudResult
                        .Message(affectedRows, AppConstants.Entity.Task, AppConstants.CrudOperation.Add)
                };
            }
        }
    }
}
