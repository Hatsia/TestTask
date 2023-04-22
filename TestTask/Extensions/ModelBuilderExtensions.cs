using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TestTask.Enums;
using TestTask.Models.Entities;

namespace TestTask.Extensions
{
    public static class ModelBuilderExtensions
    {
        private static readonly string _password = "12345678";

        private static string[] _roleIds;
        private static string[] _userIds;

        public static ModelBuilder SeedData(this ModelBuilder builder)
        {
            builder.FillRoles()
                   .FillUsers()
                   .FillUserRoles()
                   .FillTeams();

            return builder;
        }

        public static ModelBuilder FillRoles(this ModelBuilder builder)
        {
            var roles = new IdentityRole[]
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleTypes.Admin.ToString(),
                    NormalizedName =RoleTypes.Admin.ToString().ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleTypes.Basic.ToString(),
                    NormalizedName =RoleTypes.Basic.ToString().ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            };

            _roleIds = new string[roles.Length];

            for (int i = 0; i < roles.Length; i++)
            {
                _roleIds[i] = roles[i].Id;
            }

            builder.Entity<IdentityRole>().HasData(roles);

            return builder;
        }

        public static ModelBuilder FillUsers(this ModelBuilder builder)
        {
            var firstName1 = "Peter";
            var lastName1 = "Petrov";
            var email1 = "admin@gmail.com";
            var firstName2 = "Boris";
            var lastName2 = "Britva";
            var email2 = "bory@gmail.com";

            var passwordHasher = new PasswordHasher<User>();

            var users = new User[]
            {
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = firstName1,
                    LastName = lastName1,
                    NormalizedUserName = (firstName1 + " " + lastName1).ToUpper(),
                    Email = email1,
                    NormalizedEmail = email1.ToUpper(),
                    EmailConfirmed = false,
                    PasswordHash = passwordHasher.HashPassword(null, _password),
                    SecurityStamp = "E5BBMDK3I3PX6MZCUDSP2TGQMJNHIOU7",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    LockoutEnd = null,
                    AccessFailedCount = 0
                },
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = firstName2,
                    LastName = lastName2,
                    NormalizedUserName = (firstName2 + " " + lastName2).ToUpper(),
                    Email = email2,
                    NormalizedEmail = email2.ToUpper(),
                    EmailConfirmed = false,
                    PasswordHash = passwordHasher.HashPassword(null, _password),
                    SecurityStamp = "M3ZDA3WQP6J2ZVGKBIZHOE7GKC4BR2ZF",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    LockoutEnd = null,
                    AccessFailedCount = 0
                }
            };

            _userIds = new string[users.Length];

            for (int i = 0; i < users.Length; i++)
            {
                _userIds[i] = users[i].Id;
            }

            builder.Entity<User>().HasData(users);

            return builder;
        }

        public static ModelBuilder FillUserRoles(this ModelBuilder builder)
        {
            var userRoles = new List<IdentityUserRole<string>>();

            for (int i = 0; i < _roleIds.Length; i++)
            {
                userRoles.Add(new IdentityUserRole<string>
                {
                    UserId = _userIds[i],
                    RoleId = _roleIds[i]
                });
            }

            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);

            return builder;
        }

        public static ModelBuilder FillTeams(this ModelBuilder builder)
        {
            var teams = new Team[]
            {
                new Team
                {
                    Id = 1,
                    Name = "Team1"
                },
                new Team
                {
                    Id = 2,
                    Name = "Team2"
                }
            };

            builder.Entity<Team>().HasData(teams);

            return builder;
        }
    }
}
