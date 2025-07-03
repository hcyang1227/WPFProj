using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        #endregion
        
        public MainViewModel()
        {
            LoadDatabase();

            SelectDefectImageCommand = new RelayCommand(param =>
            {
                if (param is DefectModel defect)
                    ShowDefectImage(defect);
            });

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
                // �p��Canva���ӭn��������(�e�שT�w)
                CanvasWidth = (int)((double)SelectedMaterial.roll_width / 1880d * 400d);
                CanvasHeight = (int)(CanvasWidth / (double)SelectedMaterial.roll_width * (double)SelectedMaterial.roll_height);

                // ����X�ݩ�� Material ���Ҧ� Image �� index
                var imageIndexes = ImageList
                    .Where(img => img.material_index == SelectedMaterial.index)
                    .Select(img => img.index)
                    .ToHashSet();

                // �A��X�o�� Image ���Ҧ� Defect
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
            // �̾� DefectModel �� DefectIndex ��� Defect
            Defect defectEntity = DefectList.FirstOrDefault(d => d.index == defect.DefectIndex);
            if (defectEntity == null || !defectEntity.image_index.HasValue)
                return string.Empty;

            // �̾� Defect �� image_index ��� Image
            var imageEntity = ImageList.FirstOrDefault(img => img.index == defectEntity.image_index.Value);
            if (imageEntity == null)
                return string.Empty;

            // �^�� Image �� path ���
            return imageEntity.path ?? string.Empty;
        }

        #endregion
    }
}