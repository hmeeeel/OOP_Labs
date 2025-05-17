using OOP.Core.Interfaces;
using OOP.Services.SerAndDeser;
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
                    MessageBox.Show($"Файл плагина не найден: {pluginPath}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            MessageBox.Show($"Плагин с именем '{pluginName}' уже загружен", "Дубликат плагина", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }

                        loadedPlugins.Add(pluginName, plugin);
                        pluginFound = true;

                        RegisterPluginSerializers(pluginAssembly, plugin.ShapeType);
                    }
                }

                if (!pluginFound)
                {
                    MessageBox.Show("Не найдено допустимых плагинов.", "Плагин не найден", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке плагина: {ex.Message}","Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void RegisterPluginSerializers(Assembly pluginAssembly, Type shapeType)
        {
            foreach (Type type in pluginAssembly.GetExportedTypes())
            {
                if (typeof(ISerializer).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var serializer = (ISerializer)Activator.CreateInstance(type);
                    ShapeSerializer.RegisterPluginSerializer(serializer);
                }

                if (typeof(IDeserializer).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var deserializer = (IDeserializer)Activator.CreateInstance(type);
                    ShapeDeserializer.RegisterPluginDeserializer(deserializer);
                }
            }
        }
    }
}