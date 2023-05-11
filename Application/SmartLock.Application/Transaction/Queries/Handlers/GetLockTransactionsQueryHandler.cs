using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Application.Transaction.Dtos;
using SmartLock.Domain.Models;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Infrastructure;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.Transaction.Queries.Handlers {
    public sealed class GetLockTransactionsQueryHandler : IRequestHandler<GetLockTransactionsQuery, Result<TransactionResultDto>> {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserManagment _userManagment;
        private readonly IUserRepository _userRepository;
        private readonly ILockRepository _lockRepository;
        private readonly IObjectMapper _mapper;

        public GetLockTransactionsQueryHandler(ITransactionRepository transactionRepository,
                                               IUserManagment userManagment,
                                               IUserRepository userRepository,
                                               ILockRepository lockRepository,
                                               IObjectMapper mapper) {
            _transactionRepository = transactionRepository;
            _userManagment = userManagment;
            _userRepository = userRepository;
            _lockRepository = lockRepository;
            _mapper = mapper;
        }

        public async Task<Result<TransactionResultDto>> Handle(GetLockTransactionsQuery request, CancellationToken cancellationToken) {
            if (!await _lockRepository.Exists(request.LockId))
                return Result<TransactionResultDto>.BadRequest(ErrorMessages.LockNotFound);

            if (string.Equals(_userManagment.CurrentUser.Role.Key, RoleKeys.User) &&
                !await _userRepository.HasAccessToLock(_userManagment.CurrentUser.Id, request.LockId))
                return new Result<TransactionResultDto>(ResultCode.Unauthorized);

            TransactionResult result = await _transactionRepository.GetAll(request.LockId, request.First, request.After);
            return new Result<TransactionResultDto>() {
                Data = _mapper.Map<TransactionResultDto>(result)
            };
        }
    }
}
