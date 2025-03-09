﻿using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Spec that check SaleItems amount and SaleItems quantity individually
/// </summary>
public class SaleLimitReachedSpecification : ISpecification<Cart>
{
    private readonly int _maximumItemsPerProduct = 20; // TODO: deixar configurável

    public bool IsSatisfiedBy(Cart cart)
    {
        return cart.Items
            .Any(i => i.Quantity > _maximumItemsPerProduct);
    }
}