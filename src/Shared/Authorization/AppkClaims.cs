namespace de.WebApi.Shared.Authorization;

public static class AppkClaims
{
    public const string Fullname = "name";
    public const string UserName = "cognito:username";
    public const string Groups = "custom:groups";
    public const string EmployeeNumber = "custom:employeeNumber";
    public const string Expiration = "exp";
}