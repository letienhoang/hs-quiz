﻿using Identity.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Identity.BusinessLogic.Identity.Dtos.Identity
{
    public class RolesDto<TRoleDto, TKey> : IRolesDto where TRoleDto : RoleDto<TKey>
    {
        public RolesDto()
        {
            Roles = new List<TRoleDto>();
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<TRoleDto> Roles { get; set; }

        List<IRoleDto> IRolesDto.Roles => Roles.Cast<IRoleDto>().ToList();
    }
}
