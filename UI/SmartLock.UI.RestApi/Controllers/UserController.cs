using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SmartLock.Application.User.Commands;
using SmartLock.Application.User.Dtos;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.UI.RestApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public sealed class UserController : ControllerBase {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public Task<Result<AuthenticationResult>> Register(RegisterUserCommand registerCommand) =>
            _mediator.Send(registerCommand);


        [HttpPost]
        [Route("login")]
        public Task<Result<AuthenticationResult>> Login(LoginUserCommand loginCommand) =>
            _mediator.Send(loginCommand);

        [HttpPost]
        [Route("RefreshToken")]
        public Task<Result<AuthenticationResult>> RefreshToken(RefreshTokenCommand tokenCommand) =>
            _mediator.Send(tokenCommand);
    }
}
