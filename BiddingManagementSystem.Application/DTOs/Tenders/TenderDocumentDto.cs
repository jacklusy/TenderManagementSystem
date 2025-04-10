using System;

namespace BiddingManagementSystem.Application.DTOs.Tenders
{
    public class TenderDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public string Description { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
    }
}