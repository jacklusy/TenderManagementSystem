using Microsoft.AspNetCore.Mvc;

namespace BiddingManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Get API status and information
        /// </summary>
        /// <returns>API status information</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new { 
                message = "Bidding Management System API is running",
                endpoints = new[] {
                    "/api/auth - Authentication endpoints",
                    "/api/tenders - Tender management endpoints",
                    "/api/bids - Bid management endpoints",
                    "/api/users - User management endpoints"
                }
            });
        }
    }
} 