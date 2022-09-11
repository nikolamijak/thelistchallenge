using Microsoft.AspNetCore.Mvc;
using TheList.TechnicalChallenge.Models;
using MediatR;
using System.Threading.Tasks;
using TheList.TechnicalChallenge.Queries.Requests;

namespace TheList.TechnicalChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CheckoutController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<Checkout> Get(int? id)
            => await _mediator.Send(new CheckoutRequest { Id = id });

    }
}
