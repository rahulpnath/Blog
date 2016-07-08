using System.Linq;

namespace RefactoringForReadability
{
    public class OverlappingSubscriptionTermWithConflictingPriceValidator : IValidate
    {
        public bool Validate(Subscription subscription)
        {       
            var hasOverlappingItems = false;
            foreach (var term in subscription.Terms)
            {
                var otherTerms = subscription.Terms.Where(a => a.Price != term.Price);
                if (otherTerms.Any())
                {
                    if (
                   ( !term.EndDate.HasValue && otherTerms.Any(a => term.StartDate < a.EndDate)) ||
                    (otherTerms.Where(a => !a.EndDate.HasValue).Any(a => a.StartDate < term.EndDate)) ||
                    (otherTerms.Any(a => term.StartDate <= a.EndDate && a.StartDate <= term.EndDate))
                    )
                    {
                        hasOverlappingItems = true;
                        break;
                    }
                }
            }

            return hasOverlappingItems;
        }
    }
}
