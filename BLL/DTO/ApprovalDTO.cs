using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class ApprovalDTO
    {
        public int ApprovalID { get; set; }
        public int ApproverID { get; set; } //EmployeeID
        public int ExpenseID { get; set; } //ExpenseID
        public bool IsApproved { get; set; } //Approved

    }
}
