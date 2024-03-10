using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public class Expense
    {
        public int ExpenseID { get; set; }
        public int TripID { get; set; }
        public string ExpenseType { get; set; }
        public decimal ItemCost { get; set; }
        public string Description { get; set; }
        public string ReceiptImage { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsApproved { get; set; }

        public Trip Trip { get; set; }
    }

}
