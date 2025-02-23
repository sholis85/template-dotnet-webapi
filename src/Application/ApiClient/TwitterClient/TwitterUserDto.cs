using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Application.ApiClient.TwitterClient;
public class TwitterUserDto : IDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string UserName { get; set; } = default!;
}
