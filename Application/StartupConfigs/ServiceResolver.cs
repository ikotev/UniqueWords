
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

        public TService GetService<TService, TImplementation>()
        {            
            var service = _serviceProvider.GetServices<TService>()
                .First(s => s.GetType() == typeof(TImplementation));

            return service;
        }
    }
}