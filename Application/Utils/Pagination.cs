using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Application.Utils
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response,
            int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var pagination = new PaginationHeader(currentPage,
                                                  itemsPerPage,
                                                  totalItems,
                                                  totalPages);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static PageList<U> Map<T, U>(PageList<T> input, PageList<U> output) 
        {
            output.CurrentPage = input.CurrentPage;
            output.TotalPages = input.TotalPages;
            output.PageSize = input.PageSize;
            output.TotalCount = input.TotalCount;

            return output;
        }
    }

    public class PaginationHeader
    {
        public PaginationHeader(int currentPage,
                                int itemsPerPage,
                                int totalItems,
                                int totalPages)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
