using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfVintageRadar.Controls;
using WpfVintageRadar.Controls.Helpers;
using WpfVintageRadar.Controls.Models;

namespace WpfVintageRadar.Controls
{
    /// <summary>
    /// Interaction logic for CircularScreen.xaml
    /// </summary>
    public partial class CircularScreen : UserControl
    {
        #region Private class members

        private int _screwHeadSize;

        #endregion

        #region Dependency properties

        public static DependencyProperty IsOnProperty =
            DependencyProperty.Register("IsOn", typeof(bool), typeof(CircularScreen), new PropertyMetadata(false, OnCircularScreenChanged));

        public static DependencyProperty IsDetectionOnProperty =
            DependencyProperty.Register("IsDetectionOn", typeof(bool), typeof(CircularScreen), new PropertyMetadata(false, OnCircularScreenChanged));

        public static DependencyProperty EdgeThicknessProperty =
            DependencyProperty.Register("EdgeThickness", typeof(int), typeof(CircularScreen), new PropertyMetadata(10));

        public static DependencyProperty EdgeLengthProperty =
            DependencyProperty.Register("EdgeLength", typeof(int), typeof(CircularScreen), new PropertyMetadata(800, OnCircularScreenChanged));


        #region Major marks

        public static DependencyProperty ShowMajorMarkLabelsProperty = DependencyProperty.Register("ShowMajorMarkLabels", typeof(bool), typeof(CircularScreen), new PropertyMetadata(false, OnCircularScreenChanged));
        public static DependencyProperty MajorMarksCountProperty = DependencyProperty.Register("MajorMarksCount", typeof(int), typeof(CircularScreen), new PropertyMetadata(10, OnCircularScreenChanged));
        public static DependencyProperty MajorMarksProperty = DependencyProperty.Register("MajorMarks", typeof(ObservableCollection<Mark>), typeof(CircularScreen));
        public static DependencyProperty MajorMarkColorProperty = DependencyProperty.Register("MajorMarkColor", typeof(Brush), typeof(CircularScreen), new PropertyMetadata(Brushes.White, OnMajorMarkColorChanged));

        #endregion

        #region Minor marks

        public static DependencyProperty MinorMarksProperty = DependencyProperty.Register("MinorMarks", typeof(ObservableCollection<Mark>), typeof(CircularScreen));
        public static DependencyProperty MinorMarkColorProperty = DependencyProperty.Register("MinorMarkColor", typeof(Brush), typeof(CircularScreen), new PropertyMetadata(Brushes.White, OnMinorMarkColorChanged));
        public static DependencyProperty MinorMarksCountProperty = DependencyProperty.Register("MinorMarksCount", typeof(int), typeof(CircularScreen), new PropertyMetadata(4, OnCircularScreenChanged));

        #endregion

        public static DependencyProperty LineMarginProperty =
            DependencyProperty.Register("LineMargin", typeof(Thickness), typeof(CircularScreen), new PropertyMetadata(new Thickness(40, 0, 0, 0)));

        public static DependencyProperty LabelMarginProperty =
            DependencyProperty.Register("LabelMargin", typeof(Thickness), typeof(CircularScreen), new PropertyMetadata(new Thickness(0, 20, 0, 0)));

        #region Values

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(CircularScreen), new PropertyMetadata(double.NegativeInfinity, OnCircularScreenChanged));

