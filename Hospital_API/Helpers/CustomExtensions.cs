using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Hospital_API.Helpers.Pagination;
using Hospital_API.Middlewares;

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

        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

    }
}
