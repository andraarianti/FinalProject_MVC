using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace DAL.Interfaces
{
    public interface IStaffDAL : ICrudDAL<Staff>
    {
        IEnumerable<Staff> GetByName(string name);
        //Function to Login
        Staff Login(string username, string password);

        //Dashboard
        int CardTotalEmployee();
    }
}
