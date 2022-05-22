﻿namespace Studens.Data.Migration;

/// <summary>
/// Contract for data migration.
/// </summary>
public interface IDataMigrationManager
{
    Task MigrateAsync();
}