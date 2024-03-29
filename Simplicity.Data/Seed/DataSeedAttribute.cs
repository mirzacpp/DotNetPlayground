﻿namespace Simplicity.Data.Seed
{
    /// <summary>
    /// Defines data seed attribute for more control over <see cref="IDataSeedContributor"/> objects.
    /// Contributors not marked with attribute will not be ignored and will have order of <c>0</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DataSeedAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets flag indicating if contributor should be ignored on seed.
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Gets or sets environment in which contributor should be used.
        /// This could be existing framework values as Development, Staging etc.
        /// If value is not specified, contributor will be used in all environments.
        /// </summary>
        public string? Environment { get; set; }

        /// <summary>
		/// Represents an order in which contribution should be executed.
		/// Lower values are executed first.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>012</c>.
		/// </remarks>
        public int Order { get; set; }
    }
}