using AdonisUI.ViewModels;
using SoulsOrganizer.Tools;
using System;
using System.Windows;

namespace SoulsOrganizer.Profiles
{
    public class SimpleFileSave : PropertyChangedBase, ISave
    {
        private SimpleFileProfile _profile;
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

        [YamlDotNet.Serialization.YamlIgnore]
        public UIElement UI
        {
            get { return null; }
        }


        public SimpleFileSave(SimpleFileProfile profile, string name = null)
        {
            _profile = profile;
            if (string.IsNullOrEmpty(name))
                name = DateTime.Now.ToString("yy-MM-dd HH-mm-ss");
            _name = name;
        }

        public virtual ISave Copy()
        {
            try
            {
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                var name = $"{Name}_copy";
                var newDossier = System.IO.Path.Combine(_profile.SaveFolder, name);
                DirectoryExtension.Copy(dossier, newDossier);
                LogManagement.AddInfo($"Save {Name} copied.");
                return new SimpleFileSave(_profile, name);
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to copy save.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to copy save.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                LogManagement.AddError("Directory not found: Unable to copy save.");
            }
            return null;
        }

        public virtual void Rename(string newName)
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
                LogManagement.AddInfo($"Save {Name} renamed.");
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to rename save.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to rename save.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                LogManagement.AddError("Directory not found: Unable to rename save.");
            }
        }

        public virtual void Delete()
        {
            try
            {
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                if (System.IO.Directory.Exists(dossier))
                    System.IO.Directory.Delete(dossier, true);
                LogManagement.AddInfo($"Save {Name} deleted.");
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to delete save.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to delete save.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                LogManagement.AddError("Directory not found: Unable to delete save.");
            }
        }

        public virtual void Create()
        {
            try
            {
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                if (!System.IO.Directory.Exists(dossier))
                    System.IO.Directory.CreateDirectory(dossier);
                var fileDestination = System.IO.Path.Combine(dossier, System.IO.Path.GetFileName(_profile.SaveFile));
                System.IO.File.Copy(_profile.SaveFile, fileDestination);
                LogManagement.AddInfo($"Save {Name} created.");
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to create save.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to create save.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                LogManagement.AddError("Directory not found: Unable to create save.");
            }
            catch (System.IO.FileNotFoundException)
            {
                LogManagement.AddError("File not found: Unable to create save.");
            }
        }

        public virtual void Restore()
        {
            try
            {
                var dossier = System.IO.Path.Combine(_profile.SaveFolder, Name);
                var saveFileName = System.IO.Path.GetFileName(_profile.SaveFile);
                var fileToCopy = System.IO.Path.Combine(dossier, System.IO.Path.GetFileName(saveFileName));
                System.IO.File.Copy(fileToCopy, _profile.SaveFile, true);
                LogManagement.AddInfo($"Save {Name} restored.");
            }
            catch (UnauthorizedAccessException)
            {
                LogManagement.AddError("Unauthorized access: Unable to restore save.");
            }
            catch (System.IO.PathTooLongException)
            {
                LogManagement.AddError("Path too long: Unable to restore save.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                LogManagement.AddError("Directory not found: Unable to restore save.");
            }
            catch (System.IO.FileNotFoundException)
            {
                LogManagement.AddError("File not found: Unable to restore save.");
            }
        }

    }

}
