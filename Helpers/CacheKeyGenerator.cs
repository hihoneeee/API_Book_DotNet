using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;

namespace TestWebAPI.Helpers
{
    public class CacheKeyGenerator
    {
        public static string GenerateKey(object filter, string sort, string fields, int? page, int? limit, string address)
        {
            var filterString = JsonConvert.SerializeObject(filter)
                .Replace("[^\\w]", "")
                .ToCharArray();
            Array.Sort(filterString);

            var sortKey = sort?.Replace("[^\\w]", "") ?? "";
            var fieldsKey = fields?.Replace("[^\\w]", "") ?? "";
            var pageKey = page?.ToString().Replace("[^\\w]", "") ?? "";
            var limitKey = limit?.ToString().Replace("[^\\w]", "") ?? "";
            var addressKey = address?.Replace("[^\\w]", "") ?? "";
            var ipAddress = GetIpAddress();

            return new string(filterString) + sortKey + fieldsKey + pageKey + limitKey + ipAddress;
        }

        private static string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString().Replace("[^\\w]", "");
                }
            }
            return string.Empty;
        }
    }
}

