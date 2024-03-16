﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class ReadTripDTO
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


        public StaffDTO Staff { get; set; }
        public StatusDTO Status { get; set; }

        //THIS FOR WEBFORM
        //public ExpenseItemsDTO ExpenseItems { get; set; }

        //THIS FOR MVC
        public IEnumerable<ExpenseItemsDTO> ExpenseItems { get; set; }
    }
}
