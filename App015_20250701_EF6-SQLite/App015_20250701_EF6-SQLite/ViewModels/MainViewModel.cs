using System.Collections.ObjectModel;
using System.Linq;

namespace App015_20250701_EF6_SQLite.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region [Initial Variable]
        public EFRepository<Material> Materials { set; get; }
        public EFRepository<Image> Images { set; get; }
        public EFRepository<Defect> Defects { set; get; }
        public ObservableCollection<Material> MaterialList { set; get; }
        public ObservableCollection<Image> ImageList { set; get; }
        public ObservableCollection<Defect> DefectList { set; get; }

        #endregion

        public MainViewModel()
        {
            LoadDatabase();
        }

        private void LoadDatabase()
        {
            LoadMaterialTable();
            LoadImageTable();
            LoadDefectTable();
        }

        #region [Load Table]

        private void LoadMaterialTable()
        {
            Materials = new EFRepository<Material>
            {
                UnitOfWork = new EFUnitOfWork
                {
                    Context = new mainEntitiesDingZing()
                }
            };
            MaterialList = new ObservableCollection<Material>(Materials.All().ToList());
        }

        private void LoadImageTable()
        {
            Images = new EFRepository<Image>
            {
                UnitOfWork = new EFUnitOfWork
                {
                    Context = new mainEntitiesDingZing()
                }
            };
            ImageList = new ObservableCollection<Image>(Images.All().ToList());
        }


        private void LoadDefectTable()
        {
            Defects = new EFRepository<Defect>
            {
                UnitOfWork = new EFUnitOfWork
                {
                    Context = new mainEntitiesDingZing()
                }
            };
            DefectList = new ObservableCollection<Defect>(Defects.All().ToList());
        }

        #endregion
        #region [Add Data To Table]

        private void AddMaterialData(Material material)
        {
            Materials.Add(material);
            Materials.UnitOfWork.Commit();
        }

        private void AddImageData(Image image)
        {
            Images.Add(image);
            Images.UnitOfWork.Commit();
        }

        private void AddDefectData(Defect defect)
        {
            Defects.Add(defect);
            Defects.UnitOfWork.Commit();
        }

        #endregion
        #region [Delete Data From Table]
        private void DeleteMaterialData(Material material)
        {
            Materials.Delete(material);
            Materials.UnitOfWork.Commit();
        }

        private void DeleteImageData(Image image)
        {
            Images.Delete(image);
            Images.UnitOfWork.Commit();
        }

        private void DeleteDefectData(Defect defect)
        {
            Defects.Delete(defect);
            Defects.UnitOfWork.Commit();
        }
        #endregion
    }
}