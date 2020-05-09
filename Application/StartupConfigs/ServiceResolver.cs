
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace UniqueWords.Application.StartupConfigs
{
    public class ServiceResolver : IServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TService GetService<TService>()
        {
            var service = _serviceProvider.GetService<TService>();                

            return service;
        }

        public TService GetService<TService, TImplementation>() where TImplementation : TService
        {
            var service = _serviceProvider.GetServices<TService>()
                .First(s => s.GetType() == typeof(TImplementation));

            return service;
        }
    }
}