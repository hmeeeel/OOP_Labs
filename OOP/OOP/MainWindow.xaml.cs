using OOP.Commands;
using OOP.Core.Interfaces;
using OOP.Shape;
using OOP.Shape.Implementations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static OOP.Core.Constants.Constants;

namespace OOP;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    private List<IDraw> shapes = new List<IDraw>(); // все фигуры на холсте
    private ShapeCreate shapeFactory = new ShapeCreate(); // создание фигур
    private UndoOrRedo commandManager = new UndoOrRedo();

    private IDraw currentShape;
    private string currentShapeType = "";
    private bool isDrawing = false;
    private bool isDrawingPolylineOrPolygon = false;
    public MainWindow()
    {
        InitializeComponent();
    }

    // ---------------------- МЫШЬ -------------------------------------------------
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
                if (currentShapeType == "Polyline")
                {
                    isDrawingPolylineOrPolygon = false;
                    isDrawing = false;
                    currentShape = null;
                }
                else if (currentShapeType == "Polygon")
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
                if (currentShapeType == "Polyline")
                {
                    (currentShape as Polylines)?.ContinueDrawing(point);
                }
                else if (currentShapeType == "Polygon")
                {
                    (currentShape as Polygons)?.ContinueDrawing(point);
                }
            }
            return;
        }

        isDrawing = true;
        currentShape = shapeFactory.CreateShape(currentShapeType,
            GetSelectedPenColor(),
            GetSelectedPenWidth(),
            point,
            GetSelectedFillColor());

        if (currentShape != null)
        {
            //shapes.Add(currentShape);
            //currentShape.StartDraw(point);
            //RedrawCanvas();

            currentShape.StartDraw(point); 
            var command = new AddShape(canvas, currentShape, shapes);// + _undoStack
            commandManager.ExecuteCommand(command);// очистка _redoStack! 

            if (currentShapeType == "Polyline" || currentShapeType == "Polygon")  isDrawingPolylineOrPolygon = true;
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
        //var line = shapeFactory.CreateLine(
        //    GetSelectedPenColor(),
        //    GetSelectedPenWidth(),
        //    new Point(25, 20),
        //    new Point(25, 100));

        //shapes.Add(line);
        //RedrawCanvas();

        currentShapeType = "Line";
        ResetDrawingModes();
    }

    private void btnDrawRectangle_Click(object sender, RoutedEventArgs e)
    {
        //var rectangle = shapeFactory.CreateRectangle(
        //    GetSelectedPenColor(),
        //    GetSelectedPenWidth(),
        //    new Point(63, 20),
        //    98,
        //    80,
        //    GetSelectedFillColor());

        //shapes.Add(rectangle);
        //RedrawCanvas();

        currentShapeType = "Rectangle";
        ResetDrawingModes();
    }

    private void btnDrawEllipse_Click(object sender, RoutedEventArgs e)
    {
        //var ellipse = shapeFactory.CreateEllipse(
        //    GetSelectedPenColor(),
        //    GetSelectedPenWidth(),
        //    new Point(150, 100),
        //    100,
        //    150,
        //    GetSelectedFillColor());

        //shapes.Add(ellipse);
        //RedrawCanvas();

        currentShapeType = "Ellipse";
        ResetDrawingModes();
    }

    private void btnDrawPolyline_Click(object sender, RoutedEventArgs e)
    {
        //var points = new List<Point>
        //    {
        //        new Point(250, 20),
        //        new Point(240, 40),
        //        new Point(270, 70),
        //        new Point(250, 90),
        //        new Point(290, 120),
        //        new Point(290, 30)
        //    };

        //var polyline = shapeFactory.CreatePolyline(
        //    GetSelectedPenColor(),
        //    GetSelectedPenWidth(),
        //    points
        //    );

        //shapes.Add(polyline);
        //RedrawCanvas();

        currentShapeType = "Polyline";
        ResetDrawingModes();
    }
    private void btnDrawPolygon_Click(object sender, RoutedEventArgs e)
    {
        //var points = new List<Point>
        //    {
        //        new Point(310, 20),
        //        new Point(350, 100),
        //        new Point(400, 250),
        //        new Point(350, 250)
        //    };

        //var polygon = shapeFactory.CreatePolygon(
        //GetSelectedPenColor(),
        //GetSelectedPenWidth(),
        //points,
        //GetSelectedFillColor());

        //shapes.Add(polygon);
        //RedrawCanvas();

        currentShapeType = "Polygon";
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
       // RedrawCanvas();
    }

    private void btnRedo_Click(object sender, RoutedEventArgs e)
    {
        commandManager.Redo();
      //  RedrawCanvas();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
    }

    private void btnLoad_Click(object sender, RoutedEventArgs e)
    {
    }

    private void btnAddPlugin_Click(object sender, RoutedEventArgs e)
    {
    }
}