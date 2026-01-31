using System;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = System.Text.Json.JsonSerializer.Deserialize<List<SeecUserDto>>(memberData);

        if (members == null)
        {
            Console.WriteLine("No user data found to seed.");
            return;
        }

        foreach (var member in members)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();

            var user = new AppUser
            {
                Id = member.Id,
                DisplayName = member.DisplayName,
                Email = member.Email.ToLower(),
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    DateOfBirth = member.DateOfBirth,
                    Description = member.Description,
                    Gender = member.Gender,
                    ImageUrl = member.ImageUrl,
                    City = member.City,
                    Country = member.Country,
                    Created = member.Created,
                    LastActive = member.LastActive,

                }
            };

            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id
            });

           context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }

}
