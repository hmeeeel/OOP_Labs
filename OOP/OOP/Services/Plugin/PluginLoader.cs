using OOP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OOP.Services.Plugin
{
    public class PluginLoader
    {
        private Dictionary<string, IPlugin> loadedPlugins;

        public PluginLoader()
        {
            loadedPlugins = new Dictionary<string, IPlugin>();
        }

        public IReadOnlyDictionary<string, IPlugin> LoadedPlugins => loadedPlugins;

        public bool LoadPlugin(string pluginPath)
        {
            try
            {
                if (!File.Exists(pluginPath))
                {
                    MessageBox.Show($"Не найден: {pluginPath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                Assembly pluginAssembly = Assembly.LoadFrom(pluginPath);
                bool pluginFound = false;

                foreach (Type type in pluginAssembly.GetExportedTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                        string pluginName = plugin.Name;

                        if (loadedPlugins.ContainsKey(pluginName))
                        {
                            MessageBox.Show($"Имя '{pluginName}' ", "Плагин загружен", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }

                        loadedPlugins.Add(pluginName, plugin);
                        pluginFound = true;

                       // MessageBox.Show($"Plugin '{plugin.Name} v{plugin.Version}' loaded successfully.\n{plugin.Description}",
                          //  "Plugin Loaded", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                if (!pluginFound)
                {
                    MessageBox.Show("В выбранном файле не найдено допустимых плагинов.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
