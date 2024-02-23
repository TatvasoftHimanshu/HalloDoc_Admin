using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Entities.ViewModel
{
    public class ViewCaseData
    {
        public int RequestId { get; set; }
        public string ConFirmationNumber { get; set; }
        public string Symptoms { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public string BusinessName { get; set; }
        public string Room { get; set; }
    }
}
