﻿using Identity.EntityFramework.Entities;
using Identity.EntityFramework.Extensions.Common;
using IdentityServer8.EntityFramework.Entities;

namespace Identity.BusinessLogic.Identity.Repositories.Interfaces
{
    public interface IPersistedGrantAspNetIdentityRepository
    {
        Task<PagedList<PersistedGrantDataView>> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<PagedList<PersistedGrant>> GetPersistedGrantsByUserAsync(string subjectId, int page = 1, int pageSize = 10);
        Task<PersistedGrant> GetPersistedGrantAsync(string key);
        Task<int> DeletePersistedGrantAsync(string key);
        Task<int> DeletePersistedGrantsAsync(string userId);
        Task<bool> ExistsPersistedGrantsAsync(string subjectId);
        Task<bool> ExistsPersistedGrantAsync(string key);
        Task<int> SaveAllChangesAsync();
        bool AutoSaveChanges { get; set; }
    }
}
