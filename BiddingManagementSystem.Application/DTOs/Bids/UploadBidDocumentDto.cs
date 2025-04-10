using Microsoft.AspNetCore.Http;

namespace BiddingManagementSystem.Application.DTOs.Bids
{
    public class UploadBidDocumentDto
    {
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public IFormFile File { get; set; }
    }
}