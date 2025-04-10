using System;
using BiddingManagementSystem.Domain.Common;

namespace BiddingManagementSystem.Domain.Aggregates.BidAggregate
{
    public class BidDocument : BaseEntity
    {
        public string Name { get; private set; }
        public string FileUrl { get; private set; }
        public string DocumentType { get; private set; }
        public string FileType { get; private set; }
        public long FileSize { get; private set; }
        
        private BidDocument() { }
        
        public BidDocument(string name, string fileUrl, string documentType, string fileType, long fileSize)
        {
            Name = name;
            FileUrl = fileUrl;
            DocumentType = documentType;
            FileType = fileType;
            FileSize = fileSize;
        }
    }
}