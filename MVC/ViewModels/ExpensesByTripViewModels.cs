using BLL.DTO;
using BO;

namespace MVC.ViewModels
{
	public class ExpenseByTripViewModels
	{
		public int TripID { get; set; }
		public ReadTripDTO Trip { get; set; }
		public Status Status { get; set; }
		public List<ExpenseItemsDTO> Expenses { get; set; }
	}
}
