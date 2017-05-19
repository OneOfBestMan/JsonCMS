using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Repos._200Towns
{
    public class RepoGallery
    {
        public string imagefilename { get; set; }
        public string photoid { get; set; }
        public string title { get; set; }
        public string ownerid { get; set; }
        public string tags { get; set; }
        public string searchtown { get; set; }
        public string licenseno { get; set; }
        public Nullable<System.DateTime> rowdateadded { get; set; }
        public string farm { get; set; }
        public string server { get; set; }
        public string secret { get; set; }
        public string licensetext { get; set; }
        public string ownername { get; set; }
        public string description { get; set; }
        public string imageurl { get; set; }
        public string largestsize { get; set; }
        public Nullable<int> maxht { get; set; }
        public Nullable<int> maxwid { get; set; }
        public string largestimagefilename { get; set; }
        public Nullable<int> largestimagewidth { get; set; }
        public Nullable<int> largestimageheight { get; set; }
        public int id { get; set; }
        public Nullable<int> views { get; set; }
        public string urltolinkto { get; set; }
        public Nullable<System.Guid> ColGUID { get; set; }
    }
}
