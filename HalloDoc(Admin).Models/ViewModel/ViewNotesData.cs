using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Entities.ViewModel
{
    public class ViewNotesData
    {
        public int requestId { get; set; }
        public List<string> TransferNote { get; set; }=new List<string>();
        public string? physicianNote { get; set; }
        public string? adminNote { get; set; }
        public string? cancelNote { get; set; }
        public string? patientCancelNote { get;set; }

    }
}
