using BLL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MVC.ViewModels
{
    public class Dashboard
    {
		public decimal TotalTripExpense { get; set; }
		public int TotalEmployees { get; set; }
		public int TotalApprovalRequest { get; set; }
		public IEnumerable<ReadTripDTO> Trip { get; set; }
	}
}
