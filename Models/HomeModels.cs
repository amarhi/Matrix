using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using TallComponents.PDF;

namespace Matrix.Models
{
    using System;
    using System.Collections.Generic;
    public class PanelModel
    {
      
        public const float THUMBRES = 18;
        public Document Document { get; set; }
        public string DocumentGuid { get; set; }

        public string Idc { get; set; }
        public string Url { get; set; }
        public string Cs { get; set; }
        public virtual IEnumerable<CSP_DOC> cSP_DOC { get; set; }
        public virtual Documents Documents { get; set; }
        public virtual IEnumerable<CSP> CSP { get; set; }
        
    }
    
}