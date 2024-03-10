using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public class Approval
    {
        public int ApprovalID { get; set; }
        public int ExpenseID { get; set; }
        public int ApproverID { get; set; }
        public int? StatusID { get; set; }
        public string Comment { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
