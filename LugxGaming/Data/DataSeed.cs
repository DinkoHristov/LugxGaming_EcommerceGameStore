﻿using LugxGaming.Data.Enums;
using LugxGaming.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.Data
{
    public class DataSeed
    {
        public static async Task AddAdmin(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<User>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // Adding roles to the db
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // Create admin user
            var admin = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                FirstName = "admin",
                LastName = "admin"
            };

            var foundUser = await userManager.FindByEmailAsync(admin.Email);

            if (foundUser == null)
            {
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }

        public static async Task AddGenres(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Genres.AnyAsync())
            {
                await dbContext.Genres.AddRangeAsync(new[]
                {
                    new Genre{ Name = "Action" },
                    new Genre{ Name = "Adventure" },
                    new Genre{ Name = "Strategy" },
                    new Genre{ Name = "Racing" },
                    new Genre{ Name = "Sport" }
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}