using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Text;
namespace TestWebAPI.Helpers
{
    public static class CodeGenerator
    {
        public static string GenerateCode(string value)
        {
            var output = new StringBuilder();

            value = value.Normalize(NormalizationForm.FormD)
                         .Replace("\u0300", "")
                         .Replace("\u0301", "")
                         .Replace("\u0303", "")
                         .Replace("\u0309", "")
                         .Replace("\u0323", "")
                         .Replace("\u0306", "")
                         .Replace("\u031B", "")
                         .Replace(" ", "");

            string merge = value + "jashdjkashd0qwlkajd092";
            int length = merge.Length;

            for (int i = 0; i < 5; i++)
            {
                int index = i == 4 ? (merge.Length / 2 + length / 2) : (length / 2);
                output.Append(merge[index]);
                length = index;
            }

            string result = value[2].ToString() + output.ToString();
            return result.ToUpper();
        }
    }
}