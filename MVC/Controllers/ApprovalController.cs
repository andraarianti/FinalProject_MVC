using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
	public class ApprovalController : Controller
	{
		private readonly IApprovalBLL _approvalBLL;
		private readonly ITripBLL _tripBLL;
		public ApprovalController(IApprovalBLL approvalBLL, ITripBLL tripBLL)
		{
			_approvalBLL = approvalBLL;
			_tripBLL = tripBLL;
		}

		public IActionResult Index()
		{
			try
			{
				var approvals = _tripBLL.GetAllWithStatus();
				return View(approvals);
			}
			catch (Exception ex)
			{
				// Tampilkan pesan kesalahan atau log pengecualian
				Console.WriteLine(ex.Message);
				return View("Error"); // Gantilah "Error" dengan nama tampilan kesalahan yang sesuai
			}
		}
	}
}