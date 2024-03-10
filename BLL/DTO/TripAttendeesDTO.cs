using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class TripAttendeesDTO
    {
        public int TripAttendeeID { get; set; }
        public int TripID { get; set; }
        public int StaffID { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
        public CreateTripReportDTO Trip { get; set; }
    }
}
