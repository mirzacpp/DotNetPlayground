﻿using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cleaners.Data
{
    /// <summary>
    /// Used to seed database with predefined data
    /// </summary>
    public class DatabaseInitializr
    {
        private readonly CorvoDbContext _context;
        private readonly UserManager<User> _userManager;

        public DatabaseInitializr(CorvoDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Seed()
        {
            // Make sure database is created with latest migrations
            _context.Database.Migrate();

            if (!_context.Users.Any())
            {
                _userManager.CreateAsync(new User
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    IsActive = true,
                    IsSoftDeleted = false,
                    Email = "mirza@test.ba",
                    UserName = "mirza@test.ba",
                    CreationDateUtc = DateTime.UtcNow,
                    EmailConfirmed = true,
                }, "Pass123$");
            }
        }
    }
}