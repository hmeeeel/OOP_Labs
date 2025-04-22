using OOP.Commands;
using OOP.Core.Interfaces;
using OOP.Shape;
using OOP.Shape.Implementations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static OOP.Core.Constants.Constants;
using static OOP.Shape.ShapeCreateNew;
namespace OOP;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    private List<IDraw> shapes = new List<IDraw>(); // все фигуры на холсте
   // private ShapeCreate shapeFactory = new ShapeCreate(); // создание фигур

    private ShapeCreateNew shapeFactory = new ShapeCreateNew();

    //  private UndoOrRedo commandManager = new UndoOrRedo();
    private UndoOrRedoList commandManager = new UndoOrRedoList();

    private IDraw currentShape;
    private string currentShapeType = "";
    private bool isDrawing = false;
    private bool isDrawingPolylineOrPolygon = false;
    public MainWindow()
    {
        InitializeComponent();
    }

    // нажатие
    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (string.IsNullOrEmpty(currentShapeType)) return;

        Point point = e.GetPosition(canvas);
        if (isDrawingPolylineOrPolygon)
        {
            // Если двойной клик и рисуем полилинию или многоугольник, завершаем их
            if (e.ClickCount == DOUBLE_CLICK_COUNT)
            {
                if (currentShapeType == "Polylines")
                {
                    isDrawingPolylineOrPolygon = false;
                    isDrawing = false;
                    currentShape = null;
                }
                else if (currentShapeType == "Polygons")
                {
                    //замык!
                    (currentShape as Polygons)?.ClosePolygon();
                    isDrawingPolylineOrPolygon = false;
                    isDrawing = false;
                    currentShape = null;
                }
            }
            else if (currentShape != null)
            {
                // Продолжаем рисование многоточечных фигур
                if (currentShapeType == "Polylines")
                {
                    (currentShape as Polylines)?.ContinueDrawing(point);
                }
                else if (currentShapeType == "Polygons")
                {
                    (currentShape as Polygons)?.ContinueDrawing(point);
                }
            }
            return;
        }

        isDrawing = true;

        //shapeFactory
        // статичсекие метода - через имя класса!
        currentShape = ShapeCreateNew.CreateShape(currentShapeType,
            GetSelectedPenColor(),
            GetSelectedPenWidth(),
            point,
            GetSelectedFillColor());

        if (currentShape != null)
        {
            currentShape.StartDraw(point);
            var command = new AddShape(canvas, currentShape, shapes);// + _undoStack
            commandManager.ExecuteCommand(command);// очистка _redoStack! 

            if (currentShapeType == "Polylines" || currentShapeType == "Polygons") isDrawingPolylineOrPolygon = true;
        }
    }

    // движение
    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDrawing && currentShape != null)
        {
            Point point = e.GetPosition(canvas);
            currentShape.UpdateDraw(point);
            RedrawCanvas();
        }
    }

    // отпускание
    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (isDrawing && currentShape != null && !isDrawingPolylineOrPolygon)
        {
            Point point = e.GetPosition(canvas);
            currentShape.EndDraw();
            isDrawing = false;
        }
    }
    private void ResetDrawingModes()
    {
        isDrawing = false;
        isDrawingPolylineOrPolygon = false;
        currentShape = null;
    }



    private void btnDrawLine_Click(object sender, RoutedEventArgs e)
    {
        currentShapeType = "Lines";
        ResetDrawingModes();
    }

    private void btnDrawRectangle_Click(object sender, RoutedEventArgs e)
    {
        currentShapeType = "Rectangles";
        ResetDrawingModes();
    }

    private void btnDrawEllipse_Click(object sender, RoutedEventArgs e)
    {
        currentShapeType = "Ellipses";
        ResetDrawingModes();
    }

    private void btnDrawPolyline_Click(object sender, RoutedEventArgs e)
    {
        currentShapeType = "Polylines";
        ResetDrawingModes();
    }
    private void btnDrawPolygon_Click(object sender, RoutedEventArgs e)
    {
        currentShapeType = "Polygons";
        ResetDrawingModes();
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        //shapes.Clear();
        //RedrawCanvas();

        var command = new ClearShape(canvas, shapes);
        commandManager.ExecuteCommand(command);
    }

    private void RedrawCanvas()
    {
        canvas.Children.Clear();
        foreach (var shape in shapes)
        {
            shape.Draw(canvas);
        }
    }



    private Brush GetSelectedPenColor()
    {
        var selectedItem = cmbPenColor.SelectedItem as ComboBoxItem;
        string colorName = selectedItem.Tag.ToString();
        return GetBrushFromName(colorName);
    }

    private Brush GetSelectedFillColor()
    {
        var selectedItem = cmbFillColor.SelectedItem as ComboBoxItem;
        string colorName = selectedItem.Tag.ToString();
        return GetBrushFromName(colorName);
    }

    private int GetSelectedPenWidth()
    {
        var selectedItem = cmbPenWidth.SelectedItem as ComboBoxItem;
        return int.Parse(selectedItem.Content.ToString());
    }

    private Brush GetBrushFromName(string colorName)
    {
        switch (colorName)
        {
            case "Black": return Brushes.Black;
            case "Red": return Brushes.Red;
            case "Blue": return Brushes.Blue;
            case "Green": return Brushes.Green;
            case "Purple": return Brushes.Purple;
            case "Transparent": return Brushes.Transparent;
            default: return Brushes.Black;
        }
    }



    private void btnUndo_Click(object sender, RoutedEventArgs e)
    {
        commandManager.Undo();
    }

    private void btnRedo_Click(object sender, RoutedEventArgs e)
    {
        commandManager.Redo();
    }




    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
    }

    private void btnLoad_Click(object sender, RoutedEventArgs e)
    {
    }

    private void btnAddPlugin_Click(object sender, RoutedEventArgs e)
    {
        AddShapeButtons();
    }


    // проверка кнопки для всех фигур
    private void AddShapeButtons()
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
                currentShapeType = shapeName;
                ResetDrawingModes();
            };

            shapeButtonsPanel.Children.Add(btn);
        }
    }
}
