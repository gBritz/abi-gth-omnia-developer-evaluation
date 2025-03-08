﻿using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Implementation of <see cref="IProductRepository"/> using Entity Framework Core.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of ProductRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public ProductRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public void Create(Product product)
        {
            _context.Products.Add(product);
        }

        /// <inheritdoc/>
        public void Delete(Guid id)
        {
            var product = new Product { Id = id };
            _context.Products.Attach(product);
            _context.Products.Remove(product);
        }

        /// <inheritdoc/>
        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<PaginationQueryResult<Product>> PaginateAsync(
            PaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .ToPaginateAsync(query, cancellationToken);
        }
    }
}
