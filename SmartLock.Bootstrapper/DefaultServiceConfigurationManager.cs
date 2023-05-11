using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SmartLock.Application.Configurations;
using SmartLock.Application.Configurations.Models;
using SmartLock.Application.Lock.Dtos;
using SmartLock.Application.PipeLines;
using SmartLock.Application.Services;
using SmartLock.Application.Services.Impl;
using SmartLock.Application.Transaction.Dtos;
using SmartLock.Application.User.Dtos;
using SmartLock.Domain.Entities;
using SmartLock.Domain.Models;
using SmartLock.Domain.Repositories;
using SmartLock.Infrastructure.DataBase;
using SmartLock.Infrastructure.Domain;
using SmartLock.Infrastructure.Domain.Lock;
using SmartLock.Infrastructure.Domain.Role;
using SmartLock.Infrastructure.Domain.Trasaction;
using SmartLock.Infrastructure.Domain.User;
using SmartLock.Shared.Abstraction.Domain;
using SmartLock.Shared.Abstraction.Infrastructure;
using SmartLock.Shared.Abstraction.Services;
using SmartLock.Shared.Infrastructure;
using SmartLock.Shared.Options;
using SmartLock.Shared.Services;

namespace SmartLock.Bootstrapper {
    public class DefaultServiceConfigurationManager : IServiceCollectionConfigurationManager {
        public virtual IServiceCollectionConfigurationManager RegisterRepositories(IServiceCollection service) {
            service.AddScoped<ILockRepository, LockRepository>()
                   .AddScoped<IUserRepository, UserRepository>()
                   .AddScoped<IUnitOfWork, UnitOfWork>()
                   .AddScoped<IRoleRepository, RoleRepository>()
                   .AddScoped<ITransactionRepository, TransactionRepository>();
            return this;
        }

        public virtual IServiceCollectionConfigurationManager RegisterServices(IServiceCollection service) {
            RegisterMediatR(service)
            .AddScoped<IDomainEventsDispatcher, DomainEventDispatcher>()
            .AddSingleton<ICryptoService, CryptoService>()
            .AddScoped<IAuthenticationProvider, JwtAuthenticationProvider>()
            .AddScoped<IUserManagment, UserManagment>()
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorePipeLineBehavior<,>))
            .AddScoped<ITransactionManagerService, TransactionManagerService>()
            .AddSingleton(p => p.GetService<IConfiguration>().GetOptions<Audience>("audience"))
            .AddSingleton(p => p.GetService<IConfiguration>().GetOptions<RefreshTokenConfig>("refreshToken"));
            return this;
        }

        public virtual IServiceCollectionConfigurationManager RegisterUtils(IServiceCollection service) {
            MapperConfiguration config = new(ConfigDataModels);
            IMapper mapper = config.CreateMapper();
            service.AddSingleton(mapper)
                   .AddSingleton<ICacheService, LocalCacheService>()
                   .AddSingleton<IObjectMapper, AutoMapperWrapper>()
                   .AddSingleton<SqlServerInitializer>()
                   .AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);
            return this;
        }

        public virtual void ConfigDataModels(IMapperConfigurationExpression mapper) {
            mapper.CreateMap<Lock, LockDto>()
                  .ConstructUsing(src => new LockDto(src.Name, src.Address, src.IsPublic, src.IsLocked));

            mapper.CreateMap<User, UserDto>()
                  .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new RoleDto(src.Role.Name, src.Role.Key)));

            mapper.CreateMap<Role, RoleDto>();

            mapper.CreateMap<Transaction, TransactionDto>()
                  .ConstructUsing((src, context) =>
                    new TransactionDto(context.Mapper.Map<LockDto>(src.Lock),
                                       context.Mapper.Map<UserDto>(src.User),
                                       src.Action,
                                       src.Success,
                                       src.CreateDate));

            mapper.CreateMap<TransactionResult, TransactionResultDto>()
                  .ConstructUsing((src, context) =>
                        new TransactionResultDto(src.Transactions.Select(t => context.Mapper.Map<TransactionDto>(t)),
                                                 src.TotalCount, src.First, src.After));
        }

        #region Private Methods
        private static IServiceCollection RegisterMediatR(IServiceCollection service) {
            service.AddMediatR(MediatRConfiguration.ConfigureMediatR);
            return service;
        }
        #endregion
    }
}
