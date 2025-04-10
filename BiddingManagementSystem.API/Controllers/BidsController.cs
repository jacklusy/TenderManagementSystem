using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiddingManagementSystem.Application.DTOs.Bids;
using BiddingManagementSystem.Application.Features.Bids.Commands;
using BiddingManagementSystem.Application.Features.Bids.Queries;
using BiddingManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiddingManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BidsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<BidDto>>> GetAllBids()
        {
            var result = await _mediator.Send(new GetAllBidsQuery());
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BidDto>> GetBidById(Guid id)
        {
            var result = await _mediator.Send(new GetBidByIdQuery { Id = id });
            return Ok(result);
        }

        [Authorize]
        [HttpGet("tender/{tenderId}")]
        public async Task<ActionResult<List<BidDto>>> GetBidsByTenderId(Guid tenderId)
        {
            var result = await _mediator.Send(new GetBidsByTenderIdQuery { TenderId = tenderId });
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Bidder")]
        [HttpGet("bidder")]
        public async Task<ActionResult<List<BidDto>>> GetCurrentUserBids()
        {
            var result = await _mediator.Send(new GetCurrentUserBidsQuery());
            return Ok(result);
        }

        [Authorize]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<BidDto>>> GetBidsByStatus(BidStatus status)
        {
            var result = await _mediator.Send(new GetBidsByStatusQuery { Status = status });
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Bidder")]
        [HttpPost]
        public async Task<ActionResult<BidDto>> CreateBid([FromBody] CreateBidDto createBidDto)
        {
            var command = new CreateBidCommand { BidDto = createBidDto };
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBidById), new { id = result.Id }, result);
        }

        [Authorize(Roles = "Admin,Bidder")]
        [HttpPut("{id}")]
        public async Task<ActionResult<BidDto>> UpdateBid(Guid id, [FromBody] UpdateBidDto updateBidDto)
        {
            var command = new UpdateBidCommand 
            { 
                Id = id, 
                BidDto = updateBidDto 
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Bidder")]
        [HttpPost("{id}/submit")]
        public async Task<ActionResult<BidDto>> SubmitBid(Guid id)
        {
            var command = new SubmitBidCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Evaluator")]
        [HttpPost("{id}/evaluate")]
        public async Task<ActionResult<BidDto>> EvaluateBid(Guid id, [FromBody] EvaluateBidDto evaluateBidDto)
        {
            var command = new EvaluateBidCommand 
            { 
                Id = id, 
                Score = evaluateBidDto.Score, 
                Comments = evaluateBidDto.Comments 
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Evaluator")]
        [HttpPost("{id}/accept")]
        public async Task<ActionResult<BidDto>> AcceptBid(Guid id)
        {
            var command = new AcceptBidCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Evaluator")]
        [HttpPost("{id}/reject")]
        public async Task<ActionResult<BidDto>> RejectBid(Guid id, [FromBody] RejectBidRequest request)
        {
            var command = new RejectBidCommand 
            { 
                Id = id, 
                Reason = request.Reason 
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Bidder")]
        [HttpPost("{id}/documents")]
        public async Task<ActionResult<BidDocumentDto>> AddBidDocument(Guid id, [FromForm] UploadBidDocumentDto documentDto)
        {
            var command = new AddBidDocumentCommand
            {
                BidId = id,
                DocumentDto = documentDto
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Bidder")]
        [HttpDelete("{bidId}/documents/{documentId}")]
        public async Task<ActionResult> RemoveBidDocument(Guid bidId, Guid documentId)
        {
            var command = new RemoveBidDocumentCommand
            {
                BidId = bidId,
                DocumentId = documentId
            };
            await _mediator.Send(command);
            return NoContent();
        }
    }

    public class RejectBidRequest
    {
        public string Reason { get; set; }
    }
} 