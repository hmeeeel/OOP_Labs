using Microsoft.Win32;
using OOP.Commands;
using OOP.Core.Interfaces;
using OOP.Services;
using OOP.Services.SerAndDeser;
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

    private List<IDraw> shapes = new List<IDraw>(); 
    private ShapeCreateNew shapeFactory = new ShapeCreateNew();
    private UndoOrRedo commandManager = new UndoOrRedo();
    private UIManager uiManager;
    private MouseHandler mouseHandler;

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

        mouseHandler = new MouseHandler(
           canvas,
           shapes,
           shapeFactory,
           commandManager,
           uiManager,
           cmbPenColor,
           cmbPenWidth,
           cmbFillColor
       );
    }
    private void SetShapeType(string shapeType)
    {
        mouseHandler.SetShapeType(shapeType);
    }

    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        mouseHandler.MouseDown(sender, e);
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        mouseHandler.MouseMove(sender, e);
    }

    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        mouseHandler.MouseUp(sender, e);
    }
    private void ResetDrawingModes()
    {
        mouseHandler.ResetDrawingModes();
    }

    private void btnDrawLine_Click(object sender, RoutedEventArgs e)
    {
        mouseHandler.SetShapeType("Lines");
        ResetDrawingModes();
    }

    private void btnDrawRectangle_Click(object sender, RoutedEventArgs e)
    {
        mouseHandler.SetShapeType("Rectangles");
        ResetDrawingModes();
    }

    private void btnDrawEllipse_Click(object sender, RoutedEventArgs e)
    {
        mouseHandler.SetShapeType("Ellipses");
        ResetDrawingModes();
    }

    private void btnDrawPolyline_Click(object sender, RoutedEventArgs e)
    {
        mouseHandler.SetShapeType("Polylines");
        ResetDrawingModes();
    }
    private void btnDrawPolygon_Click(object sender, RoutedEventArgs e)
    {
        mouseHandler.SetShapeType("Polygons");
        ResetDrawingModes();
    }
    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
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
        uiManager.SaveShapes();
    }

    private void btnLoad_Click(object sender, RoutedEventArgs e)
    {
        uiManager.LoadShapes();
    }

    private void btnAddPlugin_Click(object sender, RoutedEventArgs e)
    {
        uiManager.LoadPlugin();
    }
}