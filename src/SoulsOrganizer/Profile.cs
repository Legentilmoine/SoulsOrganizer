using AdonisUI.ViewModels;
using SoulsOrganizer.Tools;
using System;
using System.Collections.Generic;

namespace SoulsOrganizer
{
    public class Profile : PropertyChangedBase
    {
        private ProfileType _type;
        private string _save;
        private string _location;
        private string _name;

        #region "Properties"

        public ProfileType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }

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

        public Profile()
        {
        }

        public Profile(string name, string saveFile, string location)
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

        public IEnumerable<Save> EnumerateSaves()
        {
            var saveFileName = System.IO.Path.GetFileName(SaveFile);
            foreach (var file in System.IO.Directory.EnumerateFiles(SaveFolder, saveFileName, System.IO.SearchOption.AllDirectories))
            {
                var saveName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(file));
                yield return new Save(this, saveName);
            }
        }

        public bool Rename(string newName)
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
                return true;
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (System.IO.PathTooLongException)
            {
            }
            return false;
        }

        public bool Move(string newLocation)
        {
            if (string.Equals(Location, newLocation))
                return true;

            try
            {
                var oldLocation = Location;
                Location = newLocation;
                if (oldLocation == null || !System.IO.Directory.Exists(SaveFolder))
                    return true;
                var oldFolder = System.IO.Path.Combine(oldLocation, Name);
                var newFolder = System.IO.Path.Combine(newLocation, Name);
                System.IO.Directory.Move(oldFolder, newFolder);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (System.IO.PathTooLongException)
            {
            }
            return false;
        }

        public Profile Copy()
        {
            try
            {
                var name = $"{Name}_copy";
                var newDossier = System.IO.Path.Combine(Location, name);
                DirectoryExtension.Copy(SaveFolder, newDossier);
                return new Profile(name, SaveFile, Location);
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (System.IO.PathTooLongException)
            {
            }
            catch (System.IO.DirectoryNotFoundException)
            {
            }
            return null;
        }

        public void Delete()
        {
            try
            {
                if (System.IO.Directory.Exists(SaveFolder))
                    System.IO.Directory.Delete(SaveFolder, true);
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (System.IO.PathTooLongException)
            {
            }
            catch (System.IO.DirectoryNotFoundException)
            {
            }
        }

        public void Create()
        {
            try
            {
                if (!System.IO.Directory.Exists(SaveFolder))
                    System.IO.Directory.CreateDirectory(SaveFolder);
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (System.IO.PathTooLongException)
            {
            }
            catch (System.IO.DirectoryNotFoundException)
            {
            }
        }

        #endregion

    }

    public enum ProfileType
    {
        Custom
    }

}
