// Application/Mapping/MappingProfile.cs
using AutoMapper;
using BiddingManagementSystem.Application.DTOs;
using BiddingManagementSystem.Application.DTOs.Auth;
using BiddingManagementSystem.Application.DTOs.Bids;
using BiddingManagementSystem.Application.DTOs.Tenders;
using BiddingManagementSystem.Domain.Aggregates.BidAggregate;
using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using BiddingManagementSystem.Domain.Aggregates.UserAggregate;
using BiddingManagementSystem.Domain.ValueObjects;

namespace BiddingManagementSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Value Objects
            CreateMap<MoneyDto, Money>().ConstructUsing(x => new Money(x.Amount, x.Currency));
            CreateMap<Money, MoneyDto>().ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                                         .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency));
            
            CreateMap<AddressDto, Address>().ConstructUsing(x => new Address(x.Street, x.City, x.State, x.Country, x.ZipCode));
            CreateMap<Address, AddressDto>();
            
            // User
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // Password will be hashed in service
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                
            CreateMap<User, AuthResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
            
            // Tender
            CreateMap<CreateTenderDto, Tender>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.WinningBidId, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Ignore())
                .ForMember(dest => dest.EligibilityCriteria, opt => opt.Ignore())
                .ForMember(dest => dest.Deliverables, opt => opt.Ignore())
                .ForMember(dest => dest.Timeline, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentTerms, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
            
            CreateMap<Tender, TenderDto>();
            CreateMap<TenderDocument, TenderDocumentDto>();
            CreateMap<EligibilityCriteria, TenderEligibilityCriteriaDto>();
            CreateMap<TenderDeliverable, TenderDeliverableDto>();
            CreateMap<TenderActivity, TenderActivityDto>();
            CreateMap<TenderPaymentTerm, TenderPaymentTermDto>();
            
            // Bid
            CreateMap<CreateBidDto, Bid>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.BidderId, opt => opt.Ignore())
                .ForMember(dest => dest.SubmissionDate, opt => opt.Ignore())
                .ForMember(dest => dest.Score, opt => opt.Ignore())
                .ForMember(dest => dest.EvaluationComments, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Ignore())
                .ForMember(dest => dest.BidItems, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
            
            CreateMap<BidItemDto, BidItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                
            CreateMap<Bid, BidDto>();
            CreateMap<BidDocument, BidDocumentDto>();
            CreateMap<BidItem, BidItemDto>();
            
            // Evaluation
            CreateMap<EvaluateBidDto, Bid>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.EvaluationComments, opt => opt.MapFrom(src => src.Comments))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
