using HalloDoc_Admin_.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Entities.ViewModel
{
    public class DashboardAdminData
    {
        public DashboardAdminData() { }
        public int Id { get; set; }
        public int Requesttype { get; set; }
        public ERequestType userType { get; set; }
        public int RequestStatus { get; set; }
        public string PatientName { get; set; } = null!;
        public string RequestorName { get; set; } = null!;
        public string PhysicianName { get; set; } = null!;
        public string DOB { get; set; } = null!;
        public DateTime RequestDate { get; set; }

        public DateTime DataofService { get; set; }
        public string Email { get; set; } = null!;
        public string PatientPhone { get; set; } = null!;
        public string RequestorPhone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Notes { get; set;}
        public int PhysicianId { get; set; }
        public string Region { get; set; } = null!;
    }
}
