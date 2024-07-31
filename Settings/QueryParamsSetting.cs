namespace TestWebAPI.Settings
{
    public class QueryParamsSetting
    {
        public int? limit { get; set; }
        public int? page { get; set; }
        public string? sort { get; set; }
        public string? fields { get; set; }
        public string? address { get; set; }
        public string? title { get; set; }
        public int? categoryId { get; set; }
        public List<string>? price { get; set; }
    }
}
