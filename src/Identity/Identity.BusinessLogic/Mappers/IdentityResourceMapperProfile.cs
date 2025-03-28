﻿using AutoMapper;
using Identity.BusinessLogic.Dtos.Configuration;
using Identity.EntityFramework.Extensions.Common;
using IdentityServer8.EntityFramework.Entities;

namespace Identity.BusinessLogic.Mappers
{
    public class IdentityResourceMapperProfile : Profile
    {
        public IdentityResourceMapperProfile()
        {
            // entity to model
            CreateMap<IdentityResource, IdentityResourceDto>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            CreateMap<IdentityResourceProperty, IdentityResourcePropertyDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<IdentityResourceProperty, IdentityResourcePropertiesDto>(MemberList.Destination)
                .ForMember(dest => dest.Key, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.IdentityResourcePropertyId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.IdentityResourceId, opt => opt.MapFrom(x => x.IdentityResource.Id));

            CreateMap<PagedList<IdentityResource>, IdentityResourcesDto>(MemberList.Destination)
                .ForMember(x => x.IdentityResources,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<IdentityResourceProperty>, IdentityResourcePropertiesDto>(MemberList.Destination)
                .ForMember(x => x.IdentityResourceProperties, opt => opt.MapFrom(src => src.Data));

            // model to entity
            CreateMap<IdentityResourceDto, IdentityResource>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new IdentityResourceClaim { Type = x })));

            CreateMap<IdentityResourcePropertiesDto, IdentityResourceProperty>(MemberList.Source)
                .ForMember(x => x.IdentityResource, dto => dto.MapFrom(src => new IdentityResource() { Id = src.IdentityResourceId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.IdentityResourcePropertyId));
        }
    }
}