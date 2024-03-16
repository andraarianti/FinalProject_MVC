using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace BLL.DTO
{
    public class StaffDTO
    {
        public int StaffID { get; set; }
        public string Name { get; set; }
        public int PositionID { get; set; }
        public string Role { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IEnumerable<PositionDTO> Positions { get; set; }

    }
}
