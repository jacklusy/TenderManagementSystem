namespace BiddingManagementSystem.Application.DTOs.Tenders
{
    public class UpdateTenderDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Category { get; set; }
        public MoneyDto BudgetRange { get; set; }
        public string ContactEmail { get; set; }
    }
}
