using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
        public ObservableCollection<ComboBoxItem> LightList { get; set; }

        // 儲存料捲資料的按鈕可否被按，文字框可否被編輯
        private bool _boolSaveMaterial = false;
        public bool BoolSaveMaterial
        {
            get => _boolSaveMaterial;
            set
            {
                _boolSaveMaterial = value;
                OnPropertyChanged(nameof(BoolSaveMaterial));
            }
        }

        // Canvas的寬度
        private int _canvasWidth = 400;
        public int CanvasWidth {
            get => _canvasWidth;
            set
            {
                if (value > 400)
                    _canvasWidth = 400;
                else if (value < 50)
                    _canvasWidth = 50;
                else
                    _canvasWidth = value;
                OnPropertyChanged(nameof(CanvasWidth));
            }
        }

        // Canvas的高度
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


        // 可選擇兩個光源
        private ComboBoxItem _selectedLight;
        public ComboBoxItem SelectedLight
        {
            get => _selectedLight;
            set
            {
                _selectedLight = value;
                SelectedDefectImage = null; // 清除缺陷圖片
                OnPropertyChanged(nameof(SelectedLight));
                UpdateFilteredDefects(); // 光源變更時自動更新
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
                    ChangeToMaterial = CloneMaterial(_selectedMaterial);
                    BoolSaveMaterial = true; // 料捲修改改為可儲存
                    SelectedDefectImage = null; // 清除缺陷圖片
                    OnPropertyChanged(nameof(SelectedMaterial));
                    UpdateFilteredDefects();
                }
            }
        }

        //將修改參數為目標料捲
        private Material _changeToMaterial = new Material {roll_id = "N/A", roll_height = 0, roll_width = 0, index = -1 };
        public Material ChangeToMaterial
        {
            get => _changeToMaterial;
            set
            {
                if (_changeToMaterial != value)
                {
                    _changeToMaterial = value;
                    OnPropertyChanged(nameof(ChangeToMaterial));
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

        //目前選擇的缺陷圖片
        private BitmapImage _selectedDefectImage;
        public BitmapImage SelectedDefectImage
        {
            get => _selectedDefectImage;
            set
            {
                if (_selectedDefectImage != value)
                {
                    _selectedDefectImage = value;
                    OnPropertyChanged(nameof(SelectedDefectImage));
                }
            }
        }

        public RelayCommand SelectDefectImageCommand { get; }
        public RelayCommand SaveMaterialCommand { get; }

        #endregion

        public MainViewModel()
        {
            LoadDatabase();

            SelectDefectImageCommand = new RelayCommand(param =>
            {
                if (param is DefectModel defect)
                    ShowDefectImage(defect);
            });

            SaveMaterialCommand = new RelayCommand(param =>
            {
                if (ChangeToMaterial.index >= 0)
                    ChangeMaterialData(SelectedMaterial, ChangeToMaterial);
            });

            //初始化時選擇第一個光源
            if (LightList.Any())
                SelectedLight = LightList.First();
        }
        
        #region [Load Database Tables]
        private void LoadDatabase()
        {
            LoadMaterialTable();
            LoadImageTable();
            LoadDefectTable();
            LightList = [
                new ComboBoxItem() { Content = "光源A" },
                new ComboBoxItem() { Content = "光源B" }
            ];
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
        #region [Update Defects & Show Image]
        private void UpdateFilteredDefects()
        {
            if (SelectedMaterial != null)
            {
                // Canvas 尺寸計算
                CanvasWidth = (int)((double)SelectedMaterial.roll_width / 1880d * 400d);
                CanvasHeight = (int)(CanvasWidth / (double)SelectedMaterial.roll_width * (double)SelectedMaterial.roll_height);

                // 取得所選光源的名稱
                string selectedLightName = SelectedLight.Content.ToString();

                // 將光源名稱轉為 light 欄位的值（光源A=1, 光源B=2）
                long selectedLightValue = 0;
                if (selectedLightName == "光源A") selectedLightValue = 1;
                else if (selectedLightName == "光源B") selectedLightValue = 2;

                // 先找出屬於該 Material 且符合光源的所有 Image 的 index
                var imageIndexes = ImageList
                    .Where(img => img.material_index == SelectedMaterial.index
                               && img.light == selectedLightValue)
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
                        kind = d.kind,
                        reliability = (double)d.reliability,
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

        public void ShowDefectImage(DefectModel defect)
        {
            string imagePath = GetImagePathForDefect(defect);

            if (imagePath != null && File.Exists(imagePath))
                SelectedDefectImage = new BitmapImage(new Uri(imagePath));
            else
                SelectedDefectImage = null;
        }

        public string GetImagePathForDefect(DefectModel defect)
        {
            // 依據 DefectModel 的 DefectIndex 找到 Defect
            Defect defectEntity = DefectList.FirstOrDefault(d => d.index == defect.DefectIndex);
            if (defectEntity == null || !defectEntity.image_index.HasValue)
                return string.Empty;

            // 依據 Defect 的 image_index 找到 Image
            var imageEntity = ImageList.FirstOrDefault(img => img.index == defectEntity.image_index.Value);
            if (imageEntity == null)
                return string.Empty;

            // 回傳 Image 的 path 欄位
            return imageEntity.path ?? string.Empty;
        }

        #endregion
        #region [Change Material Data & Clone Material]
        private void ChangeMaterialData(Material original, Material updated)
        {
            if (original == null || updated == null)
                return;

            // 找到資料庫中的原始資料
            Material materialInDb = Materials.All().FirstOrDefault(m => m.index == original.index);
            if (materialInDb == null)
                return;

            // 更新欄位（只更新允許修改的欄位）
            materialInDb.roll_width = updated.roll_width;
            materialInDb.roll_height = updated.roll_height;
            materialInDb.roll_id = updated.roll_id;
            // TODO: 如有其他欄位需同步，請依需求補上

            // 提交變更
            Materials.UnitOfWork.Commit();

            // 更新本地 MaterialList 以反映變更
            int idx = MaterialList.IndexOf(original);
            Console.WriteLine(idx);
            if (idx >= 0)
            {
                MaterialList.Clear();
                foreach (var m in Materials.All())
                    MaterialList.Add(m);
                SelectedMaterial = MaterialList[idx];
            }
        }

        private Material CloneMaterial(Material source)
        {
            if (source == null) return null;
            return new Material
            {
                index = source.index,
                roll_width = source.roll_width,
                roll_height = source.roll_height,
                roll_id = source.roll_id
                // TODO: 其他欄位需複製依需求補上
            };
        }

        #endregion
    }
}