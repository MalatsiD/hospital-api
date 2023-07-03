using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace Hospital_API.Helpers
{
    public static class CustomExtensions
    {
        public static string TrimStringValue(this string value)
        {
            if(value != null)
            {

                return Regex.Replace(value.Trim(), @"\s+", " ");
            }

            return value!;
        }
    }
}
