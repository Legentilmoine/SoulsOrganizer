using System.Windows.Controls;

namespace SoulsOrganizer.Profiles
{
    /// <summary>
    /// Interaction logic for UIProfile.xaml
    /// </summary>
    public partial class UIProfile : UserControl
    {
        public UIProfile()
        {
            InitializeComponent();
        }

        private void btBrowseSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.FileName = ((SimpleFileProfile)DataContext).SaveFile;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ((SimpleFileProfile)DataContext).SaveFile = dialog.FileName;
        }

    }
}
