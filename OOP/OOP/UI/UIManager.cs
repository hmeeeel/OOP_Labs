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


namespace OOP.UI
{
    public class UIManager

    {
        private Canvas canvas;
        private List<IDraw> shapes;
        private StackPanel shapeButtonsPanel;
        private UndoOrRedoList commandManager;
        private Action<string> setShapeTypeCallback;
        private Action resetDrawingModesCallback;
        private Button btnUndo;
        private Button btnRedo;

        public UIManager(Canvas canvas, List<IDraw> shapes, StackPanel shapeButtonsPanel, UndoOrRedoList commandManager, Action<string> setShapeType, Action resetDrawingModes, Button btnUndo, Button btnRedo)
        {
            this.canvas = canvas;
            this.shapes = shapes;
            this.shapeButtonsPanel = shapeButtonsPanel;
            this.commandManager = commandManager;
            this.setShapeTypeCallback = setShapeType;
            this.resetDrawingModesCallback = resetDrawingModes;
            this.btnUndo = btnUndo;
            this.btnRedo = btnRedo;

            UpdateUndoRedoButton();
        }

        public static Brush GetSelectedPenColor(ComboBox cmbPenColor)
        {
            var selectedItem = cmbPenColor.SelectedItem as ComboBoxItem;
            string colorName = selectedItem.Tag.ToString();
            return GetBrushFromName(colorName);
        }

        public static Brush GetSelectedFillColor(ComboBox cmbFillColor)
        {
            var selectedItem = cmbFillColor.SelectedItem as ComboBoxItem;
            string colorName = selectedItem.Tag.ToString();
            return GetBrushFromName(colorName);
        }

        public static int GetSelectedPenWidth(ComboBox cmbPenWidth)
        {
            var selectedItem = cmbPenWidth.SelectedItem as ComboBoxItem;
            return int.Parse(selectedItem.Content.ToString());
        }

        public static Brush GetBrushFromName(string colorName)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (converter.CanConvertFrom(typeof(string))) return (Brush)converter.ConvertFromString(colorName);
            else return Brushes.Black;
        }

        public void AddShapeButtons()
        {
            List<string> availableShapes = ShapeCreateNew.GetAvailableShapes();
            foreach (string shapeName in availableShapes)
            {
                Button btn = new Button
                {
                    Content = shapeName,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    MinWidth = 80
                };

                btn.Click += (sender, e) =>
                {
                    setShapeTypeCallback(shapeName); 
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
    }
}

