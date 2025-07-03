using System;
using System.Linq;
using System.Collections.Generic;
	
namespace App015_20250701_EF6_SQLite
{   
	public  class ImageRepository : EFRepository<Image>, IImageRepository
	{

	}

	public  interface IImageRepository : IRepository<Image>
	{

	}
}