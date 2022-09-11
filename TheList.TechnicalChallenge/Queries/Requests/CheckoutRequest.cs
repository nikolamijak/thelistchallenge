using MediatR;
using TheList.TechnicalChallenge.Models;

namespace TheList.TechnicalChallenge.Queries.Requests
{
    public class CheckoutRequest : IRequest<Checkout>
    {
        public int? Id { get; set; }
    }
}
