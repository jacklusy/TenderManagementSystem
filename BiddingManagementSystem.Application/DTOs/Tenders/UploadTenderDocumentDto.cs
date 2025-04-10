using Microsoft.AspNetCore.Http;

namespace BiddingManagementSystem.Application.DTOs.Tenders
{
    public class UploadTenderDocumentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}