        public static DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(CircularScreen), new PropertyMetadata(double.NegativeInfinity));

        public static DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(CircularScreen), new PropertyMetadata(0.0d));

        #endregion

        public static DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(CircularScreen), new PropertyMetadata(0.0, OnCircularScreenChanged));


        public static DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(CircularScreen), new PropertyMetadata(360.0, OnCircularScreenChanged));

        public static DependencyProperty RadarItemsProperty = DependencyProperty.Register("RadarItems", typeof(ObservableCollection<RadarItem>), typeof(CircularScreen));

        public static DependencyProperty RadarItemsCountProperty = DependencyProperty.Register("RadarItemsCount", typeof(int), typeof(CircularScreen), new PropertyMetadata(0));
        
        public static DependencyProperty RadarItemsImageSizeRatioProperty = DependencyProperty.Register("RadarItemsImageSizeRatio", typeof(double), typeof(CircularScreen), new PropertyMetadata(0.05));



        #endregion

        #region  Custom RoutedEvent

        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent SwitchOnOffEvent = EventManager.RegisterRoutedEvent("SwitchOnOff", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CircularScreen));
        // Provide CLR accessors for the event
        public event RoutedEventHandler SwitchOnOff
        {
            add => AddHandler(SwitchOnOffEvent, value);
            remove => RemoveHandler(SwitchOnOffEvent, value);
        }
        /// <summary>
        /// Raises the SwitchOnOff event
        /// </summary>
        public void RaiseSwitchOnOffEvent()
        {
            var newEventArgs = new RoutedEventArgs(SwitchOnOffEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Edge length, used to make sure the control is a square
        /// </summary>
        public int EdgeLength
        {
            get => (int)GetValue(EdgeLengthProperty);
            set => SetValue(EdgeLengthProperty, value);
        }

        /// <summary>
        /// Shows the major mark labels 
        /// </summary>
        public bool ShowMajorMarkLabels
        {
            get => (bool)GetValue(ShowMajorMarkLabelsProperty);
            set => SetValue(ShowMajorMarkLabelsProperty, value);
        }

        /// <summary>
        /// Is the control turned ON
        /// </summary>
        public bool IsOn
        {
            get => (bool)GetValue(IsOnProperty);
            set => SetValue(IsOnProperty, value);
        }

        /// <summary>
        /// Is the radar detection turned ON
        /// </summary>
        public bool IsDetectionOn
        {
            get => (bool)GetValue(IsDetectionOnProperty);
            set => SetValue(IsDetectionOnProperty, value);
        }

        /// <summary>
        /// Control edge thickness
        /// </summary>
        public int EdgeThickness
        {
            get => (int)GetValue(EdgeThicknessProperty);
            set => SetValue(EdgeThicknessProperty, value);
        }

        /// <summary>
        /// Main ticks representing some relevant angles
        /// </summary>
        public int MajorMarksCount
        {
            get => (int)GetValue(MajorMarksCountProperty);
            set => SetValue(MajorMarksCountProperty, value);
        }

        /// <summary>
        /// Small ticks representing degrees values
        /// </summary>
        public int MinorMarksCount
        {
            get => (int)GetValue(MinorMarksCountProperty);
            set => SetValue(MinorMarksCountProperty, value);
        }

        /// <summary>
        /// Stores all the relevant marks (angles)
        /// </summary>
        public ObservableCollection<Mark> MajorMarks
        {
            get => (ObservableCollection<Mark>)GetValue(MajorMarksProperty);
            set => SetValue(MajorMarksProperty, value);
        }

        /// <summary>
        /// Color of the major marks 
        /// </summary>
        public Brush MajorMarkColor
        {
            get => (Brush)GetValue(MajorMarkColorProperty);
            set => SetValue(MajorMarkColorProperty, value);
        }

        /// <summary>
        /// Minor marks 
        /// </summary>
        public ObservableCollection<Mark> MinorMarks
        {
            get => (ObservableCollection<Mark>)GetValue(MinorMarksProperty);
            set => SetValue(MinorMarksProperty, value);
        }

        /// <summary>
        /// Color of the major marks 
        /// </summary>
        public Brush MinorMarkColor
        {
            get => (Brush)GetValue(MinorMarkColorProperty);
            set => SetValue(MinorMarkColorProperty, value);
        }


        /// <summary>
        /// Line margin
        /// </summary>
        public Thickness LineMargin
        {
            get => (Thickness)GetValue(LineMarginProperty);
            set => SetValue(LineMarginProperty, value);
        }

        /// <summary>
        /// Line margin
        /// </summary>
        public Thickness LabelMargin
        {
            get => (Thickness)GetValue(LabelMarginProperty);
            set => SetValue(LabelMarginProperty, value);
        }
        //TODO rename this one
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public double MinValue
        {
            get => (double)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }


        /// <summary>
        /// Start angle
        /// </summary>
        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        /// <summary>
        /// End angle
        /// </summary>
        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        /// <summary>
        /// Objects which are displayer on the screen
        /// </summary>
        public ObservableCollection<RadarItem> RadarItems
        {
            get => (ObservableCollection<RadarItem>)GetValue(RadarItemsProperty);
            set => SetValue(RadarItemsProperty, value);
        }


        public int RadarItemsCount
        {
            get => (int)GetValue(RadarItemsCountProperty);
            set => SetValue(RadarItemsCountProperty, value);
        }

        public double RadarItemsImageSizeRatio
        {
            get => (double) GetValue(RadarItemsImageSizeRatioProperty);
            set => SetValue(RadarItemsImageSizeRatioProperty, value);
        }



        #endregion

        #region Event handlers

        /// <summary>
        /// Something has changed in CircularScreen control
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="e"></param>
        public static void OnCircularScreenChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var circularScreen = dependencyObject as CircularScreen;
            if (circularScreen != null && !circularScreen.IsOn)
            {
                circularScreen.DynamicLayout.Children.Clear();
            }
            else
            {
                circularScreen?.RaiseSwitchOnOffEvent();
                circularScreen?.DrawScrewHeads();
                circularScreen?.UpdateUIElements();
            }

        }

        /// <summary>
        /// The major mark color has been changed
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="e"></param>
        public static void OnMajorMarkColorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var circularScreen = dependencyObject as CircularScreen;
            if (circularScreen != null && circularScreen.MajorMarks == null) return;
            foreach (var mark in circularScreen.MajorMarks)
            {
                mark.MarkColor = circularScreen.MajorMarkColor;
            }
        }

        /// <summary>
        /// The minor mark color has been changed
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="e"></param>
        public static void OnMinorMarkColorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var circularScreen = dependencyObject as CircularScreen;
            if (circularScreen != null && circularScreen.MinorMarks == null) return;
            foreach (var mark in circularScreen.MinorMarks)
                mark.MarkColor = circularScreen.MinorMarkColor;
        }

        /// <summary>
        /// Once the user control is loaded do this stuff
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircularScreen_Loaded(object sender, RoutedEventArgs e)
        {
            _screwHeadSize = ComputeScrewHeadSize();
            if (_screwHeadSize != 0)
            {
                DrawScrewHeads();
            }
            UpdateUIElements();

        }

        #endregion


        /// <summary>
        /// Default constructor
        /// </summary>
        public CircularScreen()
        {
            InitializeComponent();
            // Add events
            Loaded += new RoutedEventHandler(CircularScreen_Loaded);


            var radarItemsMotionDispatcherTimer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(0.25) };
            radarItemsMotionDispatcherTimer.Tick += ComputeRadarItemsMotionDispatchersMotion;
            radarItemsMotionDispatcherTimer.Start();
        }

        #region Private methods

        #region Screw heads

        /// <summary>
        /// Draws the 4 screw heads of the screen
        /// </summary>
        private void DrawScrewHeads()
        {
            var offset = Width - EdgeThickness;
            var cos45 = (Math.Cos(Math.PI / 4)) * offset;
            var sin45 = (Math.Sin(Math.PI / 4)) * offset;

            var screwHeadImageSource = new BitmapImage(new Uri(@"pack://application:,,,/WpfVintageRadar.Controls;component/Assets/Images/screw_head.png"));
            var topRightScrewHead = new Image()
            {
                Source = screwHeadImageSource,
                Width = _screwHeadSize,
                Height = _screwHeadSize,
                Margin = new Thickness(cos45, 0, 0, sin45)
            };
            LayoutRoot.Children.Add(topRightScrewHead);

            var bottomRightScrewHead = new Image()
            {
                Source = screwHeadImageSource,
                Width = _screwHeadSize,
                Height = _screwHeadSize,
                Margin = new Thickness(cos45, 0, 0, -sin45)
            };
            LayoutRoot.Children.Add(bottomRightScrewHead);

            var bottomLeftScrewHead = new Image()
            {
                Source = screwHeadImageSource,
                Width = _screwHeadSize,
                Height = _screwHeadSize,
                Margin = new Thickness(-cos45, 0, 0, -sin45)
            };
            LayoutRoot.Children.Add(bottomLeftScrewHead);

            var topLeftScrewHead = new Image()
            {
                Source = screwHeadImageSource,
                Width = _screwHeadSize,
                Height = _screwHeadSize,
                Margin = new Thickness(-cos45, 0, 0, sin45)
            };
            LayoutRoot.Children.Add(topLeftScrewHead);
        }

        /// <summary>
        /// Computes the screw head image size depending of the circular screen edge thickness
        /// </summary>
        /// <returns></returns>
        private int ComputeScrewHeadSize()
        {
            var width = EdgeThickness - 6;
            return width >= 5 ? width : 0;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void UpdateUIElements()
        {
            if (!IsOn) return;


            DynamicLayout.Children.Clear();
            var majorMarksIncrement = 360 / MajorMarksCount;
            var angleIncrement = majorMarksIncrement / (MinorMarksCount + 1);
            // Major marks
            MajorMarks = new ObservableCollection<Mark>();
            for (var i = 0; i <= this.MajorMarksCount; i++)
            {
                var mark = new Mark
                {
                    StrokeThickness = 8,
                    LineHeight = 10,
                    Angle = (i * majorMarksIncrement) + this.StartAngle,
                    Label = (i * majorMarksIncrement).ToString()
                };

                Panel.SetZIndex(mark, -1);

                this.MajorMarks.Add(mark);
                DynamicLayout.Children.Add(mark);

                mark.MarkColor = MajorMarkColor;
                mark.LineMargin = LineMargin;
                mark.LabelMargin = LabelMargin;
                mark.ShowMajorMarkLabels = ShowMajorMarkLabels;
            }

            // Minor marks
            MinorMarks = new ObservableCollection<Mark>();

            foreach (var majorMark in MajorMarks)
            {
                if (MajorMarks.IndexOf(majorMark) < MajorMarks.Count - 1)
                {
                    for (var i = 0; i <= MinorMarksCount; i++)
                    {
                        var mark = new Mark()
                        {
                            StrokeThickness = 4,
                            LineHeight = 5,
                            Angle = (i * angleIncrement) + angleIncrement + majorMark.Angle,
                            MarkColor = MinorMarkColor,
                            LineMargin = LineMargin,
                            LabelMargin = LabelMargin
                        };
                        Panel.SetZIndex(mark, 0);
                        MinorMarks.Add(mark);
                        DynamicLayout.Children.Add(mark);
                    }
                }
            }
        }


        /// <summary>
        /// Calculates objects motions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComputeRadarItemsMotionDispatchersMotion(object sender, EventArgs e)
        {
            if (!IsDetectionOn) return;
            Debug.WriteLine($"RadarItems count : {RadarItems.Count} - ToBeRemoved : {RadarItems.Count(ri => ri.State == RadarItem.RadarItemState.ToBeRemoved)}");
            
            RadarItemsLayout.Children.Clear();
            RemoveRadarItemsToBeDeleted();
            if (RadarItems.Count < RadarItemsCount)
            {
                var nbRadarItemsToCreate = RadarItemsCount - RadarItems.Count;
                for (int i = 0; i < nbRadarItemsToCreate; i++)
                {
                    AddNewRadarItem();
                }
            }
            //if (RadarItems.Count!=)
            foreach (var radarItem in RadarItems)
            {

                var image = new Image()
                {
                    Source = radarItem.ImageSource,
                    Width = RadarItemsImageSizeRatio*EdgeLength/2,
                    Height = RadarItemsImageSizeRatio * EdgeLength / 2,
                    Stretch = Stretch.Uniform,
                    //Visibility = (radarItem.State==RadarItem.RadarItemState.Moving?Visibility.Visible:Visibility.Collapsed)
                };
                var XBound = ComputesXBound(radarItem.Velocity.X, radarItem.Angle, image.Width);
                var YBound = ComputesYBound(radarItem.Velocity.Y, radarItem.Angle, image.Height);
                //Debug.WriteLine($"B O U N D S   ==>X : {radarItem.Location.X} - Y : {radarItem.Location.Y} - xbound : {XBound} - ybound : {YBound}");

                #region Bounds

                if (XBound < 0)
                {
                    if (radarItem.Location.X < XBound)
                    {
                        //RadarItems.Remove(radarItem);
                        radarItem.State = RadarItem.RadarItemState.ToBeRemoved;
                        //AddNewRadarItem();
                        return;
                    }
                }
                else
                {
                    if (radarItem.Location.X > XBound)
                    {

                        
                        //RadarItems.Remove(radarItem);
                        radarItem.State = RadarItem.RadarItemState.ToBeRemoved;
                        //AddNewRadarItem();
                        return;
                    }
                }

                if (YBound < 0)
                {
                    if (radarItem.Location.Y < YBound)
                    {

                        
                        //RadarItems.Remove(radarItem);
                        radarItem.State = RadarItem.RadarItemState.ToBeRemoved;
                        //AddNewRadarItem();
                        return;
                    }
                }
                else
                {
                    if (radarItem.Location.Y > YBound)
                    {

                        
                        //RadarItems.Remove(radarItem);
                        radarItem.State = RadarItem.RadarItemState.ToBeRemoved;
                        //AddNewRadarItem();
                        return;
                    }
                }
                #endregion

                //if (radarItem.Location.X < -Xbound || radarItem.Location.X > Xbound)
                //{
                //    radarItem.Velocity = new Vector(-1 * radarItem.Velocity.X, radarItem.Velocity.Y);
                //}
                //if (radarItem.Location.Y < -Ybound || radarItem.Location.Y > Ybound)
                //{
                //    radarItem.Velocity = new Vector(radarItem.Velocity.X, -1 * radarItem.Velocity.Y);
                //}

                // add the velocity vector for the radar item motion 
                radarItem.Location = Point.Add(radarItem.Location, radarItem.Velocity);

                var translateTransform = new TranslateTransform(radarItem.Location.X, radarItem.Location.Y);
                var rotateTransform = new RotateTransform(radarItem.Angle);

                //Debug.WriteLine($"angle : {radarItem.Angle} - InnerEllipse.ActualWidth : {InnerEllipse.ActualWidth} - radarItem.Location.X {radarItem.Location.X} - radarItem.Location.Y {radarItem.Location.Y}");


                var transformGroup = new TransformGroup();
                transformGroup.Children.Add(rotateTransform);
                transformGroup.Children.Add(translateTransform);

                image.RenderTransformOrigin = new Point(0.5, 0.5);
                image.RenderTransform = transformGroup;
                //image.Visibility=Visibility.Hidden;

                RadarItemsLayout.Children.Add(image);

            }
        }

        /// <summary>
        /// Computes the X bound (the limit that the flying object can reach)
        /// </summary>
        /// <param name="velocityX">the velocity X </param>
        /// <param name="angle">Angle of the flying object</param>
        /// <param name="spriteWidth">The sprite width</param>
        /// <returns>The X bound</returns>
        private int ComputesXBound(double velocityX, double angle, double spriteWidth)
        {
            var Xbound = (Math.Cos(MathsHelper.ConvertDegreesToRadians(angle)) * (EdgeLength / 2)) - EdgeThickness;
            if (velocityX > 0)
            {
                Xbound = Xbound + spriteWidth;
            }

            return (int)Xbound;
        }

        /// <summary>
        /// Computes the X bound (the limit that the flying object can reach)
        /// </summary>
        /// <param name="velocityY">The velocity Y</param>
        /// <param name="angle">Angle of the flying object</param>
        /// <param name="spriteHeight">The sprite height</param>
        /// <returns></returns>
        private int ComputesYBound(double velocityY, double angle, double spriteHeight)
        {
            var YBound = (Math.Sin(MathsHelper.ConvertDegreesToRadians(angle)) * (EdgeLength / 2)) - EdgeThickness;
            if (velocityY > 0)
            {
                YBound = YBound + spriteHeight;
            }

            return (int)YBound;
        }

        #region RadarItems manager

        private void AddNewRadarItem()
        {
            //TODO Improve this crappy code ASAP
            var radarItem = new RadarItem((EdgeLength - EdgeThickness) / 2, true);
            RadarItems.Add(radarItem);
        }

        private void RemoveRadarItemsToBeDeleted()
        {
            if (RadarItems.Count == 0 ) return;
            var itemsToRemove = RadarItems.Where(ri => ri.State == RadarItem.RadarItemState.ToBeRemoved).ToList();
            if (itemsToRemove.Count == 0) return;
            
            foreach (var item in itemsToRemove)
            {
                RadarItems.Remove(item);
            }
            //Debug.WriteLine($"!! RadarItems removed {itemsToRemove.Count} items !!");
        }

        #endregion

        #endregion
    }
}
