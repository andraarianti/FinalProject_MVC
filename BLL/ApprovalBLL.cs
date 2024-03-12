using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL;
using BO;

namespace BLL
{
    public class ApprovalBLL : IApprovalBLL
    {
        public readonly IApprovaDAL _approvaDAL;

        public ApprovalBLL()
        {
            _approvaDAL = new ApprovalDAL();
        }
        public void SetApproval(ApprovalDTO approval)
        {
            try {
                var setApproval = new Approval
                {
                    ApproverID = approval.ApproverID,
                    ExpenseID = approval.ExpenseID
                };
                _approvaDAL.SetApproval(setApproval);
            }
            catch (Exception ex)
            {
                throw new Exception("Error SetApproval BLL: " + ex.Message);
            }
        }

        public void SetRejection(ApprovalDTO approval)
        {
            try
            {
                var setRejection = new Approval
                {
                    ApproverID = approval.ApproverID,
                    ExpenseID = approval.ExpenseID
                };
                _approvaDAL.SetRejection(setRejection);
            }
            catch (Exception ex)
            {
                throw new Exception("Error SetRejection BLL: " + ex.Message);
            }
        }
    }
}
