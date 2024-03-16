using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WEB_MVC.Controllers
{
	public class PositionController	: Controller
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
			var model = _positionBLL.GetById(id);
			return PartialView("_Edit", model);
		}

		[HttpPost]
		public IActionResult Edit(PositionDTO positionDTO)
		{
			try
			{
				_positionBLL.Update(positionDTO);
				TempData["Message"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>Position has been updated</div>";
			}
			catch (Exception ex)
			{
				TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			try
			{
				_positionBLL.Delete(id);
				TempData["Message"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>Position has been deleted</div>";
			}
			catch (Exception ex)
			{
				TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
			}

			return RedirectToAction("Index");
		}
	}
}
