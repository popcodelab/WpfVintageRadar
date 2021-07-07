using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfVintageRadar.Commands;
using WpfVintageRadar.Controls.Models;

namespace WpfVintageRadar.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly int _screenDiameter;


        private bool _isCircularScreenOn;
        public bool IsCircularScreenOn
        {
            get => _isCircularScreenOn;
            set => SetProperty(ref _isCircularScreenOn, value);
        }

        private bool _isDetectionOn = false;
        public bool IsDetectionOn
        {
            get => _isDetectionOn;
            set => SetProperty(ref _isDetectionOn, value);
        }

        private int _radarItemsCount = 0;

        public int RadarItemsCount
        {
            get => _radarItemsCount;
            set=>SetProperty(ref _radarItemsCount, value);
        }

        private decimal _objectsSizeRatio = 0.12m;
        public decimal ObjectsSizeRatio
        {
            get => _objectsSizeRatio;
            set => SetProperty(ref _objectsSizeRatio , value);
        }


        private ObservableCollection<RadarItem> _radarItems;
        //private int _edgeThickness;

        public ObservableCollection<RadarItem> RadarItems
        {
            get => _radarItems;
            set =>SetProperty(ref _radarItems, value);
        }


        public MainViewModel(int screenDiameter)
        {
            _screenDiameter = screenDiameter;
            IncreaseObjectsCountCommand = new RelayCommand(IncreaseObjectsCount, CanModifyObjectsCount);
            DecreaseObjectsCountCommand = new RelayCommand(DecreaseObjectsCount, CanModifyObjectsCount);
            IncreaseObjectsSizeRatioCommand = new RelayCommand(IncreaseObjectsSizeRatio, CanModifyObjectsCount);
            DecreaseObjectsSizeRatioCommand = new RelayCommand(DecreaseObjectsSizeRatio, CanModifyObjectsCount);
            _radarItems = new ObservableCollection<RadarItem>();
        }

        #region Commands

        public ICommand IncreaseObjectsCountCommand { get; set; }
        public ICommand DecreaseObjectsCountCommand { get; set; }
        public ICommand IncreaseObjectsSizeRatioCommand { get; set; }
        public ICommand DecreaseObjectsSizeRatioCommand { get; set; }



        #endregion


        private void IncreaseObjectsCount(object value)
        {
            if (_radarItemsCount >= 5) return;
            _radarItems.Add(new RadarItem(_screenDiameter/2));
            RadarItemsCount++;
        }

        private void DecreaseObjectsCount(object value)
        {
            if (_radarItemsCount <= 0) return;
            RadarItemsCount--;
            _radarItems.RemoveAt(0);
            
        }

        private void IncreaseObjectsSizeRatio(object value)
        {
            if (_objectsSizeRatio >= 0.2m) return;
            ObjectsSizeRatio = ObjectsSizeRatio + 0.01m;
        }

        private void DecreaseObjectsSizeRatio(object value)
        {
            if (_objectsSizeRatio <=0.05m) return;
            ObjectsSizeRatio = ObjectsSizeRatio - 0.01m;
        }

        private bool CanModifyObjectsCount(object value)
        {
            return IsDetectionOn;
        }
    }
}
