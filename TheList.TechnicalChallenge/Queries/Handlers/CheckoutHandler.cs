using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TheList.TechnicalChallenge.Exceptions;
using TheList.TechnicalChallenge.Models;
using TheList.TechnicalChallenge.Queries.Requests;
using TheList.TechnicalChallenge.Repository;

namespace TheList.TechnicalChallenge.Queries.Handlers
{
    public class CheckoutHandler : IRequestHandler<CheckoutRequest, Checkout>
    {
        private readonly IRepository _repository;

        public CheckoutHandler(IRepository repository) => _repository = repository;
        public async Task<Checkout> Handle(CheckoutRequest request, CancellationToken cancellationToken)
        {
            var checkout = await _repository.GetCheckout(request.Id.Value);
            return checkout switch { 
                null => throw new CheckoutNotFoundException(request.Id),
                _ => checkout,
            };
        }
    }
}
