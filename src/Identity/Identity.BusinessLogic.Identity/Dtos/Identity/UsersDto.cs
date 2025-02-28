using Identity.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Identity.BusinessLogic.Identity.Dtos.Identity
{
    public class UsersDto<TUserDto, TKey> : IUsersDto where TUserDto : UserDto<TKey>
    {
        public UsersDto()
        {
            Users = new List<TUserDto>();
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<TUserDto> Users { get; set; }

        List<IUserDto> IUsersDto.Users => Users.Cast<IUserDto>().ToList();
    }
}
