using ClosedXML.Excel;
using HalloDoc_Admin_.Entities.Models;
using HalloDoc_Admin_.Entities.ViewModel;
using HalloDoc_Admin_.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDoc_Admin_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IDashboardAdmin _db;
       

        public HomeController(ILogger<HomeController> logger,IDashboardAdmin db)
        {
            _logger = logger;
            _db = db;
            
        }

        public IActionResult Index()
        {
            List<int> list = _db.GetCountByStatus();
            ViewBag.list = list;
            return View();
        }

        public IActionResult showStatusData(int status) 
        {
            
            switch(status)
            {
                case 1:
                    return RedirectToAction("NewStatus");

                case 2:
                    return RedirectToAction("PendingStatus");

                case 3:
                    return RedirectToAction("ActiveStatus");

                case 4:
                    return RedirectToAction("ConcludeStatus");

                case 5:
                    return RedirectToAction("ToCloseStatus");

                case 6:
                    return RedirectToAction("UnpaidStatus");
                default:
                    return RedirectToAction("NewStatus");
            }
        }
        public IActionResult NewStatus()
        {
            List<DashboardAdminData>? list = _db.GetDataByStatus(new List<int> {1});
            TempData["status"] = "New";
            return PartialView(list);
        }

        public IActionResult PendingStatus()
        {
            List<DashboardAdminData>? list = _db.GetDataByStatus(new List<int> {2});
            TempData["status"] = "Pending";
            return PartialView(list);
        }

        public IActionResult ActiveStatus()
        {
            List<DashboardAdminData>? list = _db.GetDataByStatus(new List<int> { 4,5 });
            TempData["status"] = "Active";
            return PartialView(list);
        }

        public IActionResult ConcludeStatus()
        {
            List<DashboardAdminData>? list = _db.GetDataByStatus(new List<int> { 6 });
            TempData["status"] = "Conclude";
            return PartialView(list);
        }

        public IActionResult UnpaidStatus()
        {
            List<DashboardAdminData>? list = _db.GetDataByStatus(new List<int> { 9});
            TempData["status"] = "Unpaid";
            return PartialView(list);
        }

        public IActionResult ToCloseStatus()
        {
            List<DashboardAdminData>? list = _db.GetDataByStatus(new List<int> { 3, 7, 8 });
            return PartialView(list);
        }

        public IActionResult ExportAllData()
        {
            List<DashboardAdminData> allData=new List<DashboardAdminData>();
            allData=_db.GetAllData();
            try
            {
                var datasheets = new XLWorkbook();
                var datasheet = datasheets.Worksheets.Add("All Request Data");
                datasheet.Cell(1, 1).Value = "Request Id";
                datasheet.Cell(1, 2).Value = "Request Type";
                datasheet.Cell(1, 3).Value = "Status";
                datasheet.Cell(1, 4).Value = "Requestor Name";
                datasheet.Cell(1, 5).Value = "Date of Birth";
                datasheet.Cell(1, 6).Value = "Date of Service";
                datasheet.Cell(1, 7).Value = "Request Date";
                datasheet.Cell(1, 8).Value = "Patient Name";
                datasheet.Cell(1, 9).Value = "Address";
                datasheet.Cell(1, 10).Value = "Requestor PhoneNo";
                datasheet.Cell(1, 11).Value = "Physician Name";

                int row = 2;
                foreach(var item in allData)
                {
                    datasheet.Cell(row, 1).Value = item.Id;
                    datasheet.Cell(row, 2).Value = item.Requesttype;
                    datasheet.Cell(row, 3).Value = item.RequestStatus;
                    datasheet.Cell(row, 4).Value = item.RequestorName;
                    datasheet.Cell(row, 5).Value = item.DOB;
                    datasheet.Cell(row, 6).Value = item.DataofService;
                    datasheet.Cell(row, 7).Value = item.RequestDate;
                    datasheet.Cell(row, 8).Value = item.PatientName;
                    datasheet.Cell(row, 9).Value = item.Address;
                    datasheet.Cell(row, 10).Value = item.RequestorPhone;
                    datasheet.Cell(row, 11).Value = item.PhysicianName;
                    row++;
                }
                datasheet.Columns().AdjustToContents();
                var memoryStream=new MemoryStream();
                datasheets.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllData.xlsx");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                throw;
            }
        }
        public IActionResult sendLink(IFormCollection form)
        {
            _db.SendMail(form["FirstName"] + " " + form["LastName"], "For New User", form["Email"]);
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}