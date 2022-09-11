using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TheList.TechnicalChallenge.Data;
using TheList.TechnicalChallenge.Repository;
using Xunit;

namespace TheList.TechnicalChallenge.Tests.Repository
{
    public class RepositoryInjectionTests
    {

        [Theory]
        [InlineData("Backup", typeof(BackupCheckoutRepository))]
        [InlineData("Checkout", typeof(CheckoutRepository))]
        [InlineData("Random", typeof(CheckoutRepository))]
        [InlineData("", typeof(CheckoutRepository))]
        public void Check_If_CorrectRepo_Is_Injected(string datastoreType, Type expectedRepo)
        {

            //Arrange
            var inMemorySettings = new Dictionary<string, string> {
                    {"DataStoreType", datastoreType}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            IServiceCollection services = new ServiceCollection();
            // Act
            services.AddRepository(configuration);
            //Assert
            var serviceProvider = services.BuildServiceProvider();
            var repo = serviceProvider.GetRequiredService<IRepository>();
            Assert.Equal(repo.GetType(), expectedRepo);
        }
    }
}
