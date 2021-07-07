using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfVintageRadar.Controls
{
    /// <summary>
    /// Interaction logic for Mark.xaml
    /// </summary>
    public partial class Mark : UserControl, INotifyPropertyChanged
    {
        #region Dependency Properties

        public static DependencyProperty ShowMajorMarkLabelsProperty = DependencyProperty.Register("ShowMajorMarkLabels", typeof(bool), typeof(Mark), new PropertyMetadata(false));

        public static DependencyProperty MarkColorProperty = DependencyProperty.Register("MarkColor", typeof(Brush), typeof(Mark),new PropertyMetadata(Brushes.White));

        public static DependencyProperty LineMarginProperty = DependencyProperty.Register("LineMargin", typeof(Thickness), typeof(Mark),new PropertyMetadata(new Thickness(0, 0, 0, 0)));

        public static DependencyProperty LabelMarginProperty = DependencyProperty.Register("LabelMargin", typeof(Thickness), typeof(Mark),new PropertyMetadata(new Thickness(0, 20, 0, 0)));

        #endregion Dependency Properties

        #region Private class members

        private double _labelAngle;
        private string _label;
        private double _angle;
        private double _strokeThickness;
        private double _lineHeight;

        #endregion Private Vars

        #region Properties

        /// <summary>
        /// Shows the major mark labels
        /// </summary>
        public bool ShowMajorMarkLabels
        {
            get => (bool)GetValue(ShowMajorMarkLabelsProperty);
            set => SetValue(ShowMajorMarkLabelsProperty, value);
        }

        public Thickness LineMargin
        {
            get => (Thickness)GetValue(LineMarginProperty);
            set => SetValue(LineMarginProperty, value);
        }

        public Thickness LabelMargin
        {
            get => (Thickness)GetValue(LabelMarginProperty);
            set => SetValue(LabelMarginProperty, value);
        }

        

        public Brush MarkColor
        {
            get => (Brush)GetValue(MarkColorProperty);
            set => SetValue(MarkColorProperty, value);
        }

        public double LabelAngle
        {
            get => _labelAngle;
            set
            {
                _labelAngle = value;
                this.OnPropertyChanged("LabelAngle");
            }
        }

        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                this.OnPropertyChanged("Label");
            }
        }

        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                this.OnPropertyChanged("Angle");
                this.LabelAngle = value - 90;
            }
        }

        public double StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                this.OnPropertyChanged("StrokeThickness");
            }
        }

        public double LineHeight
        {
            get => _lineHeight;
            set
            {
                _lineHeight = value;
                this.OnPropertyChanged("LineHeight");
                this.LineMargin = new Thickness(55 - this.LineHeight, 0, 0, 0);
            }
        }

        #endregion Public Vars

        public Mark()
        {
            InitializeComponent();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

    }
}
