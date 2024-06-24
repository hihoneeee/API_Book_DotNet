using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class PropertyHasDetailRepositories : IPropertyHasDetailRepositories
    {
        private readonly ApplicationDbContext _context;

        public PropertyHasDetailRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PropertyHasDetail> CreatePropertyAsync(PropertyHasDetail propertyHasDetail)
        {
            _context.PropertyHasDetails.Add(propertyHasDetail);
            await _context.SaveChangesAsync();
            return propertyHasDetail;
        }
    }
}
