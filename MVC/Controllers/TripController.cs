using BLL;
using BLL.DTO;
using BLL.Interfaces;
using BO;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModels;

namespace MVC.Controllers
{
	public class TripController : Controller
	{
		private readonly ITripBLL _tripBLL;
		private readonly IStaffBLL _staffBLL;
		// Konstruktor yang digunakan untuk dependensi injection
		public TripController(ITripBLL tripBLL, IStaffBLL staffBLL)
		{
			_tripBLL = tripBLL;
			_staffBLL = staffBLL;
		}

		public IActionResult Index()
		{
			try
			{
				var trip = _tripBLL.GetAllWithStatus();
				return View(trip);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return View("Error");
			}
		}

		public IActionResult Detail(int TripID)
		{
			try
			{
				ExpenseByTripViewModels expenseByTripViewModels = new ExpenseByTripViewModels();
				expenseByTripViewModels.Trip = _tripBLL.GetByIdTrip(TripID);
				expenseByTripViewModels.Expenses = _tripBLL.GetTripWithExpenseByTripId(TripID);
				return View(expenseByTripViewModels);
			}
			catch (Exception ex)
			{
				// Tampilkan pesan kesalahan atau log pengecualian
				Console.WriteLine(ex.Message);
				var error = ex.Message;
				return View($"Error in Controller Detail : {error}"); // Gantilah "Error" dengan nama tampilan kesalahan yang sesuai
			}
		}

		public IActionResult Create()
		{
			try
			{
				var staff = _staffBLL.GetAll();
				var listStaff = new SelectList(staff, "StaffID", "Name");
				ViewBag.Staff = listStaff;
				return View();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return View("Error");
			}
		}

		[HttpPost]
		public IActionResult Create(CreateTripReportDTO tripDTO, List<ExpenseItemsDTO> expenseItemsDTO, string submitButton, IFormFile ReceiptImage)
		{
			if (submitButton == "reimbursement")
			{
				if (ReceiptImage != null)
				{
					if (BLL.Helper.IsImageFile(ReceiptImage.FileName))
					{
						//random file name based on GUID
						var fileName = $"{Guid.NewGuid()}_{ReceiptImage.FileName}";
						var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReceiptImages", fileName);
						foreach (var expenseItems in expenseItemsDTO)
						{
							expenseItems.ReceiptImage = fileName;
						}
						//Insialisasi Status pada TripDTO
						tripDTO.StatusID = 2; // 1 = Draft
						_tripBLL.CreateTrip(tripDTO, expenseItemsDTO);

						//copy file ke folder
						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							ReceiptImage.CopyTo(fileStream);
						}
						TempData["Message"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>TripReport has been submitted successfully !</div>";
						return RedirectToAction("Index");
					}
					else
					{
						TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
						return RedirectToAction("Create");
					}
					
				}

				
			}
			else if (submitButton == "draft")
			{
				if (ReceiptImage != null)
				{
					if (BLL.Helper.IsImageFile(ReceiptImage.FileName))
					{
						//random file name based on GUID
						var fileName = $"{Guid.NewGuid()}_{ReceiptImage.FileName}";
						var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReceiptImages", fileName);
						foreach (var expenseItems in expenseItemsDTO)
						{
							expenseItems.ReceiptImage = fileName;
						}
						//Insialisasi Status pada TripDTO
						tripDTO.StatusID = 1; // 1 = Draft
						_tripBLL.CreateTrip(tripDTO, expenseItemsDTO);

						//copy file ke folder
						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							ReceiptImage.CopyTo(fileStream);
						}
						TempData["Message"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>TripReport has been added successfully !</div>";
						return RedirectToAction("Index");
					}
					else
					{
						TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
						return RedirectToAction("Create");
					}
				}
				return RedirectToAction("Index");
			}

			// Jika submitButton tidak sesuai dengan kondisi di atas, maka kembalikan view default
			TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>Submit Button is not valid !</div>";
			return View("Create");
		}

		public IActionResult Delete(int id)
		{
			_tripBLL.DeleteTrip(id);
			return RedirectToAction("Index");
		}

		public IActionResult CreateExpense(ExpenseItemsDTO expense, IFormFile ReceiptImage)
		{
			if (ReceiptImage != null)
			{
				if (BLL.Helper.IsImageFile(ReceiptImage.FileName))
				{
					//random file name based on GUID
					var fileName = $"{Guid.NewGuid()}_{ReceiptImage.FileName}";
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReceiptImages", fileName);
					expense.ReceiptImage = fileName;

					_tripBLL.CreateExpense(expense);

					//copy file ke folder
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						ReceiptImage.CopyTo(fileStream);
					}
					TempData["Message"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>TripReport has been added successfully !</div>";
					return RedirectToAction("Detail", new { TripID = expense.TripID });

				}
				else
				{
					TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
					return RedirectToAction("Detail", new { TripID = expense.TripID });

				}
			}
			TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
			return RedirectToAction("Detail", new { TripID = expense.TripID });

		}
	}

}
