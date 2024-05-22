using AutoMapper;
using FunlabProgramChallenge.Models;
using FunlabProgramChallenge.ViewModels;

namespace FunlabProgramChallenge.Utility
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Member, MemberViewModel>().ReverseMap();
        }
    }
}
