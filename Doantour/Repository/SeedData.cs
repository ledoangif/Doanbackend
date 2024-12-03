using Doantour.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Doantour.Repository
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context =
                   new Hachutravelcontext(
                       serviceProvider.GetRequiredService<DbContextOptions<Hachutravelcontext>>()))
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                if (!await roleManager.Roles.AnyAsync())
                {
                    var roles = new List<IdentityRole>
                    {
                        new IdentityRole { Name = "Admin" },
                        new IdentityRole { Name = "User" }
                    };

                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }
                }
                if (!context.Role.Any())
                {
                    // Seed data for Role model
                    var roles = new List<Role>
                    {
                        new Role { RoleName = "Admin" },
                        new Role { RoleName = "User" }
                    };
                    await context.Role.AddRangeAsync(roles);
                    await context.SaveChangesAsync();
                }


                if (!userManager.Users.Any())
                {
                    var adminUser = new AppUser
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        FullName = "Admin",
                        PhoneNumber = "0123456789",
                        RoleId = 1
                    };
                    await userManager.CreateAsync(adminUser, "AdminPassword123!");

                    var regularUser = new AppUser
                    {
                        UserName = "user@example.com",
                        Email = "user@example.com",
                        FullName = "User",
                        PhoneNumber = "987654321",
                        RoleId = 2
                    };
                    await userManager.CreateAsync(regularUser, "UserPassword123!");
                    await context.SaveChangesAsync();
                }
                if (!context.Country.Any())
                {
                    var item = new List<Country>()
                    {   new Country  {
                        CountryName = "Việt Nam",
                        ContinentName = "Châu Á",
                        CountryImage = "https://res.cloudinary.com/dyjeuo2dv/image/upload/v1731462149/Image_638670841412616081.jpg",
                        },

                        new Country  {
                        CountryName = "Trung Quốc",
                        ContinentName = "Châu Á",
                        CountryImage = "https://res.cloudinary.com/dyjeuo2dv/image/upload/v1731462370/Image_638670843581015306.png",
                        },
                        new Country
                        {
                        CountryName = "Thái Lan ",
                        ContinentName = "Châu Á",
                        CountryImage = "https://res.cloudinary.com/dyjeuo2dv/image/upload/v1731462505/Image_638670844928426926.jpg",
                        },
                        new Country
                        {
                         CountryName = "Hàn Quốc ",
                         ContinentName = "Châu Á",
                         CountryImage = "https://res.cloudinary.com/dyjeuo2dv/image/upload/v1731462749/Image_638670847212665164.png",
                        },
                        new Country
                        {
                         CountryName = "Nhật Bản ",
                         ContinentName = "Châu Á",
                         CountryImage = "https://res.cloudinary.com/dyjeuo2dv/image/upload/v1731462884/Image_638670848668319956.jpg",
                        },
                         new Country
                        {
                         CountryName = "Nga",
                         ContinentName = "Châu Âu",
                         CountryImage = "https://res.cloudinary.com/dyjeuo2dv/image/upload/v1731463026/Image_638670850142768030.jpg",
                        },
                    };
                    await context.Country.AddRangeAsync(item);
                    await context.SaveChangesAsync();
                }
                //if (!context.Hotel.Any())
                //{
                //    var item = new List<Hotel>()
                //    {
                //        new Hotel  {
                //         HotelName= "Park Hyatt Saigon",
                //         Address = "2 Lam Son Square, Ho Chi Minh City (Saigon), Vietnam, 700000",
                //         Rate = 4 ,
                //         CEO =" Nguyễn Minh Chi  ",
                //         LinkHotel ="https://www.hyatt.com/en-US/hotel/vietnam/park-hyatt-saigon/saiph",
                //         Hotline ="02898987678",
                //        },
                //        new Hotel
                //        {
                //         HotelName= "Thái Lan ",
                //         Address = "Châu Á",
                //         Rate = 4,
                //         CEO =" Nguyễn Minh Chi  ",
                //         LinkHotel ="https://www.mandarinoriental.com/en/paris/place-vendome/?src=ps.brand.mopar.bng&content_id=cg483425322_ag1227055981997410_kwkwd-76691262035309_cr_ex{extensionid}_mandarin%20oriental%20hotels%20paris_&gclsrc=3p.ds&msclkid=d664f770bd4019e554d8912dc5056533",
                //         Hotline ="02898987678",
                //        },
                //        new Hotel
                //        {
                //         HotelName= "Hàn Quốc ",
                //          Address = "Châu Á",
                //         Rate = "https://res.cloudinary.com/dnfnp1vdp/image/upload/v1717400991/Image_638530229899793780.jpg",
                //        },
                //        new Hotel
                //        {
                //         HotelName= "Nhật Bản ",
                //          Address = "Châu Á",
                //         Rate = "https://res.cloudinary.com/dnfnp1vdp/image/upload/v1717401093/Image_638530230922899625.jpg",
                //        },
                //         new Hotel
                //        {
                //         HotelName= "Nga",
                //          Address = "Châu Âu",
                //         Star = "https://res.cloudinary.com/dnfnp1vdp/image/upload/v1717401151/Image_638530231495983574.jpg",
                //        },
                //    };
                //    await context.Country.AddRangeAsync(item);
                //    await context.SaveChangesAsync();
                //}
            }
        }
    }
}