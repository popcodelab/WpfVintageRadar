using System.Windows;
using System.Windows.Media.Animation;
using WpfVintageRadar.ViewModels;

namespace WpfVintageRadar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(CircularScreen.EdgeLength - CircularScreen.EdgeThickness);
        }

        #region Event handlers

        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSettingsButton.Visibility = Visibility.Collapsed;
            CloseSettingsButton.Visibility = Visibility.Visible;
            (Resources["CloseSettingsButtonRotationStoryBoard"] as Storyboard)?.Begin(this);
        }

        private void CloseSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSettingsButton.Visibility = Visibility.Visible;
            CloseSettingsButton.Visibility = Visibility.Visible;
            (Resources["OpenSettingsButtonRotationStoryBoard"] as Storyboard)?.Begin(this);
        }

        #endregion


    }
}
