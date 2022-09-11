using System.Threading.Tasks;
using TheList.TechnicalChallenge.Models;

namespace TheList.TechnicalChallenge.Repository
{
    public interface IRepository
    {
        public Task<Checkout> GetCheckout(int id);
    }
}
