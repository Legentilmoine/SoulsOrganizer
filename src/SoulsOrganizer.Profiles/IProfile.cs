using System.Collections.Generic;

namespace SoulsOrganizer.Profiles
{
    public interface IProfile
    {
        string Name { get; }
        string Type { get; }
        string Location { get; set; }


        IProfile Copy();
        ISave CreateSave(string saveName = null);
        void Create();
        void Delete();
        bool Rename(string newName);
        bool Move(string newLocation);
        void Clone(IProfile profileToClone);
        IEnumerable<ISave> EnumerateSaves();
    }
}