using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfVintageRadar.Shapes
{
    /// <summary>
    /// Draw a sector of circle 
    /// </summary>
    public class Sector : Shape
    {
        #region Dependency properties

        // Angle that arc starts at
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Sector), new PropertyMetadata(0.0, null, new CoerceValueCallback(new CoerceValueCallback(CoerceAngle))));


        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(Sector), new PropertyMetadata(90.0, null, new CoerceValueCallback(CoerceAngle)));


        #endregion

        #region Properties

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

        #endregion

        /// <summary>
        /// Constrains the angle to be with 0 - 360 degrees bounds
        /// </summary>
        /// <param name="dependencyObject">affected dependencyObject</param>
        /// <param name="value">the value</param>
        /// <returns>the angle</returns>
        private static object CoerceAngle(DependencyObject dependencyObject, object value)
        {
            var angle = (double)value;
            angle = Math.Min(angle, 359.9);
            angle = Math.Max(angle, 0.0);
            return angle;
        }

        /// <summary>
        /// Draws the shape
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                double maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThickness);
                double maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThickness);

                double xStart = maxWidth / 2.0 * Math.Cos(StartAngle * Math.PI / 180.0);
                double yStart = maxHeight / 2.0 * Math.Sin(StartAngle * Math.PI / 180.0);

                double xEnd = maxWidth / 2.0 * Math.Cos(EndAngle * Math.PI / 180.0);
                double yEnd = maxHeight / 2.0 * Math.Sin(EndAngle * Math.PI / 180.0);

                var streamGeometry = new StreamGeometry();
                using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
                {
                    streamGeometryContext.BeginFigure(
                        new Point((RenderSize.Width / 2.0) + xStart,
                                  (RenderSize.Height / 2.0) - yStart),
                        true,   // Filled
                        true);  // Closed
                    streamGeometryContext.ArcTo(
                        new Point((RenderSize.Width / 2.0) + xEnd,
                                  (RenderSize.Height / 2.0) - yEnd),
                        new Size(maxWidth / 2.0, maxHeight / 2),
                        0.0,     // rotationAngle
                        (EndAngle - StartAngle) > 180,   // greater than 180 deg?
                        SweepDirection.Counterclockwise,
                        true,    // isStroked
                        false);
                    streamGeometryContext.LineTo(new Point((RenderSize.Width / 2.0), (RenderSize.Height / 2.0)), true, false);
                }

                return streamGeometry;
            }
        }
    }
}
