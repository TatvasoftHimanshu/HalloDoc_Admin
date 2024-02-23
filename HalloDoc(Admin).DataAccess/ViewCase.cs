using HalloDoc_Admin_.Entities.Data;
using HalloDoc_Admin_.Entities.Models;
using HalloDoc_Admin_.Entities.ViewModel;
using HalloDoc_Admin_.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Repositories
{
    public class ViewCase : IViewCase
    {
        private readonly HalloDocContext _context;
        public ViewCase(HalloDocContext context) 
        { 
            _context = context;
        }
        ViewCaseData IViewCase.getViewCaseData(int requestId)
        {
            ViewCaseData? viewCase = (from requestClient in _context.RequestClients
                           join region in _context.Regions on requestClient.RegionId equals region.RegionId into requestGroup
                           from result in requestGroup.DefaultIfEmpty()
                           join requestBusiness in _context.RequestBusinesses on requestClient.RequestId equals requestBusiness.RequestId into requestBusinessGroup
                           from result1 in requestBusinessGroup.DefaultIfEmpty()
                           join business in _context.Businesses on result1.BusinessId equals business.BusinessId into BusinessGroup
                           from result2 in BusinessGroup.DefaultIfEmpty()
                           select new ViewCaseData
                           {
                               RequestId = requestClient.RequestId,
                               ConFirmationNumber = _context.Requests.FirstOrDefault(x => x.RequestId == requestId).ConfirmationNumber,
                               Symptoms=requestClient.Notes??"",
                               FirstName=requestClient.FirstName, 
                               LastName=requestClient.LastName,
                               DOB=new DateOnly((int)requestClient.IntYear,DateTime.ParseExact(requestClient.StrMonth,"MMMM", new CultureInfo("en-US")).Month,(int)requestClient.IntDate),
                               Email=requestClient.Email,
                               Phone=requestClient.PhoneNumber,
                               Region=result!=null?result.Name:"",
                               BusinessName=result2!=null?result2.Name:requestClient.Street+requestClient.City+requestClient.State
                            }).SingleOrDefault(x=>x.RequestId == requestId);
            
                return viewCase??new ViewCaseData();
        }
    }
}
