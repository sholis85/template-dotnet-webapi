using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Domain.Common.Contracts;
public interface ISoftDelete
{
    DateTime? DeletedOn { get; set; }
}