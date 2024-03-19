using System.Text.Json;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace MVC.Controllers
{
	public class UsersController : Controller
	{
		private readonly IStaffBLL _staffBLL;
		private readonly IPositionBLL _positionBLL;

		// Konstruktor yang menerima argumen yang diperlukan
		public UsersController(IStaffBLL staffBLL, IPositionBLL positionBLL)
		{
			_staffBLL = staffBLL;
			_positionBLL = positionBLL;
		}
		public IActionResult Index()
		{
			try
			{
				//check if user is logged in
				if (HttpContext.Session.GetString("Staff") == null)
				{
					return RedirectToAction("Login");
				}
				var staff = _staffBLL.GetAll();
				var position = _positionBLL.GetAll();
				var positionList = new SelectList(position, "PositionID", "PositionName");
				ViewBag.Positions = positionList;
				return View(staff);
			}
			catch (Exception ex)
			{
				// Tampilkan pesan kesalahan atau log pengecualian
				Console.WriteLine(ex.Message);
				return View("Error"); // Gantilah "Error" dengan nama tampilan kesalahan yang sesuai
			}
		}

		public IActionResult Create(StaffDTO staffDTO)
		{
			try
			{
				//checked selected items positions in views
				var selectedPosition = staffDTO.PositionID;

				_staffBLL.Insert(staffDTO);
				TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Staff Success !</div>";
			}
			catch (Exception ex)
			{
				TempData["message"] = $"<div class='alert alert-danger'><strong>Error Create : </strong>{ex.Message}</div>";
			}
			return RedirectToAction("Index");
		}

		public IActionResult Login()
		{
			if (TempData["Message"] != null)
			{
				ViewBag.Message = TempData["Message"];
			}

			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginDTO loginDTO)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			try
			{
				var username = loginDTO.Username;
				var password = loginDTO.Password;
				var staffDTO = _staffBLL.LoginMVC(loginDTO);
				//save user to session
				var staffDTOSerializer = JsonSerializer.Serialize(staffDTO);
				HttpContext.Session.SetString("Staff", staffDTOSerializer);

				TempData["Message"] = "Welcome " + staffDTO.Username;
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				ViewBag.Message = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
				return View();
			}
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Remove("Staff");
			return RedirectToAction("Login");
		}

		public IActionResult GetStaffData(int id)
		{
			return Json(_staffBLL.GetById(id)); // Mengembalikan data staff dalam format JSON
		}

		public IActionResult Edit(StaffDTO staffDTO)
		{
			try
			{
				_staffBLL.Update(staffDTO);
				TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Update Staff Success !</div>";
			}
			catch (Exception ex)
			{
				TempData["message"] = $"<div class='alert alert-danger'><strong>Error Update : </strong>{ex.Message}</div>";
			}
			return RedirectToAction("Index");
		}
		public IActionResult Delete(int id)
		{
			_staffBLL.Delete(id);
			return RedirectToAction("Index");
		}

		public IActionResult Profile()
		{
			var staff = JsonSerializer.Deserialize<StaffDTO>(HttpContext.Session.GetString("Staff"));
			return View(staff);
		}
	}
}
