using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.EntityFramework.Shared.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Identity.API.Database.Configuration
{
    public class UserIdentityEntityConfiguration : IEntityTypeConfiguration<UserIdentity>
    {
        public void Configure(EntityTypeBuilder<UserIdentity> builder)
        {
        }
    }
}