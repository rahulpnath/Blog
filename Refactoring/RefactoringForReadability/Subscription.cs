using System.Collections.Generic;

namespace RefactoringForReadability
{
    public class Subscription
    {
        public List<SubscriptionTerm> Terms { get; set; }
        public Subscription()
        {
            this.Terms = new List<SubscriptionTerm>();
        }
    }
}
