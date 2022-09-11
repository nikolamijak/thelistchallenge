using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheList.TechnicalChallenge.Controllers;
using TheList.TechnicalChallenge.Exceptions;
using TheList.TechnicalChallenge.Models;
using TheList.TechnicalChallenge.Queries.Handlers;
using TheList.TechnicalChallenge.Queries.Requests;
using TheList.TechnicalChallenge.Repository;
using TheList.TechnicalChallenge.Tests.Mocks;
using Xunit;

namespace TheList.TechnicalChallenge.Tests.ControllersTests
{
    public class CheckoutControllerTests
    {        
        [Theory]       
        [InlineData(1234,"Backup", typeof(CheckoutNotFoundException))]
        [InlineData(500,"Random", typeof(CheckoutNotFoundException))]
        [InlineData(-1, "Backup", typeof(CustomValidationException))]
        [InlineData(-10, "Backup", typeof(CustomValidationException))]
        [InlineData(null, "Random", typeof(CustomValidationException))]
        [InlineData(1, "Backup",null)]
        [InlineData(2, "Random",null)]
        [InlineData(3, "Random", null)]
        public async Task Check_Get_Response_CheckoutNotFoundWithDecorators(int? id, string datastoreType, Type exceptionType)
        {
            //Arrange
            ServiceCollection services = new ServiceCollection();
            var inMemorySettings = new Dictionary<string, string> {
                    {"DataStoreType", datastoreType}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            services
                .AddMediator()
                .AddFluentValidators()
                .AddRepository(configuration);
            var sp = services.BuildServiceProvider();
            var mediator = sp.GetRequiredService<IMediator>();
            var sut = new CheckoutController(mediator);

            //Act
            Func<Task> act = async () => await sut.Get(id);

            //Assert
            if (exceptionType != null)
                await Assert.ThrowsAsync(exceptionType, act);
            else
                Assert.NotNull(act);

        }
    }
}
