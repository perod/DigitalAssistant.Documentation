using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBuilder
{
    public class Template
    {
        public string SourceProject { get; set; }
        public string DestinationProject { get; set; }
        public string RootNamespace { get; set; }
        public List<string> SourceProjects { get; set; }
    }
}
