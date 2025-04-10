using System;
using BiddingManagementSystem.Domain.Common;

namespace BiddingManagementSystem.Domain.Aggregates.TenderAggregate
{
    public class TenderDocument : BaseEntity
    {
        public string Name { get; private set; }
        public string FileUrl { get; private set; }
        public string Description { get; private set; }
        public string FileType { get; private set; }
        public long FileSize { get; private set; }
        
        private TenderDocument() { }
        
        public TenderDocument(string name, string fileUrl, string description, string fileType, long fileSize)
        {
            Name = name;
            FileUrl = fileUrl;
            Description = description;
            FileType = fileType;
            FileSize = fileSize;
        }
    }
}