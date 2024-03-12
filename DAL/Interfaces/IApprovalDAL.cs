using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace DAL.Interfaces
{
    public interface IApprovaDAL : ICrudDAL<Approval>
    {
        void SetApproval(Approval approval);
        void SetRejection(Approval approval);
        //Get Approval by Employee ID
        IEnumerable<Approval> GetByTripId(int id);
    }

}
