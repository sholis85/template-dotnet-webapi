using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Auth.CognitoJwt;
public class CognitoJwtSettings
{
    public string? Region { get; set; }

    public string UserPoolId { get; set; }

    public string AppClientId { get; set; }

    public bool ValidateAudience { get; set; }
}