using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
	public class PositionController : Controller
	{
		private readonly IPositionBLL _positionBLL;
		public PositionController(IPositionBLL positionBLL)
		{
			_positionBLL = positionBLL;
		}
		public IActionResult Index()
		{
			try
			{
				var positions = _positionBLL.GetAll();
				return View(positions);
			}
			catch (Exception ex)
			{
				// Tampilkan pesan kesalahan atau log pengecualian
				Console.WriteLine(ex.Message);
				return View("Error"); // Gantilah "Error" dengan nama tampilan kesalahan yang sesuai
			}
		}

		[HttpPost]
		public IActionResult Create(PositionDTO positionDTO)
		{
			try
			{
				_positionBLL.Insert(positionDTO);
				TempData["Message"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>Position has been added</div>";
			}
			catch (Exception ex)
			{
				TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
			}

			return RedirectToAction("Index");
		}

		public IActionResult Edit(int id)
		{
			var position = _positionBLL.GetById(id);
			return View(position);
		}

		public IActionResult Delete(int id)
		{
			_positionBLL.Delete(id);
			return RedirectToAction("Index");
		}
	}
}
