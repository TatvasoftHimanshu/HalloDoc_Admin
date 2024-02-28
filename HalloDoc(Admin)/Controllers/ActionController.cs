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
        public IActionResult UpdateCase(ViewCaseData viewcaseData,int id)
        {
            viewcaseData.RequestId = id;
            _action.updateCase(viewcaseData);
            return RedirectToAction("viewCase",new {requestId=viewcaseData.RequestId});
        }
        public IActionResult ViewNotes(int requestId)
        {
            ViewNotesData notesData=new ViewNotesData();
            notesData =_action.getViewNotesData(requestId);
            return View(notesData);
        }

        public IActionResult updateNotes(string Notes,string noteType,int id) 
        {
            _action.updateNote(Notes,noteType,id);
            return RedirectToAction("viewNotes", new {requestId=id});
        }
        public IActionResult cancelCase(IFormCollection form,int requestId)
        {
            _action.cancelCase(form["reason"], form["Notes"], requestId);
            return RedirectToAction("Index","Home");
        }

        public IActionResult GetRegion()
        {
            return Json(new {regionlist=_action.getRegion()});
        }
        [HttpPost]
        public IActionResult GetPhysicianByRegion(int regionId)
        {
            return Json(new { physicianlist = _action.GetPhysicianList(regionId) });
        }
        public IActionResult assignCase(IFormCollection form,int requestId)
        {
            int.TryParse(form["Region"], out int regionId);
            int.TryParse(form["Physician"], out int physicianId);
            _action.assignCase(regionId,physicianId, form["Description"], requestId);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult blockCase(int requestId,IFormCollection form)
        {
            _action.blockCase(requestId, form["Reason"]);
            return RedirectToAction("Index", "Home");
        }
    }
}
