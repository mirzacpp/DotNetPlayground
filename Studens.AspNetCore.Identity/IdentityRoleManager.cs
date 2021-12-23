namespace Studens.AspNetCore.Identity
{
    /// <inheritdoc />
    /// TODO: Maybe resolve <see cref="CancellationToken"/> from external service so we do not depend on <see cref="IHttpContextAccessor"/>
    public class IdentityRoleManager<TRole> : RoleManager<TRole> where TRole : class
    {
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// The cancellation token associated with the current HttpContext.RequestAborted or CancellationToken.None if unavailable.
        /// </summary>
        protected override CancellationToken CancellationToken => _cancellationToken;

        /// <summary>
        /// Gets or sets the persistence store the manager operates over.
        /// </summary>
        /// <value>The persistence store the manager operates over.</value>
        protected IIdentityRoleStore<TRole> IdentityRoleStore { get; }

        public IdentityRoleManager(
            IIdentityRoleStore<TRole> store,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger,
            IHttpContextAccessor contextAccessor) :
            base(store, roleValidators, keyNormalizer, errors, logger)
        {
            IdentityRoleStore = store;
            _cancellationToken = contextAccessor?.HttpContext?.RequestAborted ?? CancellationToken.None;
        }

        #region Methods

        public virtual Task<IList<TRole>> GetAsync(
            int? skip = null,
            int? take = null,
            string? roleName = null,
            bool asNoTracking = false,
            string? email = null)
        {
            ThrowIfDisposed();

            return IdentityRoleStore.GetAsync(skip, take, NormalizeKey(roleName), asNoTracking, CancellationToken);
        }

        #endregion Methods
    }
}