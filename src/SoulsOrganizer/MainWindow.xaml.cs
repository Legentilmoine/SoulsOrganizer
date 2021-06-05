using AdonisUI.Controls;
using NHotkey;
using NHotkey.Wpf;
using SoulsOrganizer.Configs;
using SoulsOrganizer.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace SoulsOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public static RoutedCommand MenuItemSelected { get; } = new RoutedCommand("MenuItemSelected", typeof(MainWindow));
        private Dictionary<ShortKeyCommand, EventHandler<HotkeyEventArgs>> _shortKeysFunctions;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            _shortKeysFunctions = new Dictionary<ShortKeyCommand, EventHandler<HotkeyEventArgs>> {
                { ShortKeyCommand.Load, Load },
                { ShortKeyCommand.Import, Import },
                { ShortKeyCommand.Edit, Edit },
                { ShortKeyCommand.Up, Up },
                { ShortKeyCommand.Down, Down }
             };
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vmMain = new VMMain();
            this.DataContext = vmMain;
            vmMain.ReloadConfig += (so, eo) => ReloadConfig();
            ReloadConfig();
        }

        private void ReloadConfig()
        {
            ManageShortKeys(Config.Instance.ShortKeys);
        }

        private void ManageShortKeys(List<ShortKey> shortKeys)
        {
            foreach (var shortKey in shortKeys)
            {
                try
                {
                    HotkeyManager.Current.AddOrReplace($"SoulsOrganizer_{shortKey.Command}", shortKey.Key, shortKey.Modifier, _shortKeysFunctions[shortKey.Command]);
                }
                catch
                {
                    // Send log
                    // Add Trace
                }
            }
        }

        public void Up(object sender, HotkeyEventArgs e)
        {
            ((VMMain)DataContext).UpSave();
            e.Handled = true;
        }

        public void Down(object sender, HotkeyEventArgs e)
        {
            ((VMMain)DataContext).DownSave();
            e.Handled = true;
        }

        public void Load(object sender, HotkeyEventArgs e)
        {
            ((VMMain)DataContext).LoadSave();
            e.Handled = true;
        }

        public void Import(object sender, HotkeyEventArgs e)
        {
            ((VMMain)DataContext).ImportSave();
            e.Handled = true;
        }

        private void Edit(object sender, HotkeyEventArgs e)
        {
            e.Handled = true;
        }

        private void btDuplicate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var element = sender as System.Windows.Controls.Control;
            if (element?.DataContext is ISave)
                ((VMMain)DataContext).DuplicateSave();
            if (element?.DataContext is IProfile)
                ((VMMain)DataContext).DuplicateProfile();
        }

        private void btDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var element = sender as System.Windows.Controls.Control;
            if (element?.DataContext is ISave)
            {
                var model = DeleteSaveModel();
                AdonisUI.Controls.MessageBox.Show(this, model);
                if (model.Result == MessageBoxResult.Yes)
                    ((VMMain)DataContext).DeleteSave();// model.CheckBoxes.FirstOrDefault()?.IsChecked ?? false);
            }
            if (element?.DataContext is IProfile)
            {
                var model = DeleteProfileModel();
                AdonisUI.Controls.MessageBox.Show(this, model);
                if (model.Result == MessageBoxResult.Yes)
                    ((VMMain)DataContext).DeleteProfile(model.CheckBoxes.FirstOrDefault()?.IsChecked ?? false);
            }
        }

        private void btBrowseSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = ((SimpleFileProfile)((VMMain)DataContext).EditProfile).SaveFile;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ((SimpleFileProfile)((VMMain)DataContext).EditProfile).SaveFile = dialog.FileName;
        }

        private void btBrowseLocation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = ((VMMain)DataContext).EditProfile.Location;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ((VMMain)DataContext).EditProfile.Location = dialog.SelectedPath;
        }

        private void btNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((VMMain)DataContext).NewProfile();
        }

        private void btUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((VMMain)DataContext).UpdateProfile();
        }

        private void btImport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((VMMain)DataContext).ImportSave();
        }

        private void btLoad_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((VMMain)DataContext).LoadSave();
        }

        private void btApply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((VMMain)DataContext).UpdateSave();
        }

        private MessageBoxModel DeleteProfileModel()
        {
            return new MessageBoxModel
            {
                Text = "Would you like to delete the profile ?",
                Caption = "Delete Profile ?",
                Icon = MessageBoxImage.Question,
                Buttons = new[] {
                    AdonisUI.Controls.MessageBoxButtons.Yes("Yes"),
                    AdonisUI.Controls.MessageBoxButtons.No("No"),
                },
                CheckBoxes = new[] {
                    new MessageBoxCheckBoxModel("Delete folder and subfolders and saves files")
                    {
                        IsChecked = true,
                        Placement = MessageBoxCheckBoxPlacement.BelowText,
                    },
                },
                IsSoundEnabled = false,
            };
        }

        private MessageBoxModel DeleteSaveModel()
        {
            return new MessageBoxModel
            {
                Text = "Would you like to delete Save ?",
                Caption = "Delete Save ?",
                Icon = MessageBoxImage.Question,
                Buttons = new[] {
                    AdonisUI.Controls.MessageBoxButtons.Yes("Yes"),
                    AdonisUI.Controls.MessageBoxButtons.No("No"),
                },
                //CheckBoxes = new[] {
                //    new MessageBoxCheckBoxModel("Delete folder and save file")
                //    {
                //        IsChecked = true,
                //        Placement = MessageBoxCheckBoxPlacement.BelowText,
                //    },
                //},
                IsSoundEnabled = false,
            };
        }

    }
}
