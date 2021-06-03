using AdonisUI.ViewModels;
using SoulsOrganizer.Configs;
using SoulsOrganizer.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SoulsOrganizer
{
    public class VMMain : PropertyChangedBase
    {
        public event EventHandler ReloadConfig;
        private ObservableCollection<Profile> _profiles;
        private ObservableCollection<Save> _saves;
        private Profile _selectedProfile;
        private Save _selectedSave;
        private Profile _editProfile;
        private string _editSave;
        private bool _openEditProfile;
        private bool _displaySettings;

        #region "Properties"

        public string Lang
        {
            get
            {
                return Config.Instance.Lang;
            }
            set
            {
                Config.Instance.Lang = value;
            }
        }

        public List<string> Langs
        {
            get
            {
                return new List<string> { "EN" };
            }
        }

        public ObservableCollection<Log> Logs
        {
            get
            {
                return LogManagement.Logs;
            }
        }

        public Log Last
        {
            get
            {
                return LogManagement.Last;
            }
        }

        public ShortKey Load
        {
            get
            {
                return Config.Instance.ShortKeys.FirstOrDefault(x => x.Command == ShortKeyCommand.Load);
            }
        }

        public ShortKey Import
        {
            get
            {
                return Config.Instance.ShortKeys.FirstOrDefault(x => x.Command == ShortKeyCommand.Import);
            }
        }

        public ShortKey Edit
        {
            get
            {
                return Config.Instance.ShortKeys.FirstOrDefault(x => x.Command == ShortKeyCommand.Edit);
            }
        }

        public ShortKey Up
        {
            get
            {
                return Config.Instance.ShortKeys.FirstOrDefault(x => x.Command == ShortKeyCommand.Up);
            }
        }

        public ShortKey Down
        {
            get
            {
                return Config.Instance.ShortKeys.FirstOrDefault(x => x.Command == ShortKeyCommand.Down);
            }
        }

        public List<ModifierKeys> Modifiers
        {
            get { return Enum.GetValues(typeof(ModifierKeys)).Cast<ModifierKeys>().ToList(); }
        }

        public List<Key> Keys
        {
            get { return Enum.GetValues(typeof(Key)).Cast<Key>().ToList(); }
        }

        public bool DisplaySettings
        {
            get { return _displaySettings; }
            set 
            { 
                _displaySettings = value;
                if (!value)
                {
                    ReloadConfig?.Invoke(this, null);
                    UpdateConfigFile();
                }
                NotifyPropertyChanged("DisplaySettings"); 
            }
        }

        public bool OpenEditProfile
        {
            get { return _openEditProfile; }
            set { _openEditProfile = value; NotifyPropertyChanged("OpenEditProfile"); }
        }

        public string EditSave
        {
            get { return _editSave; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException($"'Name' cannot be null or whitespace.", "Name");
                _editSave = value; NotifyPropertyChanged("EditSave");
            }
        }

        public Profile EditProfile
        {
            get { return _editProfile; }
            set { _editProfile = value; NotifyPropertyChanged("EditProfile"); }
        }

        public Profile SelectedProfile
        {
            get { return _selectedProfile; }
            set { _selectedProfile = value; SelectedProfileChanged(); NotifyPropertyChanged("SelectedProfile"); }
        }
         
        public ObservableCollection<Profile> Profiles
        {
            get { return _profiles; }
            set { _profiles = value; NotifyPropertyChanged("Profiles"); }
        }

        public ObservableCollection<Save> Saves
        {
            get { return _saves; }
            set { _saves = value; NotifyPropertyChanged("Saves"); }
        }

        public Save SelectedSave
        {
            get { return _selectedSave; }
            set { _selectedSave = value; SelectedSaveChanged(); NotifyPropertyChanged("SelectedSave"); }
        }

        #endregion
        #region "Constructor"

        public VMMain()
        {
            Profiles = new ObservableCollection<Profile>(Config.Instance.Profiles);
            Saves = new ObservableCollection<Save>();
            EditProfile = CreateNewProfile();
            SelectedProfile = Profiles.FirstOrDefault();
            OpenEditProfile = SelectedProfile == null;
        } 
        #endregion

        #region "Profiles"

        private void SelectedProfileChanged()
        {
            EditProfile = CreateNewProfile(SelectedProfile);
            InitilizeSaves();
        }

        public void NewProfile()
        {
            var newProfile = CreateNewProfile(EditProfile);
            newProfile.Create();
            Profiles.Add(newProfile);
            SortProfiles();
            if (Profiles.Count == 1 && SelectedProfile == null)
                OpenEditProfile = false;
            SelectedProfile = newProfile;
            InitilizeSaves();
            UpdateConfigFile();
            NotifyPropertyChanged("Last");
        }

        private Profile CreateNewProfile(Profile baseProfile = null)
        {
            return new Profile(
                baseProfile?.Name ?? "Name",
                baseProfile?.SaveFile ?? "Save File",
                baseProfile?.Location ?? "Location"
            );
        }

        public void UpdateProfile()
        {
            SelectedProfile.Rename(EditProfile.Name);
            SelectedProfile.SaveFile = EditProfile.SaveFile;
            UpdateConfigFile();
            NotifyPropertyChanged("Last");
        }

        public void DuplicateProfile()
        {
            if (SelectedProfile == null)
                return;
            var profile = SelectedProfile.Copy();
            if (profile != null)
            {
                Profiles.Add(profile);
                SortProfiles();
                SelectedProfile = profile;
                UpdateConfigFile();
            }
            NotifyPropertyChanged("Last");
        }

        public void DeleteProfile(bool deleteFolder = true)
        {
            if (SelectedProfile == null)
                return;
            if (deleteFolder)
                SelectedProfile.Delete();
            Profiles.Remove(SelectedProfile);
            SelectedProfile = Profiles.FirstOrDefault();
            UpdateConfigFile();
            NotifyPropertyChanged("Last");
        }

        private void UpdateConfigFile()
        {
            Config.Instance.Profiles = Profiles.ToList();
            Config.Write();
        }

        public void SortProfiles()
        {
            Profiles.Sort((a, b) => string.Compare(a?.Name ?? "", b?.Name ?? ""));
        }

        #endregion
        #region "Saves"

        private void SelectedSaveChanged()
        {
            EditSave = SelectedSave != null ? SelectedSave.Name : "Save";
            NotifyPropertyChanged("Last");
        }

        public void InitilizeSaves()
        {
            Saves = new ObservableCollection<Save>(SelectedProfile != null ? SelectedProfile.EnumerateSaves() : new List<Save>());
            SortSaves();
        }

        public void DeleteSave(bool deleteFolder = true)
        {
            if (SelectedSave == null)
                return;
            if (deleteFolder)
                SelectedSave.Delete();
            Saves.Remove(SelectedSave);
            SelectedSave = Saves.FirstOrDefault();
            NotifyPropertyChanged("Last");
        }

        public void LoadSave()
        {
            if (SelectedSave == null)
                return;
            SelectedSave.Restore();
            NotifyPropertyChanged("Last");
        }

        public void ImportSave()
        {
            if (SelectedProfile == null)
                return;
            var save = new Save(SelectedProfile);
            save.Create();
            Saves.Add(save);
            SortSaves();
            SelectedSave = save;
            NotifyPropertyChanged("Last");
        }

        public void DuplicateSave()
        {
            if (SelectedProfile == null || SelectedSave == null)
                return;
            var save = SelectedSave.Copy();
            if (save != null)
            {
                Saves.Add(save);
                SortSaves();
                SelectedSave = save;
            }
            NotifyPropertyChanged("Last");
        }

        public void UpdateSave()
        {
            var selected = SelectedSave;
            SelectedSave.Rename(EditSave);
            SortSaves();
            SelectedSave = selected;
            NotifyPropertyChanged("Last");
        }

        public void UpSave()
        {
            var index = SelectedSave != null ? Saves.IndexOf(SelectedSave) : 0;
            index = index + 1 >= Saves.Count ? 0 : index + 1;
            SelectedSave = Saves.ElementAt(index);
        }

        public void DownSave()
        {
            var index = SelectedSave != null ? Saves.IndexOf(SelectedSave) : Saves.Count - 1;
            index = index - 1 < 0 ? index = Saves.Count - 1 : index - 1;
            SelectedSave = Saves.ElementAt(index);
        }

        public void SortSaves()
        {
            Saves.Sort((a, b) => a.Name.CompareTo(b.Name));
        }

        #endregion
    }
}
