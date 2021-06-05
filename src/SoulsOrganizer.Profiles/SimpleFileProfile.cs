using AdonisUI.ViewModels;
using SoulsOrganizer.Tools;
using System;
using System.Collections.Generic;

namespace SoulsOrganizer.Profiles
{
    public class SimpleFileProfile : PropertyChangedBase, IProfile
    {
        private string _save;
        private string _location;
        private string _name;

        #region "Properties"

        public virtual string Type => "SimpleFile";

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException($"'{nameof(Name)}' cannot be null or whitespace.", nameof(Name));

                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string SaveFile
        {
            get { return _save; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException($"'{nameof(SaveFile)}' cannot be null or whitespace.", nameof(SaveFile));
                _save = value;
                NotifyPropertyChanged("SaveFile");
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException($"'{nameof(Location)}' cannot be null or whitespace.", nameof(Location));
                _location = value;
                NotifyPropertyChanged("Location");
            }
        }

        [YamlDotNet.Serialization.YamlIgnore]
        public string SaveFolder { get { return System.IO.Path.Combine(Location, Name); } }

        #endregion
        #region "Constructor"

        public SimpleFileProfile()
        {
            Name = "Name";
            SaveFile = "Save File";
            Location = "Location";
        }

        public SimpleFileProfile(string name, string saveFile, string location)
        {
            if (string.IsNullOrWhiteSpace(saveFile))
                throw new ArgumentException($"'{nameof(saveFile)}' cannot be null or whitespace.", nameof(saveFile));

            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException($"'{nameof(location)}' cannot be null or whitespace.", nameof(location));

            Name = name;
            SaveFile = saveFile;
            Location = location;
        }

        #endregion

        #region "Functions"

        public virtual IEnumerable<ISave> EnumerateSaves()
        {
            var saveFileName = System.IO.Path.GetFileName(SaveFile);
            foreach (var file in System.IO.Directory.EnumerateFiles(SaveFolder, saveFileName, System.IO.SearchOption.AllDirectories))
            {
                var saveName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(file));
                yield return CreateSave(saveName);
            }
        }

        public virtual void Clone(IProfile profileToClone)
        {
            Rename(profileToClone.Name);
            Move(profileToClone.Location);
            if (profileToClone is SimpleFileProfile)
                SaveFile = ((SimpleFileProfile)profileToClone).SaveFile;

        }

        public virtual ISave CreateSave(string saveName = null)
        {
            return new SimpleFileSave(this, saveName);
        }

        public virtual bool Rename(string newName)
        {
            if (string.Equals(Name, newName))
                return true;

            try
            {
                var oldName = Name;
                Name = newName;
                var oldFolder = System.IO.Path.Combine(Location, oldName);
                if (Location == null || !System.IO.Directory.Exists(oldFolder))
                    return true;
                var newFolder = System.IO.Path.Combine(Location, newName);
                System.IO.Directory.Move(oldFolder, newFolder);
                LogManagement.AddInfo($"Profile {Name} renamed.");
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to rename profile.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to rename profile.");
            }
            return false;
        }

        public virtual bool Move(string newLocation)
        {
            if (string.Equals(Location, newLocation))
                return true;

            try
            {
                var oldLocation = Location;
                Location = newLocation;
                if (oldLocation == null || oldLocation.Equals("Location") || !System.IO.Directory.Exists(SaveFolder))
                    return true;
                var oldFolder = System.IO.Path.Combine(oldLocation, Name);
                var newFolder = System.IO.Path.Combine(newLocation, Name);
                System.IO.Directory.Move(oldFolder, newFolder);
                LogManagement.AddInfo($"Profile {Name} moved.");
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to move profile.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to move profile.");
            }
            return false;
        }

        public virtual IProfile Copy()
        {
            try
            {
                var name = $"{Name}_copy";
                var newDossier = System.IO.Path.Combine(Location, name);
                DirectoryExtension.Copy(SaveFolder, newDossier);
                LogManagement.AddInfo($"Profile {Name} copied.");
                return new SimpleFileProfile(name, SaveFile, Location);
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to copy profile.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to copy profile.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                LogManagement.AddError("Directory not found: Unable to copy profile.");
            }
            return null;
        }

        public virtual void Delete()
        {
            try
            {
                if (System.IO.Directory.Exists(SaveFolder))
                    System.IO.Directory.Delete(SaveFolder, true);
                LogManagement.AddInfo($"Profile {Name} deleted.");
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to delete profile.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to delete profile.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                LogManagement.AddError("Directory not found: Unable to delete profile.");
            }
        }

        public virtual void Create()
        {
            try
            {
                if (!System.IO.Directory.Exists(SaveFolder))
                    System.IO.Directory.CreateDirectory(SaveFolder);
                LogManagement.AddInfo($"Profile {Name} created.");
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to create profile.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to create profile.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                LogManagement.AddError("Directory not found: Unable to create profile.");
            }
        }

        #endregion

    }

}
