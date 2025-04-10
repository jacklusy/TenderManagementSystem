using System;
using BiddingManagementSystem.Domain.Common;

namespace BiddingManagementSystem.Domain.Aggregates.TenderAggregate
{
    public class EligibilityCriteria : BaseEntity
    {
        public string CriteriaName { get; private set; }
        public string CriteriaDescription { get; private set; }
        
        private EligibilityCriteria() { }
        
        public EligibilityCriteria(string criteriaName, string criteriaDescription)
        {
            CriteriaName = criteriaName;
            CriteriaDescription = criteriaDescription;
        }
    }
}