using System;

namespace BiddingManagementSystem.Application.DTOs.Bids
{
    public class BidDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public string DocumentType { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
    }
}