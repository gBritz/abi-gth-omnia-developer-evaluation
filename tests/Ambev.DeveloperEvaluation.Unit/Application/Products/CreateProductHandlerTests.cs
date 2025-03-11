using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Common.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the <see cref="CreateProductHandler"/> class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateProductHandler(
            _productRepository,
            new EnsureCategoryService(_categoryRepository),
            _unitOfWork,
            _mapper);
    }

    /// <summary>
    /// Tests that an invalid request when try to create product throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid request When try create product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateProductCommand();

        // When
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await method.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that an valid request when try to create a product with existing title should throws a domain exception to notifiy existing product title.
    /// </summary>
    [Fact(DisplayName = "Given valid product command When try to create and return with same title Then should throws domain exception")]
    public async Task Handle_FoundProductWithSameTitle_ThrowsDomainException()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        _productRepository.GetByTitleAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(new Product()));

        // When
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await method.Should()
            .ThrowAsync<DomainException>()
            .WithMessage($"Product with title {command.Title} already exists");
    }

    /*
    /// <summary>
    /// Tests that an valid request when delete product should return true to indicates a success operation.
    /// </summary>
    [Fact(DisplayName = "Given valid product identifier When delete product Then should return true to indicates a success operation")]
    public async Task Handle_FoundProduct_Should_Removed_It()
    {
        // Given
        var productId = Guid.NewGuid();
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        _unitOfWork.ApplyChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));
        _mapper.Map<Product>(Arg.Any<CreateProductCommand>())
            .Returns(CreateProductHandlerTestData.GenerateValidProductByCommand(command));

        // TODO cenário que não tem produto, cria lendo de GetByNameAsync.

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }
    */
}
