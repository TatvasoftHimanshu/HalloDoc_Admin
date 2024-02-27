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
            notesData=_action.getViewNotesData(requestId);
            return View(notesData);
        }

        public IActionResult updateNotes(string Notes,string noteType,int id) 
        {
            _action.updateNote(Notes,noteType,id);
            return RedirectToAction("viewNotes", new {requestId=id});
        }

    }
}
