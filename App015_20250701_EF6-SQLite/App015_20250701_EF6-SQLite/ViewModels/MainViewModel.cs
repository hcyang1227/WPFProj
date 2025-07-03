using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace App015_20250701_EF6_SQLite.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region [Initial Variable]
        public EFRepository<Material> Materials { get; set; }
        public EFRepository<Image> Images { get; set; }
        public EFRepository<Defect> Defects { get; set; }
        public ObservableCollection<Material> MaterialList { get; set; }
        public ObservableCollection<Image> ImageList { get; set; }
        public ObservableCollection<Defect> DefectList { get; set; }

        //�ثe��ܪ��Ʊ�
        private Material _selectedMaterial;
        public Material SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                if (_selectedMaterial != value)
                {
                    _selectedMaterial = value;
                    OnPropertyChanged(nameof(SelectedMaterial));
                    UpdateFilteredDefects();
                }
            }
        }

        //�Q�z��X��defect�M��
        private ObservableCollection<Defect> _filteredDefectList = new ObservableCollection<Defect>();
        public ObservableCollection<Defect> FilteredDefectList
        {
            get => _filteredDefectList;
            set
            {
                _filteredDefectList = value;
                OnPropertyChanged(nameof(FilteredDefectList));
            }
        }
        #endregion
        
        public MainViewModel()
        {
            LoadDatabase();

            // ��l�Ʈɸ��J�Ĥ@���Ʊ�
            if (MaterialList.Any())
                SelectedMaterial = MaterialList.First();
        }
        
        #region [Load Database Tables]
        private void LoadDatabase()
        {
            LoadMaterialTable();
            LoadImageTable();
            LoadDefectTable();
        }

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
        #region [Update Filtered Defects]
        private void UpdateFilteredDefects()
        {
            if (SelectedMaterial != null)
            {
                // ����X�ݩ�� Material ���Ҧ� Image �� index
                var imageIndexes = ImageList
                    .Where(img => img.material_index == SelectedMaterial.index)
                    .Select(img => img.index)
                    .ToHashSet();

                // �A��X�o�� Image ���Ҧ� Defect
                var filtered = DefectList
                    .Where(d => d.image_index.HasValue && imageIndexes.Contains(d.image_index.Value));

                FilteredDefectList = new ObservableCollection<Defect>(filtered);
            }
            else
            {
                FilteredDefectList = new ObservableCollection<Defect>();
            }
        }
        #endregion
    }
}