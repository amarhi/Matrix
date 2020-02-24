using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TallComponents.PDF;

namespace Matrix.Models
{

    public partial class CSP_DOC
    { 
        public int Id { get; set; }
        public string CodeCSP { get; set; }
        public string CodeDoc { get; set; }
        public string C5 { get; set; }
        public string C6 { get; set; }
        public Nullable<int> C7 { get; set; }
        public virtual CSP CSP { get; set; }
        public virtual Documents Documents { get; set; }
             

    }
}
