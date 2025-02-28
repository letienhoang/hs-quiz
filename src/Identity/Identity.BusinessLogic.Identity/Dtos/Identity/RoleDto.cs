using Identity.BusinessLogic.Identity.Dtos.Identity.Base;
using Identity.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Identity.BusinessLogic.Identity.Dtos.Identity
{
    public class RoleDto<TKey> : BaseRoleDto<TKey>, IRoleDto
    {
        [Required]
        public string Name { get; set; }
    }
}
