using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Auth.CognitoJwt;
public class UserGroupSettings
{
    public string digital { get; set; }

    public string callcenter { get; set; }

    public string branches { get; set; }
}