using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
using BO;

namespace BLL.Interfaces
{
    public interface IStaffBLL
    {
        void Delete(int id);
        IEnumerable<StaffDTO> GetAll();
        StaffDTO GetById(int id);
        IEnumerable<StaffDTO> GetByName(string name);
        void Insert(StaffDTO obj);
        void Update(StaffDTO obj);
        //Login
        StaffDTO Login(string username, string password);

    }
}
