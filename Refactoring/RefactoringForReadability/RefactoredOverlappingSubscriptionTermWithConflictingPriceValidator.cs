using System.Linq;

namespace RefactoringForReadability
{
    public class RefactoredOverlappingSubscriptionTermWithConflictingPriceValidator : IValidate
    {
        public bool Validate(Subscription subscription)
        {
            foreach (var term in subscription.Terms)
            {
                var termsWithDifferentPrice = subscription.Terms.Where(a => a.Price != term.Price);
                return termsWithDifferentPrice
                    .Any(a => a.IsOverlapping(term));
            }

            return false;
        }
    }
}
