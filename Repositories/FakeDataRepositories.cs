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
                .RuleFor(u => u.last_name, f => f.Person.LastName)
                .RuleFor(u => u.phone, f => "0" + f.Random.Number(100000000, 999999999).ToString())
                .RuleFor(u => u.email, f => f.Internet.Email())
                .RuleFor(u => u.address, f => f.Address.FullAddress())
                .RuleFor(u => u.password, f => BCrypt.Net.BCrypt.HashPassword("123456"))
                .RuleFor(u => u.avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.roleCode, "D22MD2");

            var users = userFaker.Generate(10);

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            var categoryFaker = new Faker<Category>()
                .RuleFor(p => p.title, f => f.Commerce.Categories(1)[0])
                .RuleFor(p => p.description, f => "Newest " + f.Commerce.Categories(1)[0] + " sale today")
                .RuleFor(p => p.avatar, f => f.Image.LoremFlickrUrl(1000, 500, "House"))
                .RuleFor(p => p.createAt, f => f.Date.Past())
                .RuleFor(p => p.updateAt, f => f.Date.Recent());

            var categories = categoryFaker.Generate(3);

            _context.Categories.AddRange(categories);
            await _context.SaveChangesAsync();

            var propertyFaker = new Faker<Property>()
                .RuleFor(p => p.title, f => f.Commerce.ProductName())
                .RuleFor(p => p.price, f => f.Random.Int(1000, 1000000)) // Ensure this is an int
                .RuleFor(p => p.avatar, f => f.Image.LoremFlickrUrl(1000, 500, "realestate"))
                .RuleFor(p => p.status, f => f.PickRandom<statusEnum>())
                .RuleFor(p => p.description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.category_id, f => f.PickRandom(categories).id)
                .RuleFor(p => p.category, f => f.PickRandom(categories))
                .RuleFor(p => p.createdAt, f => f.Date.Past())
                .RuleFor(p => p.updatedAt, (f, p) => p.createdAt.AddMonths(f.Random.Int(1, 12)));

            var properties = propertyFaker.Generate(60);

            _context.Properties.AddRange(properties);
            await _context.SaveChangesAsync();

            var propertyHasDetailFaker = new Faker<PropertyHasDetail>()
                .RuleFor(phd => phd.province, f => f.Address.State())
                .RuleFor(phd => phd.city, f => f.Address.City())
                .RuleFor(phd => phd.images, f => f.Image.LoremFlickrUrl(1000, 500, "realestate"))
                .RuleFor(phd => phd.address, f => f.Address.FullAddress())
                .RuleFor(phd => phd.bedroom, f => f.Random.Int(1, 10))
                .RuleFor(phd => phd.bathroom, f => f.Random.Int(1, 5))
                .RuleFor(phd => phd.year_build, f => f.Date.Past(50).Year)
                .RuleFor(phd => phd.size, f => f.Random.Int(500, 10000))
                .RuleFor(phd => phd.seller_id, f => f.PickRandom(users).id)
                .RuleFor(phd => phd.seller, f => f.PickRandom(users))
                .RuleFor(phd => phd.property_id, f => f.PickRandom(properties).id)
                .RuleFor(phd => phd.property, f => f.PickRandom(properties))
                .RuleFor(phd => phd.type, f => f.PickRandom<typeEnum>());

            var propertyHasDetails = propertyHasDetailFaker.Generate(60);

            _context.propertyHasDetails.AddRange(propertyHasDetails);
            await _context.SaveChangesAsync();

            var userMediaFaker = new Faker<User_Media>()
                .RuleFor(um => um.provider, f => f.Company.CompanyName())
                .RuleFor(um => um.icon, f => f.Internet.Avatar())
                .RuleFor(um => um.link, f => f.Internet.Url())
                .RuleFor(um => um.user_id, f => f.PickRandom(users).id)
                .RuleFor(um => um.user, f => f.PickRandom(users))
                .RuleFor(um => um.createdAt, f => f.Date.Past())
                .RuleFor(um => um.updatedAt, (f, um) => um.createdAt.AddMonths(f.Random.Int(1, 12)));

            var userMedias = userMediaFaker.Generate(60);

            _context.User_Medias.AddRange(userMedias);
            await _context.SaveChangesAsync();

        }
    }
}
