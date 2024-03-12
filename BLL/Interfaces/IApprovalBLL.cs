using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IApprovalBLL
    {
        void SetApproval(ApprovalDTO approval);
        void SetRejection(ApprovalDTO approval);
    }
}
