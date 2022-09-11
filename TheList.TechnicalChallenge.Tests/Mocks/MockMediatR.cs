using MediatR;
using Moq;
using System.Threading;
using TheList.TechnicalChallenge.Models;
using TheList.TechnicalChallenge.Queries.Handlers;
using TheList.TechnicalChallenge.Queries.Requests;
using TheList.TechnicalChallenge.Repository;

namespace TheList.TechnicalChallenge.Tests.Mocks
{
    public static class MockMediatR
    {
        static Mock<IMediator> _mediator;
        static MockMediatR()
        {
            _mediator = new Mock<IMediator>();
        }
        public static Mock<IMediator> GetMediatRWithCheckoutHandler(int? id)
        {
            Mock<IRepository> _mockRepo = MockIRepository.GetRepositoryItems(id.Value);
           
            var handler = new CheckoutHandler(_mockRepo.Object);
            var request = new CheckoutRequest { Id = id };
            _mediator
                    .Setup(m => m.Send(request, CancellationToken.None))                   
                    .Returns(handler.Handle(request,default));

            return _mediator;
        }
    }
}
