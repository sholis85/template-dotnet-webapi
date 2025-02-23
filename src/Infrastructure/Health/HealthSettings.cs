using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Health;
public class HealthSettings
{
    public Cognito Cognito { get; set; }
    public Mysql Mysql { get; set; }
    public Oracle Oracle { get; set; }
    public Redis Redis { get; set; }
    public Uris Uris { get; set; }
    public Tcps Tcps { get; set; }
}

public class Cognito
{
    public bool Enabled { get; set; }
}

public class Mysql
{
    public bool Enabled { get; set; }
}

public class Oracle
{
    public bool Enabled { get; set; }
}

public class Redis
{
    public bool Enabled { get; set; }
}

public class Uris
{
    public bool Enabled { get; set; }
    public List<HealthUrl> Urls { get; set; }

}

public class Tcps
{
    public bool Enabled { get; set; }
    public List<TcpUrl> Urls { get; set; }
}

public class HealthUrl
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class TcpUrl
{
    public string Name { get; set; }
    public string Url { get; set; }
    public int Port { get; set; }
}