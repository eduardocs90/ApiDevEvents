using AutoMapper;
using AwersomeDevEvents.API.Entities;
using AwersomeDevEvents.API.Models;

namespace AwersomeDevEvents.API.Mappers
{
    public class DevEventProfile : Profile
    {
        public DevEventProfile()
        {
            CreateMap<DevEvent, DevEventViewModel>();
            CreateMap<DevEventSpeaker, DevEventSpeakerViewModel>();

            CreateMap<DevEventInputModel, DevEvent>();
            CreateMap<DevEventSpeakerInputModel, DevEventSpeaker>();
        }
    }
}
