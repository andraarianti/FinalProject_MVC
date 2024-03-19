using System.Diagnostics;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITripBLL _tripBLL;
        private readonly IStaffBLL _staffBLL;

        public HomeController(ILogger<HomeController> logger, ITripBLL tripBLL, IStaffBLL staffBLL)
        {
            _logger = logger;
            _tripBLL = tripBLL;
            _staffBLL = staffBLL;
        }

        public IActionResult Index()
        {
			//Check Session
			if (HttpContext.Session.GetString("Staff") == null)
			{
				return RedirectToAction("Login", "Users");
			}

            decimal totalExpense = _tripBLL.CardTotalExpense();
            int totalEmployee = _staffBLL.CardTotalEmployee();
            int totalApprovalRequest = _tripBLL.CardTotalApprovalRequest();
            IEnumerable<ReadTripDTO> trip = _tripBLL.GetAllOnlyInProgress();

            Dashboard dashboard = new Dashboard
            { 
                TotalTripExpense = totalExpense,
                TotalEmployees = totalEmployee,
                TotalApprovalRequest = totalApprovalRequest,
                Trip = trip
			};
			return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
