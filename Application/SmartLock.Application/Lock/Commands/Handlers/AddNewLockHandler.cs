using MediatR;

using SmartLock.Domain.Entities;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Lock.Commands.Handlers {
    public sealed class AddNewLockHandler : IRequestHandler<AddNewLockCommand, Result<Entities.Lock>> {
        private readonly ILockRepository _lockRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddNewLockHandler(ILockRepository lockRepository,
                                 IUnitOfWork unitOfWork) {
            _lockRepository = lockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Entities.Lock>> Handle(AddNewLockCommand request, CancellationToken cancellationToken) {
            Entities.Lock @lock = Entities.Lock.Create(request.Name, request.Address, request.IsPublic);
            await _lockRepository.AddAsync(@lock);
            await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
            return new Result<Entities.Lock>() {
                Data = @lock
            };
        }
    }
}
