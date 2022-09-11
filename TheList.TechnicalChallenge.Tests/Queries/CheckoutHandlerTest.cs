using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TheList.TechnicalChallenge.Exceptions;
using TheList.TechnicalChallenge.Queries.Handlers;
using TheList.TechnicalChallenge.Queries.Requests;
using TheList.TechnicalChallenge.Repository;
using TheList.TechnicalChallenge.Tests.Mocks;
using Xunit;

namespace TheList.TechnicalChallenge.Tests.Queries
{

    public class CheckoutHandlerTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]       
        public async Task Check_If_Correct_Handler_Is_Invoked(int id)
        {
            Mock<IRepository> _mockRepo = MockIRepository.GetRepositoryItems(id);
            var handler = new CheckoutHandler(_mockRepo.Object);
            var result = await handler.Handle(new CheckoutRequest() { Id = id }, CancellationToken.None);

            Assert.True(result.Id == id);

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(200)]
        [InlineData(5000)]       
        public async Task Check_If_Correct_Exception_Is_Thrown(int id)
        {
            Mock<IRepository> _mockRepo = MockIRepository.GetRepositoryItems(id);
            var handler = new CheckoutHandler(_mockRepo.Object);
           
            Func<Task> act = async () => await handler.Handle(new CheckoutRequest() { Id = id }, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<CheckoutNotFoundException>(act);

        }
    }
}
