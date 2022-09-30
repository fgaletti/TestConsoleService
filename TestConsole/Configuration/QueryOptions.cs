using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.Configuration
{
    public class QueryOptions
    {
        public const string SectionName = "Queries";

        public string OrderNumbersQuery { get; set; }
        public string OrderQuery { get; set; }
    }
}
