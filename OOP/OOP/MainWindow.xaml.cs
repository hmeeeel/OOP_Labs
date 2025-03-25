using Microsoft.Win32;
using OOP.Interfaces;
using OOP.Shape;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace OOP;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    private List<IDraw> shapes = new List<IDraw>();
    private ShapeCreate shapeFactory = new ShapeCreate();


    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnDrawLine_Click(object sender, RoutedEventArgs e)
    {
        var line = shapeFactory.CreateLine(
            GetSelectedPenColor(),
            GetSelectedPenWidth(),
            new Point(25, 20),
            new Point(25, 100)); 

        shapes.Add(line);
        RedrawCanvas();
    }

    private void btnDrawRectangle_Click(object sender, RoutedEventArgs e)
    {
        var rectangle = shapeFactory.CreateRectangle(
            GetSelectedPenColor(),
            GetSelectedPenWidth(),
            new Point(63, 20),
            98,
            80,
            GetSelectedFillColor());

        shapes.Add(rectangle);
        RedrawCanvas();
    }

    private void btnDrawEllipse_Click(object sender, RoutedEventArgs e)
    {
        var ellipse = shapeFactory.CreateEllipse(
            GetSelectedPenColor(),
            GetSelectedPenWidth(),
            new Point(150, 100),
            100,
            150,
            GetSelectedFillColor());

        shapes.Add(ellipse);
        RedrawCanvas();
    }
    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        shapes.Clear();
        RedrawCanvas();
    }

    private void RedrawCanvas()
    {
        canvas.Children.Clear();
        foreach (var shape in shapes)
        {
            shape.Draw(canvas);
        }
    }

    private void btnDrawPolyline_Click(object sender, RoutedEventArgs e)
    {
        var points = new List<Point>
            {
                new Point(250, 20),
                new Point(240, 40),
                new Point(270, 70),
                new Point(250, 90),
                new Point(290, 120),
                new Point(290, 30)
            };

        var polyline = shapeFactory.CreatePolyline(
            GetSelectedPenColor(),
            GetSelectedPenWidth(),
            points
            );

        shapes.Add(polyline);
        RedrawCanvas();
    }
    private void btnDrawPolygon_Click(object sender, RoutedEventArgs e)
    {
        var points = new List<Point>
            {
                new Point(310, 20),
                new Point(350, 100),
                new Point(400, 250),
                new Point(350, 250)
            };

        var polygon = shapeFactory.CreatePolygon(
        GetSelectedPenColor(),
        GetSelectedPenWidth(),
        points,
        GetSelectedFillColor());

        shapes.Add(polygon);

        //var points2 = new List<Point>
        //    {
        //        new Point(380, 20),
        //        new Point(450, 150),
        //        new Point(500, 300),
        //        new Point(420, 290)
        //    };


        //var polygon2 = shapeFactory.CreatePolygon(
        //    Brushes.Purple,
        //    2,
        //    points2);  

        //shapes.Add(polygon2);
        RedrawCanvas();
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
    }

    private void btnRedo_Click(object sender, RoutedEventArgs e)
    {
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