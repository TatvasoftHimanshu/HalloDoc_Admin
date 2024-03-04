using HalloDoc_Admin_.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Entities.ViewModel
{
    public class DocumentsData
    { 

        public string PatientName { get; set; }
        public string confirmation_number { get; set; }
        public int RequestId { get; set; }
         public string email { get; set; }
         public List<RequestWiseFile> list { get; set; }=new List<RequestWiseFile>();

    }
}
