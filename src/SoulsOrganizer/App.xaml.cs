using SoulsOrganizer.Profiles;
using SoulsOrganizer.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace SoulsOrganizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var plugins = GetPlugins();
            Configs.Config.Instance.Plugins = plugins;
        }

        private Dictionary<string, Type> GetPlugins()
        {
            var dictionnaryPlugins = new Dictionary<string, Type>();
            dictionnaryPlugins.AddOrReplace("Simple File", typeof(SimpleFileProfile));

            if (!System.IO.Directory.Exists("./Plugins"))
                return dictionnaryPlugins;

            foreach (var pluginDll in Directory.EnumerateFiles("./Plugins", "SoulsOrganizer.*.dll"))
            {
                var pluginAssembly = TryLoadFrom(pluginDll);
                if (pluginAssembly == null)
                    continue;

                var pluginsProfiles = pluginAssembly
                    .GetTypes()
                    .Where(t => t.GetInterface(nameof(IProfile)) != null)
                    .ToList();

                foreach (var pluginProfile in pluginsProfiles)
                {
                    // get the type and assure pluginProfile can be created with activator
                    var instance = CreateInstance(pluginProfile);
                    if (instance == null)
                        continue;
                    dictionnaryPlugins.AddOrReplace(instance.Type, pluginProfile);
                }
            }
            return dictionnaryPlugins;
        }

        private IProfile CreateInstance(Type pluginProfile)
        {
            try
            {
                return (IProfile)Activator.CreateInstance(pluginProfile);
            }
            catch { }
            return null;
        }

        private Assembly TryLoadFrom(string pluginDll)
        {
            try
            {
                return Assembly.LoadFrom(pluginDll);
            }
            catch { }
            return null;
        }
    }
}
