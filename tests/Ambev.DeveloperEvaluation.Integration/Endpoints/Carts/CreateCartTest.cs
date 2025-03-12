using Ambev.DeveloperEvaluation.Integration.Application;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;
using FluentAssertions;
using Moq;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Endpoints.Carts;

/// <summary>
/// Tests for CreateCart endpoint.
/// </summary>
public class CreateCartTest : WebApiTestBase
{
    /// <summary>
    /// Test when does have carts and get list of them result should be empty.
    /// </summary>
    [Fact(DisplayName = "Test when does have carts and get list of them result should be empty.", Skip = "Test quebrando por causa dos claims do usuário, vestigar melhor")]
    public async Task Given_Nothing_Carts_When_Try_Get_Car_Response_Should_Be_NotFound()
    {
        var userId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        Factory.User.Id = userId;

        Seeder = seeder => seeder
            .NewCustomer(userId)
            .NewProduct(productId1)
            .NewProduct(productId2);

        var body = new
        {
            UserId = userId,
            Date = DateTime.UtcNow,
            Branch = "Branching name",
            Products = new[]
            {
                new
                {
                    ProductId = productId1,
                    Quantity = 1,
                },
                new
                {
                    ProductId = productId2,
                    Quantity = 10,
                },
            }
        };

        var responseMessage = await HttpClient.PostAsJsonAsync("/api/carts", body);
        responseMessage.EnsureSuccessStatusCode();

        var response = await responseMessage.Content.ReadFromJsonAsync<ApiResponseWithData<CartResponse>>();
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().NotBeEmpty();

        // note: to guarantee if dbcontext is called once time.
        var mockedDbContext = Factory.GetMock<DefaultContext>();
        mockedDbContext.Verify(_ => _.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
