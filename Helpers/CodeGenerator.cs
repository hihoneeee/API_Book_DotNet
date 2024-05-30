using System.Text;
namespace TestWebAPI.Helpers
{
    public static class CodeGenerator
    {
        public static string GenerateCode(string value)
        {
            // Normalize input value
            value = value.Normalize(NormalizationForm.FormD)
                         .Replace("\u0300", "")
                         .Replace("\u0301", "")
                         .Replace("\u0303", "")
                         .Replace("\u0309", "")
                         .Replace("\u0323", "")
                         .Replace("\u0306", "")
                         .Replace("\u031B", "")
                         .Replace(" ", "");

            // Add timestamp to ensure uniqueness
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            // Combine value and timestamp
            string merge = value + timestamp;

            // Take characters from the merged string to generate the code
            var output = new StringBuilder();
            int length = merge.Length;

            for (int i = 0; i < 5; i++)
            {
                int index = i == 4 ? (merge.Length / 2 + length / 2) : (length / 2);
                output.Append(merge[index]);
                length = index;
            }

            // Combine with the second character of the original value to form the final code
            string result = value[1].ToString() + output.ToString();
            return result.ToUpper();
        }

    }
}