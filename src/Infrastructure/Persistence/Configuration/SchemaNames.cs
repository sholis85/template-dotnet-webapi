using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Persistence.Configuration;

/// <summary>
/// Isolate different catalogs for responsability seggregation, Catalog for entities, Auditing for audit...
/// </summary>
internal static class SchemaNames
{
    // TODO: figure out how to capitalize these only for Oracle
    public static string Catalog = "Catalog"; // "CATALOG";

}