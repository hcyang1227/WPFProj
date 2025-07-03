using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
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

        private int _canvasWidth = 400;
        public int CanvasWidth {
            get => _canvasWidth;
            set
            {
                if (value > 400)
                    _canvasWidth = 400;
                else
                    _canvasWidth = value;
                OnPropertyChanged(nameof(CanvasWidth));
            }
        }
        private int _canvasHeight = 1200;
        public int CanvasHeight
        {
            get => _canvasHeight;
            set
            {
                _canvasHeight = value;
                OnPropertyChanged(nameof(CanvasHeight));
            }
        }

        //目前選擇的料捲
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

        //被篩選出的defect清單
        private ObservableCollection<DefectModel> _filteredDefectList = new ObservableCollection<DefectModel>();
        public ObservableCollection<DefectModel> FilteredDefectList
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

            // 初始化時載入第一卷料捲
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
                // 計算Canva應該要有的長度(寬度固定)
                CanvasWidth = (int)((double)SelectedMaterial.roll_width / 1880d * 400d);
                CanvasHeight = (int)(CanvasWidth / (double)SelectedMaterial.roll_width * (double)SelectedMaterial.roll_height);

                // 先找出屬於該 Material 的所有 Image 的 index
                var imageIndexes = ImageList
                    .Where(img => img.material_index == SelectedMaterial.index)
                    .Select(img => img.index)
                    .ToHashSet();

                // 再找出這些 Image 的所有 Defect
                var filtered = DefectList
                    .Where(d => d.image_index.HasValue && imageIndexes.Contains(d.image_index.Value))
                    .Select(d => new DefectModel
                    {
                        DefectIndex = d.index,
                        roll_x = (double)d.roll_x,
                        roll_y = (double)d.roll_y,
                        roll_width = (double)SelectedMaterial.roll_width,
                        roll_height = (double)SelectedMaterial.roll_height,
                        CanvasWidth = CanvasWidth,
                        CanvasHeight = CanvasHeight
                    });

                FilteredDefectList = new ObservableCollection<DefectModel>(filtered);
            }
            else
            {
                FilteredDefectList = new ObservableCollection<DefectModel>();
            }
        }

        #endregion
    }
}