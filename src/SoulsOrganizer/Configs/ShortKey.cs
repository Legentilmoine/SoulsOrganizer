using System;
using System.Windows.Input;
using YamlDotNet.Serialization;

namespace SoulsOrganizer.Configs
{
    public class ShortKey
    {
        public ShortKeyCommand Command { get; set; }
        public Key Key { get; set; }
        public ModifierKeys Modifier { get; set; }

        [YamlIgnore]
        public ModifierKeys Modifier1
        { 
            get
            {
                var parts = Modifier.ToString().Split(',');
                if (parts.Length == 1)
                    return Modifier;
                return (ModifierKeys)Enum.Parse(typeof(ModifierKeys), parts[0]);
            }
            set
            {
                Modifier = Modifier2 | value;
            }
        }

        [YamlIgnore]
        public ModifierKeys Modifier2
        {
            get
            {
                var parts = Modifier.ToString().Split(',');
                if (parts.Length == 1)
                    return ModifierKeys.None;
                return (ModifierKeys)Enum.Parse(typeof(ModifierKeys), parts[1]);
            }
            set
            {
                Modifier = Modifier1 | value;
            }
        }
    }

}
