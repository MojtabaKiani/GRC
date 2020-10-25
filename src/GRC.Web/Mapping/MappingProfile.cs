using AutoMapper;
using GRC.Core.Entities;
using GRC.Web.Models;

namespace GRC.Web.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Questionary, QuestionaryViewModel>().ReverseMap();
        }
    }
}
