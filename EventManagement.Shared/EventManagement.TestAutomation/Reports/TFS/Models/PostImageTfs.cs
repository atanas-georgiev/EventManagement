using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.TestAutomation.Reports.TFS.Models
{
    public class PostImageTfs
    {
        public string Stream { get; set; }

        public string FileName { get; set; }

        public string Comment { get; set; }

        public string AttachmentType { get; set; }
    }
}
