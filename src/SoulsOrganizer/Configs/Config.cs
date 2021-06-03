using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace SoulsOrganizer.Configs
{
    public class Config
    {
        public static Config _instance;

        public string Lang { get; set; }
        
        public List<ShortKey> ShortKeys { get; set; }

        public List<Profile> Profiles { get; set; }

        [YamlIgnore]
        public static Config Instance
        {
            get 
            {
                if (_instance == null)
                    _instance = Read();
                if (_instance.Profiles == null)
                    _instance.Profiles = new List<Profile>();
                return _instance;
            }
        }

        public static Config Read()
        {
            var configFile = System.IO.File.ReadAllText("./config.yml");

            var deserializerBuilder = new YamlDotNet.Serialization.DeserializerBuilder();
            deserializerBuilder.WithNamingConvention(YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention.Instance);
            deserializerBuilder.IgnoreFields();
            var des = deserializerBuilder.Build();
            return des.Deserialize<Config>(configFile);
        }

        public static void Write()
        {
            var configFile = System.IO.File.ReadAllText("./config.yml");

            var serializerBuilder = new YamlDotNet.Serialization.SerializerBuilder();
            serializerBuilder.WithNamingConvention(YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention.Instance);
            serializerBuilder.IgnoreFields();
            var ser = serializerBuilder.Build();
            System.IO.File.WriteAllText("./config.yml", ser.Serialize(Instance));
        }
    }
}
