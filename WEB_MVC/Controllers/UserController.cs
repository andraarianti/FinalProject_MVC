using System.Text.Json;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WEB_MVC.Controllers
{
	public class UserController : Controller
	{
		private readonly IStaffBLL _staffBLL;
		public UserController(IStaffBLL staffBLL)
		{
            _staffBLL = staffBLL;
        }
		public IActionResult Index()
		{
			try
			{
				var staff = _staffBLL.GetAll();
				return View(staff);
			}
			catch (Exception ex)
			{
				// Tampilkan pesan kesalahan atau log pengecualian
				Console.WriteLine(ex.Message);
				return View("Error"); // Gantilah "Error" dengan nama tampilan kesalahan yang sesuai
			}
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
			if(!ModelState.IsValid)
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
	}
}
