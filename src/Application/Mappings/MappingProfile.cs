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
using System.Linq;

namespace EventManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<Location, LocationDto>();
            CreateMap<CreateLocationDto, Location>();

            CreateMap<UpdateLocationDto, Location>();
            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomDto, Room>();

            CreateMap<UpdateRoomDto, Room>();
            CreateMap<Speaker, SpeakerDto>();
            CreateMap<CreateSpeakerDto, Speaker>();
            CreateMap<UpdateSpeakerDto, Speaker>();

            CreateMap<Participant, ParticipantDto>();
            CreateMap<CreateParticipantDto, Participant>();
            CreateMap<UpdateParticipantDto, Participant>();

            CreateMap<Session, SessionDto>()
                .ForMember(dest => dest.Speakers, opt => opt.MapFrom(src => src.SessionSpeakers.Select(ss => ss.Speaker)));

            CreateMap<CreateSessionDto, Session>();
            CreateMap<UpdateSessionDto, Session>();
            CreateMap<Event, EventListDto>();
            CreateMap<Event, EventDetailDto>()
                .ForMember(dest => dest.Sessions, opt => opt.MapFrom(src => src.Sessions))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.EventParticipants.Select(ep => ep.Participant)));

            CreateMap<CreateEventDto, Event>();
            CreateMap<UpdateEventDto, Event>();

            CreateMap<Rating, RatingDto>()
                .ForMember(dest => dest.SessionTitle, opt => opt.MapFrom(src => src.Session.Title))
                .ForMember(dest => dest.ParticipantName, opt =>
                    opt.MapFrom(src => $"{src.Participant.FirstName} {src.Participant.LastName}"));
            CreateMap<CreateRatingDto, Rating>();
            CreateMap<UpdateRatingDto, Rating>();
        }
    }
}