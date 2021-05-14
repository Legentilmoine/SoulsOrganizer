using AdonisUI.ViewModels;
using SoulsOrganizer.Tools;
using System;

namespace SoulsOrganizer
{
    public class Save : PropertyChangedBase
    {
        private Profile _profile;
        private string _name;

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

        public Save(Profile profile, string name = null)
        {
            _profile = profile;
            if (string.IsNullOrEmpty(name))
                name = DateTime.Now.ToString("yy-MM-dd HH-mm-ss");
            _name = name;
        }

        public Save Copy()
        {
            try
            {
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                var name = $"{Name}_copy";
                var newDossier = System.IO.Path.Combine(_profile.SaveFolder, name);
                DirectoryExtension.Copy(dossier, newDossier);
                return new Save(_profile, name);
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

        public void Rename(string newName)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(newName))
                return;

            try
            {
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                var newDossier = System.IO.Path.Combine(_profile.SaveFolder, newName);
                if (System.IO.Directory.Exists(dossier))
                    System.IO.Directory.Move(dossier, newDossier);
                Name = newName;
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

        public void Delete()
        {
            try
            {
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                if (System.IO.Directory.Exists(dossier))
                    System.IO.Directory.Delete(dossier, true);
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
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                if (!System.IO.Directory.Exists(dossier))
                    System.IO.Directory.CreateDirectory(dossier);
                var fileDestination = System.IO.Path.Combine(dossier, System.IO.Path.GetFileName(_profile.SaveFile));
                System.IO.File.Copy(_profile.SaveFile, fileDestination);
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
            catch (System.IO.FileNotFoundException)
            {
            }
        }

        public void Restore()
        {
            try
            {
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                var saveFileName = System.IO.Path.GetFileName(_profile.SaveFile);
                var fileToCopy = System.IO.Path.Combine(dossier, System.IO.Path.GetFileName(saveFileName));
                System.IO.File.Copy(fileToCopy, _profile.SaveFile, true);
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
            catch (System.IO.FileNotFoundException)
            {
            }
        }

    }

}
