namespace BiddingManagementSystem.Application.DTOs.Bids
{
    public class UpdateBidDto
    {
        public MoneyDto BidAmount { get; set; }
        public string TechnicalProposalSummary { get; set; }
        public List<BidItemDto> BidItems { get; set; }
    }
}
