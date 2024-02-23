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
        public IViewCase _Case;

        public HomeController(ILogger<HomeController> logger,IDashboardAdmin db,IViewCase _Case)
        {
            _logger = logger;
            _db = db;
            this._Case = _Case; 
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
            List<DashboardAdminData>? list = _db.GetDataByStatus(new List<int> { 3,7,8});
            TempData["status"] = "Unpaid";
            return PartialView(list);
        }

        public IActionResult ToCloseStatus()
        {
            List<DashboardAdminData>? list = _db.GetDataByStatus(new List<int> {9});
            return PartialView(list);
        }

        public IActionResult viewCase(int requestId)
        {
            ViewCaseData viewCaseData = _Case.getViewCaseData(requestId);
            return View(viewCaseData);
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