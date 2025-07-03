using System;
using System.Linq;
using System.Collections.Generic;
	
namespace App015_20250701_EF6_SQLite
{   
	public  class DefectRepository : EFRepository<Defect>, IDefectRepository
	{

	}

	public  interface IDefectRepository : IRepository<Defect>
	{

	}
}