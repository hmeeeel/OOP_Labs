using OOP.Commands;
using OOP.Core.Interfaces;
using OOP.Shape.Factory;
using OOP.Shape.Implementations;
using OOP.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static OOP.Core.Constants.Constants;
using static OOP.Shape.Factory.ShapeCreateNew;
using static OOP.UI.UIManager;

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
    private UIManager uiManager;

    private IDraw currentShape;
    private string currentShapeType = "";
    private bool isDrawing = false;
    private bool isDrawingPoly = false;
    public MainWindow()
    {
        InitializeComponent();

        uiManager = new UIManager(
               canvas,
               shapes,
               shapeButtonsPanel,
               commandManager,
               SetShapeType,
               ResetDrawingModes, 
               btnUndo,       
               btnRedo       
           );
    }
    private void SetShapeType(string shapeType)
    {
        currentShapeType = shapeType;
    }

    // нажатие
    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (string.IsNullOrEmpty(currentShapeType)) return;

        Point point = e.GetPosition(canvas);
        if (isDrawing && currentShape != null)
        {
            isDrawing = currentShape.HandleMouseDown(point, e.ClickCount);
            if (!isDrawing)
            {
                isDrawingPoly = false;
                currentShape = null;
            }
            return;
        }

        isDrawing = true;

        //shapeFactory
        // статичсекие метода - через имя класса!
        currentShape = ShapeCreateNew.CreateShape(currentShapeType,
                UIManager.GetSelectedPenColor(cmbPenColor),
                UIManager.GetSelectedPenWidth(cmbPenWidth),
                point,
                UIManager.GetSelectedFillColor(cmbFillColor));

        if (currentShape != null)
        {
            isDrawing = true;
            isDrawingPoly = currentShape.IsOneClick();

            currentShape.StartDraw(point);
            var command = new AddShape(canvas, currentShape, shapes);// + _undoStack
            commandManager.ExecuteCommand(command);// очистка _redoStack! 
            uiManager.UpdateUndoRedoButton();
        }
    }

    // движение
    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDrawing && currentShape != null)
        {
            Point point = e.GetPosition(canvas);
            currentShape.UpdateDraw(point);
            uiManager.RedrawCanvas();
        }
    }

    // отпускание
    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (isDrawing && currentShape != null && !isDrawingPoly)
        {
            Point point = e.GetPosition(canvas);
            currentShape.EndDraw();
            isDrawing = false;
        }
    }
    private void ResetDrawingModes()
    {
        isDrawing = false;
        isDrawingPoly = false;
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
        uiManager.UpdateUndoRedoButton();
    }
    private void btnUndo_Click(object sender, RoutedEventArgs e)
    {
        commandManager.Undo();
        uiManager.UpdateUndoRedoButton();
    }

    private void btnRedo_Click(object sender, RoutedEventArgs e)
    {
        commandManager.Redo();
        uiManager.UpdateUndoRedoButton();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
    }

    private void btnLoad_Click(object sender, RoutedEventArgs e)
    {
    }

    private void btnAddPlugin_Click(object sender, RoutedEventArgs e)
    {
        uiManager.AddShapeButtons();
    }
}
