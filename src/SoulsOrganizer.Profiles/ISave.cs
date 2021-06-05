using System.Windows;

namespace SoulsOrganizer.Profiles
{
    public interface ISave
    {
        string Name { get; set; }
        [YamlDotNet.Serialization.YamlIgnore]
        UIElement UI { get; }

        ISave Copy();
        void Create();
        void Delete();
        void Rename(string newName);
        void Restore();
    }
}