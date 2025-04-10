using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiddingManagementSystem.Application.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendEmailAsync(string to, string subject, string body, List<string> attachments, bool isHtml = false);
        Task SendEmailToMultipleRecipientsAsync(List<string> recipients, string subject, string body, bool isHtml = false);
    }
} 