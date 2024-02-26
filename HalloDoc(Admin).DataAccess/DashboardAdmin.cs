using HalloDoc_Admin_.Entities.Data;
using HalloDoc_Admin_.Repositories.Interface;
using HalloDoc_Admin_.Entities.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc_Admin_.Entities.Models;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc_Admin_.Repositories
{
    public class DashboardAdmin:IDashboardAdmin
    {
        private readonly HalloDocContext _context;
        public DashboardAdmin(HalloDocContext context) { 
            _context = context;
        }

        public List<int> GetCountByStatus()
        {
            List<int> list=new List<int>();
            list.Add(_context.Requests.Where(x => x.Status == 1).Count());
            list.Add(_context.Requests.Where(x => x.Status == 2).Count());
            list.Add(_context.Requests.Where(x => x.Status == 4 || x.Status==5).Count());
            list.Add(_context.Requests.Where(x => x.Status == 6).Count());
            list.Add(_context.Requests.Where(x => x.Status == 3 || x.Status==7 ||x.Status==8).Count());
            list.Add(_context.Requests.Where(x => x.Status == 9).Count());
            return list;

        }

        List<DashboardAdminData> IDashboardAdmin.GetAllData()
        {
            var data = (from request in _context.Requests
                        join Client in _context.RequestClients on request.RequestId equals Client.RequestId into requestGroup
                        from result in requestGroup.DefaultIfEmpty()
                        join statusLog in _context.RequestStatusLogs on request.RequestId equals statusLog.RequestId into statusLogGroup
                        from logResult in statusLogGroup.DefaultIfEmpty()
                        join physician in _context.Physicians on request.PhysicianId equals physician.PhysicianId into requestedPhysician
                        from result1 in requestedPhysician.DefaultIfEmpty()
                        select new DashboardAdminData
                        {
                            Id = request.RequestId,
                            RequestType = request.RequestTypeId,
                            RequestStatus = request.Status,
                            RequestorName = request.FirstName + " " + request.LastName,
                            DOB = result != null ? @$"{result.IntDate}/{result.StrMonth}/{result.IntYear}" : "",
                            DataofService = logResult.CreatedDate,
                            RequestDate = request.CreatedDate,
                            PatientName = result != null ? result.FirstName + " " + result.LastName : "",
                            Email = result != null && result.Email != null ? result.Email : "",
                            PatientPhone = result != null ? result.PhoneNumber : "",
                            RequestorPhone = request.PhoneNumber,
                            Address = result != null ? result.Address : "",
                            Notes = result != null ? result.Notes : "",
                            PhysicianId = result1 != null ? result1.PhysicianId : 0,
                            PhysicianName = result1.FirstName + " " + result1.LastName,

                        }).ToList();

            return data;
        }

        List<DashboardAdminData> IDashboardAdmin.GetDataByStatus(List<int> status)
        {
           var data = (from request in _context.Requests
                       join Client in _context.RequestClients on request.RequestId equals Client.RequestId into requestGroup
                       from result in requestGroup.DefaultIfEmpty()
                       join statusLog in _context.RequestStatusLogs on request.RequestId equals statusLog.RequestId into statusLogGroup
                       from logResult in statusLogGroup.DefaultIfEmpty()
                       join physician in _context.Physicians on request.PhysicianId equals physician.PhysicianId into requestedPhysician
                       from result1 in requestedPhysician.DefaultIfEmpty() where status.Contains(request.Status)
                       select new DashboardAdminData
                       {
                           Id=request.RequestId,
                           RequestType=request.RequestTypeId,
                           RequestStatus=request.Status,
                           RequestorName=request.FirstName+" "+request.LastName,
                           DOB= result!=null?@$"{result.IntDate}/{result.StrMonth}/{result.IntYear}":"",
                           DataofService=logResult.CreatedDate,
                           RequestDate=request.CreatedDate,
                           PatientName= result!=null?result.FirstName+" "+result.LastName:"",
                           Email= result!=null && result.Email!=null?result.Email:"",
                           PatientPhone=result != null ? result.PhoneNumber : "",
                           RequestorPhone=request.PhoneNumber,
                           Address=result!=null?result.Address:"",
                           Notes= result != null ? result.Notes : "",
                           PhysicianId=result1!=null?result1.PhysicianId:0,
                           PhysicianName=result1.FirstName+" "+result1.LastName,

                       }).ToList();

            return data;
        }


        void IDashboardAdmin.Save()
        {
            _context.SaveChanges();
        }
    }
}
