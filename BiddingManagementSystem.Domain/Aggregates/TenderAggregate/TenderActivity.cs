using System;
using BiddingManagementSystem.Domain.Common;

namespace BiddingManagementSystem.Domain.Aggregates.TenderAggregate
{
    public class TenderActivity : BaseEntity
    {
        public string ActivityName { get; private set; }
        public DateTime ExpectedDate { get; private set; }
        
        private TenderActivity() { }
        
        public TenderActivity(string activityName, DateTime expectedDate)
        {
            ActivityName = activityName;
            ExpectedDate = expectedDate;
        }
    }
}
