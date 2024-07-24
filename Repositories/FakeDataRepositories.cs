using Bogus;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Helpers;
using TestWebAPI.Models;

namespace TestWebAPI.Repositories
{
    public class FakeDataRepositories
    {
        private readonly ApplicationDbContext _context;

        public FakeDataRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task GenerateAndInsertData()
        {
            var roles = new List<Role>
            {
                new Role { code = "D22MD2", value = "Admin", createAt = DateTime.Now, updateAt = DateTime.Now },
                new Role { code = CodeGenerator.GenerateCode("Seller"), value = "Seller", createAt = DateTime.Now, updateAt = DateTime.Now },
                new Role { code = CodeGenerator.GenerateCode("Buyer"), value = "Buyer", createAt = DateTime.Now, updateAt = DateTime.Now },
            };

            _context.Roles.AddRange(roles);
            await _context.SaveChangesAsync();

            var permissions = new List<Permission>
            {
                new Permission { code = CodeGenerator.GenerateCode("add-role"), value = "add-role", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("get-role"), value = "get-role", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("update-role"), value = "update-role", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("delete-role"), value = "delete-role", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("assign-permission"), value = "assign-permission", createdAt = DateTime.Now, updatedAt = DateTime.Now },

                new Permission { code = CodeGenerator.GenerateCode("add-permission"), value = "add-permission", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("get-permission"), value = "get-permission", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("update-permission"), value = "update-permission", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("delete-permission"), value = "delete-permission", createdAt = DateTime.Now, updatedAt = DateTime.Now },

                new Permission { code = CodeGenerator.GenerateCode("add-category"), value = "add-category", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("get-category"), value = "get-category", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("update-category"), value = "update-category", createdAt = DateTime.Now, updatedAt = DateTime.Now },
                new Permission { code = CodeGenerator.GenerateCode("delete-category"), value = "delete-category", createdAt = DateTime.Now, updatedAt = DateTime.Now },
            };

            _context.Permissions.AddRange(permissions);
            await _context.SaveChangesAsync();

            var permission = await _context.Permissions.ToListAsync();

            var roleHasPermission = new List<Role_Permission>
            {
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "add-role").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "get-role").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "update-role").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "delete-role").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "assign-permission").code },

                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "add-permission").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "get-permission").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "update-permission").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "delete-permission").code },

                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "add-category").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "get-category").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "update-category").code },
                new Role_Permission { codeRole = "D22MD2", codePermission = permission.First(p => p.value == "delete-category").code },
            };

            _context.RolePermissions.AddRange(roleHasPermission);
            await _context.SaveChangesAsync();

            var userFaker = new Faker<User>()
                .RuleFor(u => u.first_name, f => f.Person.FirstName)
                .RuleFor(u => u.last_name, f => f.Random.String2(1, 10)) // Giới hạn chiều dài tên
                .RuleFor(u => u.phone, f => "0" + f.Random.Number(100000000, 999999999).ToString())
                .RuleFor(u => u.email, f => f.Internet.Email())
                .RuleFor(u => u.address, f => f.Address.FullAddress())
                .RuleFor(u => u.password, f => BCrypt.Net.BCrypt.HashPassword("12345678"))
                .RuleFor(u => u.avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.roleCode, "D22MD2");

            var users = userFaker.Generate(10);

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            var categories = new List<Category>
            {
                new Category { title = "Apartment", description = "Apartment is the best 2024", avatar = "https://res.cloudinary.com/da7u0cpve/image/upload/v1721703483/BDS.com/png-clipart-civil-engineering-building-apartment-business-building-building-condominium_wggff9.png", createAt = DateTime.Now, updateAt = DateTime.Now },
                new Category { title = "House", description = "House is the best 2024", avatar = "https://res.cloudinary.com/da7u0cpve/image/upload/v1721703516/BDS.com/png-transparent-kerala-house_cvpth2.png", createAt = DateTime.Now, updateAt = DateTime.Now },
                new Category { title = "Office", description = "Office spaces for business", avatar = "https://res.cloudinary.com/da7u0cpve/image/upload/v1721703962/BDS.com/png-transparent-building-branch-office-building-office-microsoft-office_wu49ja.png", createAt = DateTime.Now, updateAt = DateTime.Now },
                new Category { title = "Villa", description = "Luxurious villas for comfortable living", avatar = "https://res.cloudinary.com/da7u0cpve/image/upload/v1721704000/BDS.com/real-estate-villa-house-3d-computer-graphics-3d-modeling-renting-building-architecture-png-clipart_fiifvm.jpg", createAt = DateTime.Now, updateAt = DateTime.Now }
            };

            _context.Categories.AddRange(categories);
            await _context.SaveChangesAsync();

            var propertyFaker = new Faker<Property>()
                .RuleFor(p => p.title, f => f.Commerce.ProductName())
                .RuleFor(p => p.price, f => f.Random.Int(1000, 1000000))
                .RuleFor(p => p.avatar, f => f.Image.LoremFlickrUrl(1000, 500, "house"))
                .RuleFor(p => p.type, f => f.PickRandom<TypeEnum>())
                .RuleFor(p => p.description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.categoryId, f => f.PickRandom(categories).id)
                .RuleFor(p => p.category, f => f.PickRandom(categories))
                .RuleFor(p => p.createdAt, f => f.Date.Past())
                .RuleFor(p => p.updatedAt, (f, p) => p.createdAt.AddMonths(f.Random.Int(1, 12)));

            var properties = propertyFaker.Generate(60);

            _context.Properties.AddRange(properties);
            await _context.SaveChangesAsync();

            var propertyHasDetails = properties.Select(property =>
            {
                return new Faker<PropertyHasDetail>()
                    .RuleFor(phd => phd.province, f => f.Address.StateAbbr())
                    .RuleFor(phd => phd.city, f => f.Address.City())
                    .RuleFor(phd => phd.images, f =>
                    {
                        var imageUrls = new List<string>();
                        for (int i = 0; i < 5; i++)
                        {
                            imageUrls.Add(f.Image.LoremFlickrUrl(1000, 500, "house"));
                        }
                        return imageUrls;
                    })
                    .RuleFor(phd => phd.address, f => f.Address.StreetAddress())
                    .RuleFor(phd => phd.bedroom, f => f.Random.Int(1, 10))
                    .RuleFor(phd => phd.bathroom, f => f.Random.Int(1, 5))
                    .RuleFor(phd => phd.yearBuild, f => f.Date.Past(50).Year)
                    .RuleFor(phd => phd.size, f => f.Random.Int(10, 100))
                    .RuleFor(phd => phd.sellerId, f => f.PickRandom(users).id)
                    .RuleFor(phd => phd.seller, f => f.PickRandom(users))
                    .RuleFor(phd => phd.propertyId, property.id)
                    .RuleFor(phd => phd.property, property)
                    .Generate();
            }).ToList();

            _context.PropertyHasDetails.AddRange(propertyHasDetails);
            await _context.SaveChangesAsync();


            var userMediaFaker = new Faker<User_Media>()
                .RuleFor(um => um.provider, f => f.Company.CompanyName())
                .RuleFor(um => um.icon, f => f.Internet.Avatar())
                .RuleFor(um => um.link, f => f.Internet.Url())
                .RuleFor(um => um.userId, f => f.PickRandom(users).id)
                .RuleFor(um => um.user, f => f.PickRandom(users))
                .RuleFor(um => um.createdAt, f => f.Date.Past())
                .RuleFor(um => um.updatedAt, (f, um) => um.createdAt.AddMonths(f.Random.Int(1, 12)));

            var userMedias = userMediaFaker.Generate(60);

            _context.User_Medias.AddRange(userMedias);
            await _context.SaveChangesAsync();

        }
    }
}
