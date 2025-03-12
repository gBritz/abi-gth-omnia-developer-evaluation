namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Security;

public class UserClaims
{
    public required string Email { get; set; }

    public required string Matricula { get; set; }

    public required string NomeCompleto { get; set; }

    public required string NomePreferido { get; set; }
}