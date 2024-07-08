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

        public async Task<PropertyHasDetail> CreateDetailyAsync(PropertyHasDetail propertyHasDetail)
        {
            _context.PropertyHasDetails.Add(propertyHasDetail);
            await _context.SaveChangesAsync();
            return propertyHasDetail;
        }

        public async Task<PropertyHasDetail> GetDetailByIdAsync(int id)
        {
            return _context.PropertyHasDetails.FirstOrDefault(phd=>phd.propertyId == id);
        }

        public async Task<PropertyHasDetail> UpdateDetailyAsync(PropertyHasDetail oldPropertyHasDetail, PropertyHasDetail newPropertyHasDetail)
        {
            oldPropertyHasDetail.province = newPropertyHasDetail.province;
            oldPropertyHasDetail.city = newPropertyHasDetail.city;
            oldPropertyHasDetail.images = newPropertyHasDetail.images;
            oldPropertyHasDetail.address = newPropertyHasDetail.address;
            oldPropertyHasDetail.bedroom = newPropertyHasDetail.bedroom;
            oldPropertyHasDetail.size = newPropertyHasDetail.size;
            oldPropertyHasDetail.bathroom = newPropertyHasDetail.bathroom;
            await _context.SaveChangesAsync();
            return oldPropertyHasDetail;
        }

        public async Task<object> DeleteDetailAsync(PropertyHasDetail propertyHasDetail)
        {
            _context.PropertyHasDetails!.Remove(propertyHasDetail);
            return await _context.SaveChangesAsync();
        }
    }
}
