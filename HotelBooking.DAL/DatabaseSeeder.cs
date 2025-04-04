using HotelBooking.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        try
        {
            // Список ролей
            var roleNames = new[] { "Admin", "User" };

            // Перевіряємо наявність ролей та додаємо їх, якщо їх ще немає
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new AppRole { Name = roleName };
                    var roleResult = await roleManager.CreateAsync(role);

                    if (!roleResult.Succeeded)
                    {
                        // Якщо не вдалося створити роль, вивести помилки
                        throw new Exception($"Failed to create role: {roleName}. Errors: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // Створення користувача Admin
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User"
                };

                // Створюємо користувача Admin з паролем
                var adminResult = await userManager.CreateAsync(adminUser, "AdminPassword123!");
                if (adminResult.Succeeded)
                {
                    // Призначаємо роль Admin
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!addToRoleResult.Succeeded)
                    {
                        // Якщо не вдалося додати роль, вивести помилки
                        throw new Exception($"Failed to assign Admin role. Errors: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    // Якщо створення користувача не вдалося, вивести помилки
                    throw new Exception($"Failed to create admin user. Errors: {string.Join(", ", adminResult.Errors.Select(e => e.Description))}");
                }
            }

            // Створення користувача User
            var defaultUser = await userManager.FindByEmailAsync("user@example.com");
            if (defaultUser == null)
            {
                defaultUser = new AppUser
                {
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    FirstName = "Default",
                    LastName = "User"
                };

                // Створюємо користувача User з паролем
                var userResult = await userManager.CreateAsync(defaultUser, "UserPassword123!");
                if (userResult.Succeeded)
                {
                    // Призначаємо роль User
                    var addToRoleResult = await userManager.AddToRoleAsync(defaultUser, "User");
                    if (!addToRoleResult.Succeeded)
                    {
                        // Якщо не вдалося додати роль, вивести помилки
                        throw new Exception($"Failed to assign User role. Errors: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    // Якщо створення користувача не вдалося, вивести помилки
                    throw new Exception($"Failed to create default user. Errors: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");
                }
            }
        }
        catch (DbUpdateException ex)
        {
            // Логування помилки оновлення бази даних
            Console.WriteLine($"Database update failed: {ex.Message}");
            Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw;
        }
    }
}
