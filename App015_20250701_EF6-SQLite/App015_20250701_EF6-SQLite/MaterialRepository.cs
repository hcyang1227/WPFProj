using System;
using System.Linq;
using System.Collections.Generic;
	
namespace App015_20250701_EF6_SQLite
{   
	public  class MaterialRepository : EFRepository<Material>, IMaterialRepository
	{

	}

	public  interface IMaterialRepository : IRepository<Material>
	{

	}
}