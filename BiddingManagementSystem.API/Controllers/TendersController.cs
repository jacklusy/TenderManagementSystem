using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiddingManagementSystem.Application.DTOs.Tenders;
using BiddingManagementSystem.Application.Features.Tenders.Commands;
using BiddingManagementSystem.Application.Features.Tenders.Queries;
using BiddingManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiddingManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TendersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TendersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TenderDto>>> GetAllTenders()
        {
            var result = await _mediator.Send(new GetAllTendersQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TenderDto>> GetTenderById(Guid id)
        {
            var result = await _mediator.Send(new GetTenderByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<TenderDto>>> GetTendersByStatus(TenderStatus status)
        {
            var result = await _mediator.Send(new GetTendersByStatusQuery { Status = status });
            return Ok(result);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<List<TenderDto>>> GetTendersByCategory(string category)
        {
            var result = await _mediator.Send(new GetTendersByCategoryQuery { Category = category });
            return Ok(result);
        }

        [HttpGet("closing-soon/{days}")]
        public async Task<ActionResult<List<TenderDto>>> GetTendersClosingSoon(int days)
        {
            var result = await _mediator.Send(new GetTendersClosingSoonQuery { DaysThreshold = days });
            return Ok(result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpPost]
        public async Task<ActionResult<TenderDto>> CreateTender([FromBody] CreateTenderDto createTenderDto)
        {
            var command = new CreateTenderCommand { TenderDto = createTenderDto };
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTenderById), new { id = result.Id }, result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpPut("{id}")]
        public async Task<ActionResult<TenderDto>> UpdateTender(Guid id, [FromBody] UpdateTenderDto updateTenderDto)
        {
            var command = new UpdateTenderCommand
            {
                Id = id,
                TenderDto = updateTenderDto
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpPost("{id}/publish")]
        public async Task<ActionResult<TenderDto>> PublishTender(Guid id)
        {
            var command = new PublishTenderCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpPost("{id}/start-evaluation")]
        public async Task<ActionResult<TenderDto>> StartTenderEvaluation(Guid id)
        {
            var command = new StartTenderEvaluationCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpPost("{id}/award")]
        public async Task<ActionResult<TenderDto>> AwardTender(Guid id, [FromBody] AwardTenderRequest request)
        {
            var command = new AwardTenderCommand
            {
                TenderId = id,
                BidId = request.BidId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpPost("{id}/close")]
        public async Task<ActionResult<TenderDto>> CloseTender(Guid id)
        {
            var command = new CloseTenderCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<TenderDto>> CancelTender(Guid id, [FromBody] CancelTenderRequest request)
        {
            var command = new CancelTenderCommand
            {
                Id = id,
                Reason = request.Reason
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpPost("{id}/documents")]
        public async Task<ActionResult<TenderDocumentDto>> AddTenderDocument(Guid id, [FromForm] UploadTenderDocumentDto documentDto)
        {
            var command = new AddTenderDocumentCommand
            {
                TenderId = id,
                DocumentDto = documentDto
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,ProcurementOfficer")]
        [HttpDelete("{tenderId}/documents/{documentId}")]
        public async Task<ActionResult> RemoveTenderDocument(Guid tenderId, Guid documentId)
        {
            var command = new RemoveTenderDocumentCommand
            {
                TenderId = tenderId,
                DocumentId = documentId
            };
            await _mediator.Send(command);
            return NoContent();
        }
    }

    public class AwardTenderRequest
    {
        public Guid BidId { get; set; }
    }

    public class CancelTenderRequest
    {
        public string Reason { get; set; }
    }
} 