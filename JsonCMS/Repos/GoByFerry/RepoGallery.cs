using System;
using System.Collections.Generic;

namespace JsonCMS.Repos._GoByFerry
{
    public partial class RepoGallery
    {
        public string Imagefilename { get; set; }
        public string Photoid { get; set; }
        public string Title { get; set; }
        public string Ownerid { get; set; }
        public string Tags { get; set; }
        public string Location { get; set; }
        public string Licenseno { get; set; }
        public DateTime? Rowdateadded { get; set; }
        public string Farm { get; set; }
        public string Server { get; set; }
        public string Secret { get; set; }
        public string Licensetext { get; set; }
        public string Ownername { get; set; }
        public string Description { get; set; }
        public string Imageurl { get; set; }
        public string Largestsize { get; set; }
        public int? Maxht { get; set; }
        public int? Maxwid { get; set; }
        public string Largestimagefilename { get; set; }
        public int? Largestimagewidth { get; set; }
        public int? Largestimageheight { get; set; }
        public int Id { get; set; }
        public int? Views { get; set; }
        public string Urltolinkto { get; set; }
    }
}
