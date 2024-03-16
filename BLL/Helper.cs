using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL
{
	public class Helper
	{
		public static bool IsImageFile(string filePath)
		{
			string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
			string extension = Path.GetExtension(filePath);
			return imageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
		}
	}
}
