using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using OOP.Shape.Factory;
using System.Windows.Shapes;
using OOP.Commands;
using System.Windows.Input;
using OOP.Core.Interfaces;
using OOP.Services.Plugin;
using Microsoft.Win32;
using OOP.Services.SerAndDeser;


namespace OOP.UI
{
    public class UIManager

    {
        private Canvas canvas;
        private List<IDraw> shapes;
        private StackPanel shapeButtonsPanel;
        private UndoOrRedo commandManager;
        private Action<string> setShapeTypeCallback;
        private Action resetDrawingModesCallback;
        private Button btnUndo;
        private Button btnRedo;
        private PluginLoader pluginLoader;
        public UIManager(Canvas canvas, List<IDraw> shapes, StackPanel shapeButtonsPanel, UndoOrRedo commandManager, Action<string> setShapeType, Action resetDrawingModes, Button btnUndo, Button btnRedo)
        {
            this.canvas = canvas;
            this.shapes = shapes;
            this.shapeButtonsPanel = shapeButtonsPanel;
            this.commandManager = commandManager;
            this.setShapeTypeCallback = setShapeType;
            this.resetDrawingModesCallback = resetDrawingModes;
            this.btnUndo = btnUndo;
            this.btnRedo = btnRedo;

            this.pluginLoader = new PluginLoader();
            UpdateUndoRedoButton();
        }

        public static Brush GetSelectedPenColor(ComboBox cmbPenColor)
        {
            if (cmbPenColor.SelectedItem == null) return Brushes.Black;

            var selectedItem = cmbPenColor.SelectedItem as ComboBoxItem;
            string colorName = selectedItem.Tag.ToString();
            return GetBrushFromName(colorName);
        }

        public static Brush GetSelectedFillColor(ComboBox cmbFillColor)
        {
            if (cmbFillColor.SelectedItem == null) return Brushes.Transparent;

            var selectedItem = cmbFillColor.SelectedItem as ComboBoxItem;
            string colorName = selectedItem.Tag.ToString();
            return GetBrushFromName(colorName);
        }

        public static int GetSelectedPenWidth(ComboBox cmbPenWidth)
        {
            if (cmbPenWidth.SelectedItem == null) return 1;

            var selectedItem = cmbPenWidth.SelectedItem as ComboBoxItem;
            return int.Parse(selectedItem.Content.ToString());
        }

        public static Brush GetBrushFromName(string colorName)
        {
            if (string.IsNullOrEmpty(colorName)) return Brushes.Black;

            var converter = new System.Windows.Media.BrushConverter();
            if (converter.CanConvertFrom(typeof(string))) return (Brush)converter.ConvertFromString(colorName);
            return Brushes.Black;
        }

        public void AddShapeButtons()
        {
            List<string> availableShapes = ShapeCreateNew.GetAvailableShapes();
            //foreach (string shapeName in availableShapes)
            foreach (var plugin in pluginLoader.LoadedPlugins)
            {
                Button btn = new Button
                {
                    Content = plugin.Value.Name,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    MinWidth = 80
                };

                btn.Click += (sender, e) =>
                {
                    setShapeTypeCallback(plugin.Value.Name);
                    resetDrawingModesCallback();
                };

                shapeButtonsPanel.Children.Add(btn);
            }
        }

        public void RedrawCanvas()
        {
            canvas.Children.Clear();
            foreach (var shape in shapes)
            {
                shape.Draw(canvas);
            }
        }

        public void UpdateUndoRedoButton()
        {
            btnUndo.IsEnabled = commandManager.CanUndo();
            btnRedo.IsEnabled = commandManager.CanRedo();
        }
        public void LoadPlugin()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Plugin Files (*.dll)|*.dll|All files (*.*)|*.*",
                Title = "Select a Plugin"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string pluginPath = openFileDialog.FileName;
                if (pluginLoader.LoadPlugin(pluginPath))
                {
                    foreach (var plugin in pluginLoader.LoadedPlugins)
                    {
                        ShapeCreateNew.RegisterPlugin(plugin.Value);
                    }

                    AddShapeButtons();
                }
            }
        }
        public void SaveShapes()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "JSON файлы (*.json)|*.json",
                    Title = "Сохранить фигуры",
                    DefaultExt = "json"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    ShapeSerializer.SaveToFile(shapes, saveFileDialog.FileName);
                    MessageBox.Show("Успех", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}","Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadShapes()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "JSON файлы (*.json)|*.json",
                    Title = "Загрузить фигуры"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    List<IDraw> previousState = new List<IDraw>(shapes);

                    canvas.Children.Clear();
                    shapes.Clear();

                    List<IDraw> loadedShapes = ShapeDeserializer.LoadFromFile(openFileDialog.FileName, canvas);
                    canvas.Children.Clear();
                    foreach (var shape in loadedShapes)
                    {
                        var addCommand = new AddShape(canvas, shape, shapes);
                        commandManager.ExecuteCommand(addCommand);
                    }

                    UpdateUndoRedoButton();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке фигур: {ex.Message}", "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}