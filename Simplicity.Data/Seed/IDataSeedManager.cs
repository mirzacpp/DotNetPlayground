﻿namespace Simplicity.Data.Seed
{
    /// <summary>
    /// Abstractions for data seeding.
    /// </summary>
    public interface IDataSeedManager
    {
        Task SeedAsync();
    }
}