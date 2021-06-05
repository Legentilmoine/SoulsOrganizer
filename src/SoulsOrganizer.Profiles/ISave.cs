namespace SoulsOrganizer.Profiles
{
    public interface ISave
    {
        string Name { get; set; }

        ISave Copy();
        void Create();
        void Delete();
        void Rename(string newName);
        void Restore();
    }
}