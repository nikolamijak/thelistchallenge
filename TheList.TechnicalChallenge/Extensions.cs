using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheList.TechnicalChallenge.Behaviours;
using TheList.TechnicalChallenge.Data;
using TheList.TechnicalChallenge.Repository;

namespace TheList.TechnicalChallenge
{
    public static class Extensions
    {
        public static IServiceCollection AddFluentValidators(this IServiceCollection serviceCollection)
        {
            var iValidatorImplementations = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetLoadableTypes())
            .Where(p => p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
            && !p.IsAbstract && !p.IsInterface && !p.IsNestedPrivate && p.IsClass && !p.FullName!.ToLowerInvariant().StartsWith("fluentvalidation", StringComparison.InvariantCultureIgnoreCase));

            foreach (var impl in iValidatorImplementations)
            {
                var serviceType = impl.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IValidator<>));
                serviceCollection.AddTransient(serviceType, impl);
            }

            return serviceCollection;
        }

        private static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {           
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e?.Types?.Where(t => t != null)!;
            }
        }

        public static IServiceCollection AddRepository(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
          serviceCollection.AddTransient(_ =>
          {
              IRepository repoDelegate =
                  configuration.GetSection("DataStoreType") switch
                  {
                      IConfigurationSection configSection when configSection.Value != null &&
                      configSection.Value.Equals("Backup", StringComparison.InvariantCultureIgnoreCase) => new BackupCheckoutRepository(),
                      _ => new CheckoutRepository()
                  };
              return repoDelegate;
          });

            return serviceCollection;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
