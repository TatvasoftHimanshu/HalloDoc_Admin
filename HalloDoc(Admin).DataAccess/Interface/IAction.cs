using HalloDoc_Admin_.Entities.ViewModel;
using HalloDoc_Admin_.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Repositories.Interface
{
    public interface IAction
    {
        ViewCaseData getViewCaseData(int id);
        void updateCase(ViewCaseData viewCaseData);
        ViewNotesData getViewNotesData(int id);
        void updateNote( string Note, string noteType,int id);
        void cancelCase(string reason,string notes,int id);
        List<Region> getRegion();
        List<Physician> GetPhysicianList(int id);
        void assignCase(int regionid,int physicianId,string description,int id);
        void blockCase(int id, string reason);

       DocumentsData getUploadsList(int id);
    }
}
