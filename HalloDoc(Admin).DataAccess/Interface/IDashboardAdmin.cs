﻿using HalloDoc_Admin_.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_Admin_.Repositories.Interface
{
    public interface IDashboardAdmin
    {
        List<DashboardAdminData> GetDataByStatus(List<int> status);
        List<DashboardAdminData> GetAllData();
        List<int> GetCountByStatus();
        void Save();
        void SendMail(string Name,string subject,string mail);
    }
}
