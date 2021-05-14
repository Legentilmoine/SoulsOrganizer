using System;

namespace SoulsOrganizer.Configs
{
    [Flags]
    public enum ShortKeyCommand
    {
        Load = 1,
        Import,
        Edit,
        Up, 
        Down
    }
}
