using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfVintageRadar.Controls.Helpers;

namespace WpfVintageRadar.Controls.Models
{

    public class RadarItem : INotifyPropertyChanged
    {

        #region Private class members

        private Point _location;
        private Vector _velocity;
        private RadarItemType _type;
        private RadarItemState _state;
        private double _angle;

        #endregion

        private readonly Random _random = new Random();

        /// <summary>
        /// Types of the radar item
        /// </summary>
        public enum RadarItemType { Airplane = 0, Helicopter = 1, FighterJet = 2, Airliner = 3 }
        /// <summary>
        /// State of the radar item
        /// </summary>
        public enum RadarItemState { Moving = 1, ToBeRemoved = 0 }



        /// <summary>
        /// Creates a radar item. Its localization is bounded to the radius of the radar
        /// </summary>
        /// <param name="radarRadius">the radius of the radar</param>
        /// <param name="mustShowUpFromTheEgde">If the user increase or decrease the number of items,
        /// the new ones can appears from anywhere otherwise, the next detected has to show up from the edge of the screen</param>
        public RadarItem(int radarRadius, bool mustShowUpFromTheEgde = false)
        {
            _type = (RadarItemType)_random.Next(4);
            var maxVelocityValue = GetMaxVelocityFromType();
            // Location
            if (mustShowUpFromTheEgde)
            {
                // Velocity must be calculated to make sure that the object will head to the center of the screen (departure from the edge)
                do
                {
                    _velocity = new Vector(_random.Next(-maxVelocityValue, maxVelocityValue), _random.Next(-maxVelocityValue, maxVelocityValue));
                } while (_velocity.X == 0 && _velocity.Y == 0);

                var angle = _random.Next(360);
                var x = Math.Cos(MathsHelper.ConvertDegreesToRadians(angle)) * radarRadius;
                var y = Math.Sin(MathsHelper.ConvertDegreesToRadians(angle)) * radarRadius;
                if (x < 0)
                {
                    _velocity.X = Math.Abs(_velocity.X);
                }
                else
                {
                    _velocity.X = -Math.Abs(_velocity.X);
                }

                if (y < 0)
                {
                    _velocity.Y = Math.Abs(_velocity.Y);
                }
                else
                {
                    _velocity.Y = -Math.Abs(_velocity.Y);
                }

                _location = new Point(x, y);
            }

            else
            {
                // Velocity can randomized on X and Y because can appear anywhere on the screen
                do
                {
                    _velocity = new Vector(_random.Next(-maxVelocityValue, maxVelocityValue), _random.Next(-maxVelocityValue, maxVelocityValue));
                } while (_velocity.X == 0 && _velocity.Y == 0);
                _location = new Point(_random.Next(-radarRadius, radarRadius), _random.Next(-radarRadius, radarRadius));
            }

            _state = RadarItemState.Moving;
            _angle = MathsHelper.ConvertRadiansToDegrees(Math.Atan2(_velocity.Y, _velocity.X));
            Debug.WriteLine($"radar type : {_type} - radarRadius : {radarRadius} - location X : {_location.X} - location Y : {_location.Y} - velocity X : {_velocity.X}  - velocity Y : {_velocity.Y} - _angle : {_angle} - State : {State}");
        }

        #region  Properties

        public Point Location
        {
            get => _location;
            set
            {
                if (_location == value) return;
                _location = value;
                NotifyPropertyChanged(nameof(Location));
            }
        }

        public Vector Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                NotifyPropertyChanged(nameof(Velocity));
            }
        }

        public RadarItemType Type
        {
            get => _type;
            set
            {
                _type = value;
                NotifyPropertyChanged(nameof(Type));
            }
        }

        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                NotifyPropertyChanged(nameof(Angle));
            }
        }

        public RadarItemState State
        {
            get => _state;
            set
            {
                _state = value;
                NotifyPropertyChanged(nameof(State));
            }
        }

        public BitmapImage ImageSource
        {
            get
            {
                switch (_type)
                {
                    case RadarItemType.Airliner:
                        return new BitmapImage(new Uri(
                            "pack://application:,,,/WpfVintageRadar.Controls;component/Assets/Images/RadarItems/Commercial.png"));
                    case RadarItemType.Helicopter:
                        return new BitmapImage(new Uri(
                            "pack://application:,,,/WpfVintageRadar.Controls;component/Assets/Images/RadarItems/Helicopter.png"));
                    case RadarItemType.FighterJet:
                        return new BitmapImage(new Uri(
                            "pack://application:,,,/WpfVintageRadar.Controls;component/Assets/Images/RadarItems/fighterjet.png"));
                    case RadarItemType.Airplane:
                        return new BitmapImage(new Uri(
                            "pack://application:,,,/WpfVintageRadar.Controls;component/Assets/Images/RadarItems/airplane.png"));
                    default:
                        return new BitmapImage(new Uri(
                            "pack://application:,,,/WpfVintageRadar.Controls;component/Assets/Images/RadarItems/Airline.png"));
                }
            }
        }



        #endregion

        #region Event handler

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        #region

        //TODO compute a minimale one

        /// <summary>
        /// Return the maximal velocity value depending of the Type or radar item
        /// </summary>
        /// <returns>the maximal velocity value</returns>
        private int GetMaxVelocityFromType()
        {
            switch (Type)
            {
                case RadarItemType.Airliner: return 5;
                case RadarItemType.Airplane: return 4;
                case RadarItemType.FighterJet: return 6;
                case RadarItemType.Helicopter: return 3;
                default: return 4;
            }
        }

        #endregion

    }
}
