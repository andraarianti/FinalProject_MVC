using System.Text.Json;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using BO;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

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
				//Check Session
				if (HttpContext.Session.GetString("Staff") == null)
				{
					return RedirectToAction("Login", "Users");
				}

				//Get data staff from session
				var staffDTO = JsonSerializer.Deserialize<StaffDTO>(HttpContext.Session.GetString("Staff"));

				ViewBag.Staff = staffDTO;

				var approvals = _tripBLL.GetAllWithoutDrafted();
				return View(approvals);
			}
			catch (Exception ex)
			{
				// Tampilkan pesan kesalahan atau log pengecualian
				Console.WriteLine(ex.Message);
				return View("Error"); // Gantilah "Error" dengan nama tampilan kesalahan yang sesuai
			}
		}

		public IActionResult Detail(int TripID)
		{
			try
			{
				TempData["TripID"] = TripID;
				ExpenseByTripViewModels expenseByTripViewModels = new ExpenseByTripViewModels();
				expenseByTripViewModels.Trip = _tripBLL.GetByIdTrip(TripID);
				expenseByTripViewModels.Expenses = _tripBLL.GetTripWithExpenseByTripId(TripID);
				return View(expenseByTripViewModels);
			}
			catch (Exception ex)
			{
				// Tampilkan pesan kesalahan atau log pengecualian
				Console.WriteLine(ex.Message);
				return View("Error"); // Gantilah "Error" dengan nama tampilan kesalahan yang sesuai
			}
		}
		public IActionResult Approved(int ExpenseID)
		{
			try
			{
				int TripID = (int)TempData["TripID"]; // Pastikan TripID disimpan dengan benar sebelumnya
				var staffDTO = JsonSerializer.Deserialize<StaffDTO>(HttpContext.Session.GetString("Staff"));
				int staffId = staffDTO.StaffID;

				ApprovalDTO approval = new ApprovalDTO();
				approval.ExpenseID = ExpenseID;
				approval.ApproverID = staffId;

				_approvalBLL.SetApproval(approval);

				return RedirectToAction("Detail", "Approval", new { TripID = TripID });
			}
			catch (Exception ex)
			{
				// Tangani eksepsi jika terjadi kesalahan
				Console.WriteLine(ex.Message);
				return View("Error");
			}
		}

		public IActionResult Rejected(int ExpenseID)
		{
			try
			{
				int TripID = (int)TempData["TripID"]; // Pastikan TripID disimpan dengan benar sebelumnya
				var staffDTO = JsonSerializer.Deserialize<StaffDTO>(HttpContext.Session.GetString("Staff"));
				int staffId = staffDTO.StaffID;

				ApprovalDTO approval = new ApprovalDTO();
				approval.ExpenseID = ExpenseID;
				approval.ApproverID = staffId;

				_approvalBLL.SetRejection(approval);

				return RedirectToAction("Detail", "Approval", new { TripID = TripID });
			}
			catch (Exception ex)
			{
				// Tangani eksepsi jika terjadi kesalahan
				Console.WriteLine(ex.Message);
				return View("Error");
			}
		}

		public IActionResult SubmitReport(int TripID)
		{
			try
			{
				//int TripID = (int)TempData["TripID"];
				
					_tripBLL.SubmitApproval(TripID, 3);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// Tangani eksepsi jika terjadi kesalahan
				Console.WriteLine(ex.Message);
				return View("Error");
			}
		}
	}
}