using HalloDoc_Admin_.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Repositories.Interface
{
    public interface IViewCase
    {
        ViewCaseData getViewCaseData(int id);
    }
}
