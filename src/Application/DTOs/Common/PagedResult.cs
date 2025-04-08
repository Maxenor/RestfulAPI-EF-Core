using System;
using System.Collections.Generic;
using System.Linq;

namespace EventManagement.Application.DTOs.Common
{
    /// <summary>
    /// Represents a paginated list of items.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    public class PagedResult<T>
    {
        public List<T> Items { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Private constructor to enforce creation via the Create static method.
        /// </summary>
        private PagedResult(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        /// <summary>
        /// Creates a new instance of PagedResult.
        /// </summary>
        /// <param name="source">The source IQueryable to paginate.</param>
        /// <param name="pageNumber">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A PagedResult containing the items for the specified page and pagination metadata.</returns>
        public static PagedResult<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(1, pageNumber);
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            // This count is only for the current page items, not the total count.
            // Use the constructor directly or the new overload if total count is known.
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Creates a new instance of PagedResult when the total count is already known.
        /// </summary>
        /// <param name="itemsForCurrentPage">The list of items for the current page.</param>
        /// <param name="totalCount">The total number of items across all pages.</param>
        /// <param name="pageNumber">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A PagedResult containing the items for the specified page and pagination metadata.</returns>
        public static PagedResult<T> CreateWithKnownTotalCount(List<T> itemsForCurrentPage, int totalCount, int pageNumber, int pageSize)
        {
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(1, pageNumber);
            return new PagedResult<T>(itemsForCurrentPage, totalCount, pageNumber, pageSize);
        }

         /// <summary>
        /// Creates a new instance of PagedResult asynchronously.
        /// Useful when the source is an IQueryable and CountAsync/ToListAsync can be used.
        /// </summary>
        /// <param name="source">The source IQueryable to paginate.</param>
        /// <param name="pageNumber">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A Task representing the asynchronous operation, containing the PagedResult.</returns>
        // Example async version (requires Microsoft.EntityFrameworkCore namespace if using CountAsync/ToListAsync)
        /*
        public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            pageNumber = Math.Max(1, pageNumber);
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }
        */
    }
}