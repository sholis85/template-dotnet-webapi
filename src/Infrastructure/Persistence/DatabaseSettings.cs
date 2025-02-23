using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Persistence;
public class DatabaseSettings
{
    public bool? UsePersistence { get; set; }
    public string? DBProvider { get; set; }
    public string? ConnectionString { get; set; }
}