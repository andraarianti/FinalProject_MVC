using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public class History
    {
        public int HistoryID { get; set; }
        public int StaffID { get; set; }
        public int ApprovalID { get; set; }
        public DateTime Date { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
