using MediatR;

using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Lock.Commands.Handlers {
    public sealed class AddNewLockHandler : IRequestHandler<AddNewLockCommand, Result> {
        private readonly ILockRepository _lockRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddNewLockHandler(ILockRepository lockRepository,
                                 IUnitOfWork unitOfWork) {
            _lockRepository = lockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddNewLockCommand request, CancellationToken cancellationToken) {
            Entities.Lock @lock = Entities.Lock.Create(request.Name, request.Address, request.IsPublic);
            await _lockRepository.AddAsync(@lock);
            await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
            return Result.Successed();
        }
    }
}
