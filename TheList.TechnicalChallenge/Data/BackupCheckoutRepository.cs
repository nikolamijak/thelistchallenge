using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheList.TechnicalChallenge.Models;
using TheList.TechnicalChallenge.Repository;

namespace TheList.TechnicalChallenge.Data
{
    public class BackupCheckoutRepository : IRepository
    {
        private static readonly IEnumerable<Checkout> Checkouts = new[]
        {
            new Checkout { Id = 1, TotalPrice = 100 },
            new Checkout { Id = 2, TotalPrice = 200 },
            new Checkout { Id = 3, TotalPrice = 300 }
        };

        public Task<Checkout> GetCheckout(int id)
        {
            return Task.FromResult(Checkouts.FirstOrDefault(checkout => checkout.Id == id));
        }
    }
}
