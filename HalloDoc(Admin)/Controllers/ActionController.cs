using HalloDoc_Admin_.Entities.Models;
using HalloDoc_Admin_.Entities.ViewModel;
using HalloDoc_Admin_.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc_Admin_.Controllers
{
    public class ActionController : Controller
    {
        private readonly IAction _action;
        public ActionController(IAction action)
        {
            _action = action;
        }
        public IActionResult viewCase(int requestId)
        {
            ViewCaseData viewCaseData = _action.getViewCaseData(requestId);
            return View(viewCaseData);
        }
        public IActionResult ViewNotes(int requestId)
        {
            return View();
        }
    }
}
