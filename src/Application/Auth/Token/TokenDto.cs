using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Application.Auth.Token;
public class TokenDto : IDto
{
    public string Token { get; set; } = default!;
    public long ExpirationInUnixTime { get; set; } = default!;
}