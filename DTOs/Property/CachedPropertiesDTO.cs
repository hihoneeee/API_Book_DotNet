namespace TestWebAPI.DTOs.Property
{
    public class CachedPropertiesDTO
    {
        public List<GetPropertyDTO> Properties { get; set; }
        public int TotalCount { get; set; }
    }
}
