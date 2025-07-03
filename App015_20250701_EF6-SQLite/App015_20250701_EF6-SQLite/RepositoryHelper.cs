namespace App015_20250701_EF6_SQLite
{
	public static class RepositoryHelper
	{
		public static IUnitOfWork GetUnitOfWork()
		{
			return new EFUnitOfWork();
		}		
		
		public static DefectRepository GetDefectRepository()
		{
			var repository = new DefectRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static DefectRepository GetDefectRepository(IUnitOfWork unitOfWork)
		{
			var repository = new DefectRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ImageRepository GetImageRepository()
		{
			var repository = new ImageRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ImageRepository GetImageRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ImageRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static MaterialRepository GetMaterialRepository()
		{
			var repository = new MaterialRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static MaterialRepository GetMaterialRepository(IUnitOfWork unitOfWork)
		{
			var repository = new MaterialRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		
	}
}