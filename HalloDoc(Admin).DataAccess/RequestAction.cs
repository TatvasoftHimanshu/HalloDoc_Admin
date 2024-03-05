using HalloDoc_Admin_.Entities.Data;
using HalloDoc_Admin_.Entities.Enum;
using HalloDoc_Admin_.Entities.Models;
using HalloDoc_Admin_.Entities.ViewModel;
using HalloDoc_Admin_.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Repositories
{
    public class RequestAction : IAction
    {
        private readonly HalloDocContext _context;
        public RequestAction(HalloDocContext context)
        {
            _context = context;
        }

        public ViewNotesData getViewNotesData(int id)
        {
            ViewNotesData noteData = new ViewNotesData();
            List<string?> transferNotes = _context.RequestStatusLogs
                                        .Where(log => log.RequestId == id && (log.TransToAdmin !=null || log.TransToPhysicianId != null))
                                        .Select(log=>log.Notes).ToList();
            noteData = _context.RequestNotes
                        .Where(request => request.RequestId == id)
                        .Select(item => new ViewNotesData
                        {
                            physicianNote = item.PhysicianNotes,
                            adminNote=item.AdminNotes

                        }).FirstOrDefault()??new ViewNotesData();
            List<RequestStatusLog> otherNotes = _context.RequestStatusLogs
                                    .Where(log => log.RequestId == id && new List<int>() { 3, 7 }.Contains(log.Status)).ToList();
            foreach(var logs in otherNotes)
            {
                if(logs.Status==3)
                {
                    noteData.cancelNote = logs.Notes;
                }
                else if(logs.Status==7)
                {
                    noteData.patientCancelNote = logs.Notes;
                }
            }
            if(transferNotes!=null)
                noteData.TransferNote.AddRange(transferNotes);
            noteData.requestId= id;
            

            return noteData;

        }

        void IAction.assignCase(int regionid, int physicianId, string description, int id)
        {
            RequestStatusLog requestStatusLog=new RequestStatusLog();
            requestStatusLog.RequestId= id;
            requestStatusLog.TransToPhysicianId= physicianId;
            requestStatusLog.Notes = description;
            requestStatusLog.CreatedDate = DateTime.Now;
            requestStatusLog.Status = 2;
            _context.RequestStatusLogs.Add(requestStatusLog);
            Request request = _context.Requests.FirstOrDefault(x => x.RequestId == id);
            request.Status = 2;
            _context.Requests.Update(request);
            _context.SaveChanges();
        }

        void IAction.blockCase(int id, string reason)
        {
            BlockRequest blockRequest = new BlockRequest
            {
                RequestId=id.ToString(),
                Reason=reason.Trim(),
                CreatedDate= DateTime.Now
            };
            _context.BlockRequests.Add(blockRequest);
            RequestStatusLog requestStatusLog = new RequestStatusLog();
            requestStatusLog.RequestId = id;
            requestStatusLog.CreatedDate = DateTime.Now;
            requestStatusLog.Status = 10;
            _context.RequestStatusLogs.Add(requestStatusLog);
            Request request = _context.Requests.FirstOrDefault(x => x.RequestId == id);
            request.Status = 10;
            _context.Requests.Update(request);
            _context.SaveChanges();
        }

        void IAction.cancelCase(string reason, string notes, int id)
        {
            RequestStatusLog requestStatusLog = new RequestStatusLog
            {
                RequestId= id,
                Status=3,
                CreatedDate= DateTime.Now,
                Notes= notes
            };
            _context.Add(requestStatusLog);
            Request request = _context.Requests.FirstOrDefault(x => x.RequestId == id);
            request.Status = 3;
            request.ModifiedDate= DateTime.Now;
            _context.Requests.Update(request);
            _context.SaveChanges();

        }

        int IAction.DeleteFileById(int fileId)
        {
            BitArray DeleteBit = new BitArray(1);
            DeleteBit.Set(0, true);
            RequestWiseFile requestWiseFile = _context.RequestWiseFiles.First(x => x.RequestWiseFileId == fileId);
            requestWiseFile.IsDeleted = DeleteBit;
            _context.RequestWiseFiles.Update(requestWiseFile);
            _context.SaveChanges();
            return requestWiseFile.RequestId;
        }

        List<Physician> IAction.GetPhysicianList(int id)
        {
            List<Physician> physicians=_context.Physicians.Where(x=>x.RegionId==id).ToList();
            return physicians;
        }

        List<Region> IAction.getRegion()
        {
            List<Region> regions = new List<Region>();
            regions = _context.Regions.ToList();
            return regions;
        }

        DocumentsData IAction.getUploadsList(int id)
        {
            BitArray bitArray = new BitArray(1);
            bitArray.Set(0, false);
            DocumentsData data = new DocumentsData();
            var result = _context.RequestWiseFiles.Where(x => x.RequestId == id && (x.IsDeleted == bitArray || x.IsDeleted == null));
            var request=_context.RequestClients.FirstOrDefault(x=>x.RequestId== id);            
            data.PatientName=request.FirstName+" "+request.LastName;
            data.confirmation_number = _context.Requests.FirstOrDefault(x => x.RequestId == id).ConfirmationNumber;
            data.email = request.Email;
            data.RequestId = id;
            List<RequestWiseFile> list1=(
                result.Select(item=>new RequestWiseFile
                {
                    RequestWiseFileId=item.RequestWiseFileId,
                    CreatedDate=item.CreatedDate,
                    FileName=item.FileName
                    
                })).ToList();
            data.list.AddRange(list1);
            return data;
        }


        ViewCaseData IAction.getViewCaseData(int requestId)
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
                                          requestType= requestClient.Request.RequestTypeId,
                                          Symptoms = requestClient.Notes ?? "",
                                          FirstName = requestClient.FirstName,
                                          LastName = requestClient.LastName,
                                          DOB = new DateOnly((int)requestClient.IntYear, DateTime.ParseExact(requestClient.StrMonth,
                                          "MMMM", new CultureInfo("en-US")).Month, (int)requestClient.IntDate),
                                          Email = requestClient.Email,
                                          Phone = requestClient.PhoneNumber,
                                          Region = result != null ? result.Name : "",
                                          BusinessName = result2 != null ? result2.Name : requestClient.Street + requestClient.City + requestClient.State
                                      }).SingleOrDefault(x => x.RequestId == requestId);
            viewCase.userType = (ERequestType)Enum.Parse(typeof(ERequestType), viewCase.requestType.ToString());
            return viewCase ?? new ViewCaseData();
        }

        bool IAction.MailDocuments(List<int> requestFilesId, int requestId)
        {
            string senderEmail = "practicetatvasoft@outlook.com";
            string senderPassword = "Tatvasoft@123";

            SmtpClient client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            
            string? email = _context.RequestClients.FirstOrDefault(x=>x.RequestId==requestId).Email;
            var requestWiseFile = from requestFiles in _context.RequestWiseFiles where requestFilesId.Contains(requestFiles.RequestWiseFileId)
                                              select new RequestWiseFile
                                              {
                                                 RequestWiseFileId=requestFiles.RequestWiseFileId,
                                                 FileName= requestFiles.FileName,
                                                 RequestId=requestFiles.RequestId,
                                              };
            string message = $@"<html>
                                <body>  
                                <h1>All Documents</h1>
                                </body>
                                </html>";
            if (email != null)
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = "Documents",
                    Body = message,
                    IsBodyHtml = true
                };
                foreach(var item in requestWiseFile)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/"+item.FileName);
                    Attachment attachment=new Attachment(filePath);
                    mailMessage.Attachments.Add(attachment);
                }
                mailMessage.To.Add(email);
                client.Send(mailMessage);
                foreach (var attachment in mailMessage.Attachments)
                {
                    attachment.Dispose();
                }
                    return true;
            }
            return false;
        }

        void IAction.updateCase(ViewCaseData viewCaseData)
        {
            RequestClient requestClient = new RequestClient();
            requestClient=_context.RequestClients.FirstOrDefault(x=>x.RequestId == viewCaseData.RequestId);
            if(requestClient!=null)
            {
                requestClient.FirstName= viewCaseData.FirstName;
                requestClient.LastName= viewCaseData.LastName;
                requestClient.PhoneNumber = viewCaseData.Phone;
                requestClient.Email= viewCaseData.Email;
                requestClient.IntDate = viewCaseData.DOB.Day;
                requestClient.IntYear= viewCaseData.DOB.Year;
                requestClient.StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(viewCaseData.DOB.Month);
                _context.RequestClients.Update(requestClient);
                _context.SaveChanges();
            }
        }

        void IAction.updateNote( string Note, string noteType,int id)
        {
            RequestNote requestNote = _context.RequestNotes.FirstOrDefault(x=>x.RequestId == id);
            if(requestNote != null)
            {
                if(noteType.Equals("Admin")) 
                {
                    requestNote.AdminNotes = Note.Trim();
                }
                else if(noteType.Equals("Physician"))
                {
                    requestNote.PhysicianNotes = Note.Trim();
                }
                if(Note!=string.Empty && requestNote.CreatedBy!=null)
                {
                    //requestNote.ModifiedBy =;
                    requestNote.ModifiedDate = DateTime.Now;
                }
                else if(Note!=string.Empty)
                {
                    //requestNote.CreatedBy =;
                    requestNote.CreatedDate = DateTime.Now;
                }
                _context.RequestNotes.Update(requestNote);
                _context.SaveChanges();
            }
            else
            {
                RequestNote requestnote = new RequestNote();
                requestnote.RequestId = id;
                if (noteType.Equals("Admin"))
                {
                    requestnote.AdminNotes = Note.Trim();
                }
                else if (noteType.Equals("Physician"))
                {
                    requestnote.PhysicianNotes = Note.Trim();
                }
                requestnote.CreatedBy = 31;
                requestnote.CreatedDate = DateTime.Now;
                _context.RequestNotes.Add(requestnote);
                _context.SaveChanges();
            }
        }

        int IAction.uploadDocument(IFormFile file, int requestId)
        {
            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
                var filePath = "uploads/" + file.FileName;
            RequestWiseFile requestWiseFile=new RequestWiseFile
            {
                RequestId = requestId,
                FileName = filePath,
                CreatedDate = DateTime.Now
            };
              
                _context.RequestWiseFiles.Add(requestWiseFile);
                _context.SaveChanges();
                return requestWiseFile.RequestWiseFileId;
            }
            return 0;
        }
    }
}
