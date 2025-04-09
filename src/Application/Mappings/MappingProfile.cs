using AutoMapper;
using EventManagement.Application.DTOs.Category;
using EventManagement.Application.DTOs.Event;
using EventManagement.Application.DTOs.Location;
using EventManagement.Application.DTOs.Participant;
using EventManagement.Application.DTOs.Rating;
using EventManagement.Application.DTOs.Room;
using EventManagement.Application.DTOs.Session;
using EventManagement.Application.DTOs.Speaker;
using EventManagement.Domain.Entities;
using System.Linq; // Required for Select in mapping collections

namespace EventManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category Mappings
            CreateMap<Category, CategoryDto>();
            // Add mappings for Create/Update DTOs if needed (usually handled in service)
            CreateMap<CreateCategoryDto, Category>(); // Map Create DTO to Entity
            CreateMap<UpdateCategoryDto, Category>(); // Map Update Category DTO to Entity

            // Location Mappings
            CreateMap<Location, LocationDto>();
            CreateMap<CreateLocationDto, Location>(); // Map Create Location DTO to Entity

            CreateMap<UpdateLocationDto, Location>(); // Map Update Location DTO to Entity
            // Room Mappings
            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomDto, Room>(); // Map Create Room DTO to Entity

            CreateMap<UpdateRoomDto, Room>(); // Map Update Room DTO to Entity
            // Speaker Mappings
            CreateMap<Speaker, SpeakerDto>();
            CreateMap<CreateSpeakerDto, Speaker>(); // Map Create DTO to Entity
            CreateMap<UpdateSpeakerDto, Speaker>(); // Map Update DTO to Entity

            // Participant Mappings
            CreateMap<Participant, ParticipantDto>();
            CreateMap<CreateParticipantDto, Participant>(); // Map Create DTO to Entity
            CreateMap<UpdateParticipantDto, Participant>(); // Map Update DTO to Entity

            // Session Mappings
            CreateMap<Session, SessionDto>()
                // Map Speakers list within Session
                .ForMember(dest => dest.Speakers, opt => opt.MapFrom(src => src.SessionSpeakers.Select(ss => ss.Speaker)));
                // Room is mapped automatically by name convention (Room -> RoomDto)

            CreateMap<CreateSessionDto, Session>(); // Map Create Session DTO to Entity
            // Event Mappings
            CreateMap<UpdateSessionDto, Session>(); // Map Update Session DTO to Entity
            CreateMap<Event, EventListDto>(); // Category and Location mapped automatically
            CreateMap<Event, EventDetailDto>()
                // Map Sessions list within Event
                .ForMember(dest => dest.Sessions, opt => opt.MapFrom(src => src.Sessions)) // Assumes Session -> SessionDto map exists
                // Map Participants list within Event
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.EventParticipants.Select(ep => ep.Participant))); // Assumes Participant -> ParticipantDto map exists
                // Category and Location mapped automatically

            CreateMap<CreateEventDto, Event>(); // Map Create DTO to Entity
            CreateMap<UpdateEventDto, Event>(); // Map Update DTO to Entity

            // Rating Mappings
            CreateMap<Rating, RatingDto>()
                .ForMember(dest => dest.SessionTitle, opt => opt.MapFrom(src => src.Session.Title))
                .ForMember(dest => dest.ParticipantName, opt => 
                    opt.MapFrom(src => $"{src.Participant.FirstName} {src.Participant.LastName}"));
            CreateMap<CreateRatingDto, Rating>(); // Map Create DTO to Entity
            CreateMap<UpdateRatingDto, Rating>(); // Map Update DTO to Entity
        }
    }
}