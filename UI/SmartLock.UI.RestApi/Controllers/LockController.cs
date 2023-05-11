using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SmartLock.Application.Lock.Commands;
using SmartLock.Application.Transaction.Dtos;
using SmartLock.Application.Transaction.Queries;
using SmartLock.Domain.Entities;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.UI.RestApi.Controllers {

    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public sealed class LockController : ControllerBase {
        private readonly IMediator _mediator;

        public LockController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleKeys.Admin}")]
        public Task<Result<Lock>> Add(AddNewLockCommand command) =>
            _mediator.Send(command);

        [HttpPut]
        [Authorize(Roles = $"{RoleKeys.Admin}")]
        public Task<Result> Edit(EditLockCommand command) =>
            _mediator.Send(command);

        [HttpPatch]
        [Route("Grant")]
        [Authorize(Roles = $"{RoleKeys.Admin},{RoleKeys.User}")]
        public Task<Result> GrantAccess(GrantLockAccessToUserCommand command) =>
            _mediator.Send(command);

        [HttpPatch]
        [Route("Deny")]
        [Authorize(Roles = $"{RoleKeys.Admin},{RoleKeys.User}")]
        public Task<Result> DenyAccess(DenyLockAccessFromUserCommand command) =>
            _mediator.Send(command);

        [HttpPatch]
        [Route("Unlock")]
        [Authorize(Roles = $"{RoleKeys.Admin},{RoleKeys.User},{RoleKeys.System}")]
        public Task<Result> Unlock(UnlockCommand command) =>
            _mediator.Send(command);

        [HttpPatch]
        [Authorize(Roles = $"{RoleKeys.System}")]
        public Task<Result> Lock(LockCommand command) =>
            _mediator.Send(command);

        [HttpGet]
        [Authorize(Roles = $"{RoleKeys.Admin},{RoleKeys.User}")]
        [Route("Transactions")]
        public Task<Result<TransactionResultDto>> GetTransactions([FromQuery] GetLockTransactionsQuery query) =>
            _mediator.Send(query);
    }
}
