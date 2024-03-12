using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public class Trip
    {
        public int TripID { get; set; }
        public int SubmittedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public int? StatusID { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }

        public Status Status { get; set; }
        public Staff Staff { get; set; }
        public Expense ExpenseItems { get; set; }

    }

}
