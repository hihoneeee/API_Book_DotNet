using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Dynamic;
using System.Reflection;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Settings;

namespace TestWebAPI.Repositories
{
    public class PropertyRepositories : IPropertyRepositories
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepositories(ApplicationDbContext context) {
            _context = context;
        }
        //public async Task<List<Property>> GetPropertiesAsync(QueryParamsSetting queryParams)
        //{
        //    var query = _context.Properties.Include(p => p.PropertyHasDetails).AsQueryable();


        //    // Address filtering
        //    if (!string.IsNullOrEmpty(queryParams.address))
        //    {
        //        query = query.Where(p => p.PropertyHasDetails.Any(phd => phd.address.ToLower().Contains(queryParams.address.ToLower())));
        //    }


        //    // title filtering
        //    if (!string.IsNullOrEmpty(queryParams.title))
        //    {
        //        query = query.Where(p => p.title.ToLower().Contains(queryParams.title.ToLower()));
        //    }

        //    // Sorting
        //    if (!string.IsNullOrEmpty(queryParams.sort))
        //    {
        //        var sortParams = queryParams.sort.Split(',');
        //        foreach (var param in sortParams)
        //        {
        //            if (param.StartsWith("-"))
        //            {
        //                query = query.OrderByDescending(p => EF.Property<object>(p, param.Substring(1)));
        //            }
        //            else
        //            {
        //                query = query.OrderBy(p => EF.Property<object>(p, param));
        //            }
        //        }
        //    }

        //    // Pagination
        //    if (queryParams.limit.HasValue)
        //    {
        //        var page = queryParams.page ?? 1;
        //        var limit = queryParams.limit.Value;
        //        var skip = (page - 1) * limit;
        //        query = query.Skip(skip).Take(limit);
        //    }

        //    return await query.ToListAsync();
        //}

        public async Task<(List<ExpandoObject>, int)> GetPropertiesAsync(QueryParamsSetting queryParams)
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

            // Fetch data from the database
            var total = await query.CountAsync();
            var properties = await query.ToListAsync();

            // Fields selection and projection
            List<ExpandoObject> result = new List<ExpandoObject>();
            if (!string.IsNullOrEmpty(queryParams.fields))
            {
                var fields = queryParams.fields.Split(',').Select(f => f.Trim()).ToList();
                bool isExclusion = fields.Any(f => f.StartsWith("-"));
                var includeFields = isExclusion
                    ? typeof(Property).GetProperties().Select(p => p.Name).Except(fields.Select(f => f.TrimStart('-'))).ToList()
                    : fields;

                foreach (var property in properties)
                {
                    var expando = new ExpandoObject() as IDictionary<string, object>;
                    var propertyType = property.GetType();

                    foreach (var field in includeFields)
                    {
                        var propInfo = propertyType.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propInfo != null)
                        {
                            expando[field] = propInfo.GetValue(property);
                        }
                    }

                    result.Add((ExpandoObject)expando);
                }
            }
            else
            {
                foreach (var property in properties)
                {
                    var expando = new ExpandoObject() as IDictionary<string, object>;
                    var propertyType = property.GetType();

                    foreach (var prop in propertyType.GetProperties())
                    {
                        expando[prop.Name] = prop.GetValue(property);
                    }

                    result.Add((ExpandoObject)expando);
                }
            }

            return (result, total);
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
