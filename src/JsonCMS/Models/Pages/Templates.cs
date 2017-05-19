using JsonCMS.Models.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.PageModels
{
    public class TemplateRegion : RegionBase
    {
        public string source { get; set; } // use enum Source
    }

    public class Template
    {
        public string template { get; set; }
        public List<TemplateRegion> regions { get; set; }
    }

    public class Templates
    {
        public List<Template> templates { get; set; }
    }
}
