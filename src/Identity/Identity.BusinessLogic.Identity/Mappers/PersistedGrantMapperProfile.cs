using AutoMapper;
using Identity.BusinessLogic.Identity.Dtos.Grant;
using Identity.EntityFramework.Entities;
using Identity.EntityFramework.Extensions.Common;
using IdentityServer8.EntityFramework.Entities;

namespace Identity.BusinessLogic.Identity.Mappers
{
    public class PersistedGrantMapperProfile : Profile
    {
        public PersistedGrantMapperProfile()
        {
            // entity to model
            CreateMap<PersistedGrant, PersistedGrantDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<PersistedGrantDataView, PersistedGrantDto>(MemberList.Destination);

            CreateMap<PagedList<PersistedGrantDataView>, PersistedGrantsDto>(MemberList.Destination)
                .ForMember(x => x.PersistedGrants,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<PersistedGrant>, PersistedGrantsDto>(MemberList.Destination)
                .ForMember(x => x.PersistedGrants,
                    opt => opt.MapFrom(src => src.Data));
        }
    }
}
