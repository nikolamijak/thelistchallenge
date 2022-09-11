using System;

namespace TheList.TechnicalChallenge.Exceptions
{
    public class CheckoutNotFoundException : Exception
    {
        public string Code { get; } = "checkout_not_found";

        public CheckoutNotFoundException(int? id) : base($"Checkout not found for id: {id}")
        { }
    }
}
