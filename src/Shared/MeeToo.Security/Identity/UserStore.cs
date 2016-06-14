using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MeeToo.DataAccess.DocumentDb;
using MeeToo.Security.Options;

namespace MeeToo.Security.Identity
{
    public class UserStore : IUserLoginStore<MeeTooUser>, IUserRoleStore<MeeTooUser>, IUserClaimStore<MeeTooUser>, IUserPasswordStore<MeeTooUser>, IUserEmailStore<MeeTooUser>
    {
        private readonly IStorage storage;
        private readonly ILookupNormalizer idNormalizer;
        private readonly IOptions<UserIdentityOptions> identityOptions;

        public UserStore(IStorage storage,
                         ILookupNormalizer idNormalizer,
                         IOptions<UserIdentityOptions> identityOptions)
        {
            this.storage = storage;
            this.idNormalizer = idNormalizer;
            this.identityOptions = identityOptions;
        }

        #region Implementation Of IUserLoginStore<MeeTooUser>

        public Task<string> GetUserIdAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(idNormalizer.Normalize(user.Id));
        }

        public async Task<string> GetUserNameAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                return user.UserName;
            }

            if (string.IsNullOrEmpty(user.Id))
            {
                return null;
            }

            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(qu => qu.Id == id)
                             .Select(qu => qu.UserName)
                             .FirstOrDefault();
        }

        public Task SetUserNameAsync(MeeTooUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(MeeTooUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult>  CreateAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            try
            {
                var identities = await CollectionAsync().ConfigureAwait(false);

                await identities.AddAsync(user, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError {Description = ex.ToString()});
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            try
            {
                var identities = await CollectionAsync().ConfigureAwait(false);

                await identities.UpdateAsync(user).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError {Description = ex.ToString()});
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            try
            {
                var identities = await CollectionAsync().ConfigureAwait(false);

                await identities.DeleteAsync(user).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.ToString() });
            }

            return IdentityResult.Success;
        }

        public async Task<MeeTooUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(userId);

            return identities.Query()
                             .FirstOrDefault(iu => iu.Id == id);
        }

        public async Task<MeeTooUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);

            return identities.Query()
                             .FirstOrDefault(qu => qu.UserName == normalizedUserName);
        }

        public Task AddLoginAsync(MeeTooUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return default(Task);
        }

        public Task RemoveLoginAsync(MeeTooUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return default(Task);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            return default(Task<IList<UserLoginInfo>>);
        }

        public Task<MeeTooUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return default(Task<MeeTooUser>);
        }

        #endregion

        #region Implementation Of IUserRoleStore<MeeTooUser>

        public Task AddToRoleAsync(MeeTooUser user, string roleName, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            user.Roles.Add(roleName);

            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(MeeTooUser user, string roleName, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            user.Roles.Remove(roleName);

            return Task.CompletedTask;
        }

        public async Task<IList<string>> GetRolesAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            var roles = identities.Query()
                                  .Where(qu => qu.Id == id)
                                  .SelectMany(qu => qu.Roles)
                                  .ToList();

            return roles;
        }

        public async Task<bool> IsInRoleAsync(MeeTooUser user, string roleName, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);

            var result = identities.Query()
                                   .FirstOrDefault(qu => qu.Roles.Contains(roleName));

            return result != null;
        }

        public async Task<IList<MeeTooUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);

            return identities.Query()
                             .Where(qu => qu.Roles.Contains(roleName))
                             .ToList();
        }

        #endregion

        #region Implementation Of IUserClaimStore<MeeTooUser>

        public async Task<IList<Claim>> GetClaimsAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(qu => qu.Id == id)
                             .SelectMany(qu => qu.Claims)
                             .AsEnumerable()
                             .Select(uc => uc.ToClaim())
                             .ToList();
        }

        public Task AddClaimsAsync(MeeTooUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task RemoveClaimsAsync(MeeTooUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<IList<MeeTooUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task ReplaceClaimAsync(MeeTooUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return null;
        }
 
        #endregion

        #region Implementation Of IUserPasswordStore<MeeTooUser>

        public Task SetPasswordHashAsync(MeeTooUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public async Task<string> GetPasswordHashAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(qu => qu.Id == id)
                             .Select(qu => qu.PasswordHash)
                             .FirstOrDefault();
        }

        public async Task<bool> HasPasswordAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(iu => iu.Id == id)
                             .Select(qu => string.IsNullOrEmpty(qu.PasswordHash))
                             .FirstOrDefault();
        }

        #endregion

        #region Implementation Of IUserEmailStore<MeeTooUser>

        public Task SetEmailAsync(MeeTooUser user, string email, CancellationToken cancellationToken)
        {
            user.Id = email;

            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(idNormalizer.Normalize(user.Id));
        }

        public async Task<bool> GetEmailConfirmedAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(qu => qu.Id == id)
                             .Select(qu => qu.EmailConfirmed)
                             .FirstOrDefault();
        }

        public Task SetEmailConfirmedAsync(MeeTooUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public async Task<MeeTooUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await FindByIdAsync(normalizedEmail, cancellationToken).ConfigureAwait(false);
        }

        public Task<string> GetNormalizedEmailAsync(MeeTooUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task SetNormalizedEmailAsync(MeeTooUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.Id = normalizedEmail;

            return Task.CompletedTask;
        }

        #endregion

        #region Implementation Of IDisposable

        public void Dispose()
        {
            storage.Dispose();
        }

        #endregion

        #region Private Stuff

        private async Task<IStorageCollection<MeeTooUser>> CollectionAsync()
        {
            return await storage.Db(identityOptions.Value.DbName)
                                .CollectionAsync<MeeTooUser>(identityOptions.Value.CollectionName)
                                .ConfigureAwait(false);
        }

        #endregion
    }
}
