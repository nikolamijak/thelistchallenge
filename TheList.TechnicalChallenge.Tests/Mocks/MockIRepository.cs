using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheList.TechnicalChallenge.Models;
using TheList.TechnicalChallenge.Repository;

namespace TheList.TechnicalChallenge.Tests.Mocks
{
    public static class MockIRepository
    {
        static Mock<IRepository> mockRepo;
        static IEnumerable<Checkout> repoItems;
        static MockIRepository()
        {
            mockRepo = new Mock<IRepository>();
            repoItems = new List<Checkout>
            {
                new Checkout { Id = 1, TotalPrice = 100 },
                new Checkout { Id = 2, TotalPrice = 200 },
                new Checkout { Id = 3, TotalPrice = 300 }
            }; 
        }
        public static Mock<IRepository> GetRepositoryItems(int id)
        {            

            mockRepo
                .Setup(r => r.GetCheckout(id))
                .ReturnsAsync(repoItems.Where(x=>x.Id == id).FirstOrDefault());

            return mockRepo;
        }
    }


}
