using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class PropertyRepositories : IPropertyRepositories
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepositories(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<List<Property>> GetAllPropertyAsync()
        {
            return await _context.Properties.ToListAsync();
        }
        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return _context.Properties.FirstOrDefault(p=>p.id == id);
        }
        public async Task<Property> CreatePropertyAsync(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task<Property> UpdatePropertyAsync(Property oldProperty, Property newProperty)
        {
            oldProperty.title = newProperty.title;
            oldProperty.description = newProperty.description;
            oldProperty.price = newProperty.price;
            oldProperty.avatar = newProperty.avatar;
            oldProperty.status = newProperty.status;
            oldProperty.categoryId = newProperty.categoryId;
            oldProperty.updatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return oldProperty;
        }
        public async Task<object> DeletePropertyAsync(Property property)
        {
            _context.Properties!.Remove(property);
            return await _context.SaveChangesAsync();
        }
    }
}
