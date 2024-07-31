using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Settings;
using System.Linq.Dynamic.Core;
using TestWebAPI.Configs;

namespace TestWebAPI.Repositories
{
    public class PropertyRepositories : IPropertyRepositories
    {
        private readonly ApplicationDbContext _context;
        private readonly RedisCacheConfig _redisCacheConfig;

        public PropertyRepositories(ApplicationDbContext context, RedisCacheConfig redisCacheConfig) {
            _context = context;
            _redisCacheConfig = redisCacheConfig;
        }

        public async Task<(IQueryable<Property>, int)> GetPropertiesAsync(QueryParamsSetting queryParams)
        {
            var query = _context.Properties
                                .Include(p => p.PropertyHasDetail)
                                .AsQueryable();

            // Address filtering
            if (!string.IsNullOrEmpty(queryParams.address))
            {
                query = query.Where(p => p.PropertyHasDetail != null && p.PropertyHasDetail.address.ToLower().Contains(queryParams.address.ToLower()));
            }

            // Title filtering
            if (!string.IsNullOrEmpty(queryParams.title))
            {
                query = query.Where(p => p.title.ToLower().Contains(queryParams.title.ToLower()));
            }

            // category filtering
            if (queryParams.categoryId.HasValue)
            {
                query = query.Where(p => p.categoryId == queryParams.categoryId);
            }

            if (queryParams.price != null && queryParams.price.Any())
            {
                var priceValues = queryParams.price.Where(p => decimal.TryParse(p, out _)).Select(decimal.Parse).ToArray();
                var operators = queryParams.price.Except(priceValues.Select(p => p.ToString())).ToArray();

                if (priceValues.Length == 2 && operators.Length == 0)
                {
                    query = query.Where(p => p.price >= priceValues[0] && p.price <= priceValues[1]);
                }
                else if (priceValues.Length == 1 && operators.Length == 1)
                {
                    var number = priceValues[0];
                    var operatorStr = operators[0];

                    switch (operatorStr)
                    {
                        case ">":
                            query = query.Where(p => p.price > number);
                            break;
                        case ">=":
                            query = query.Where(p => p.price >= number);
                            break;
                        case "<":
                            query = query.Where(p => p.price < number);
                            break;
                        case "<=":
                            query = query.Where(p => p.price <= number);
                            break;
                        default:
                            break;
                    }
                }
            }

            // Sorting
            if (!string.IsNullOrEmpty(queryParams.sort))
            {
                var sortParams = queryParams.sort.Split(',');
                foreach (var param in sortParams)
                {
                    if (param.StartsWith("-"))
                    {
                        query = query.OrderByDescending(p => EF.Property<object>(p, param.Substring(1)));
                    }
                    else
                    {
                        query = query.OrderBy(p => EF.Property<object>(p, param));
                    }
                }
            }

            // Pagination
            if (queryParams.limit.HasValue)
            {
                var page = queryParams.page ?? 1;
                var limit = queryParams.limit.Value;
                var skip = (page - 1) * limit;
                query = query.Skip(skip).Take(limit);
            }

            var total = await _context.Properties
                                                .Include(p => p.PropertyHasDetail)
                                                .Where(p => string.IsNullOrEmpty(queryParams.address) || (p.PropertyHasDetail != null && p.PropertyHasDetail.address.ToLower().Contains(queryParams.address.ToLower())))
                                                .Where(p => string.IsNullOrEmpty(queryParams.title) || p.title.ToLower().Contains(queryParams.title.ToLower()))
                                                .Where(p => !queryParams.categoryId.HasValue || p.categoryId == queryParams.categoryId)
                                                .CountAsync();

            return (query, total);
        }

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties.Include(p => p.PropertyHasDetail).ThenInclude(phd=>phd.seller).FirstOrDefaultAsync(p => p.id == id);
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
            oldProperty.type = newProperty.type;
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
