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
            Brushes.Black,
            2,
            new Point(25, 20),
            new Point(25, 100)); 

        shapes.Add(line);
        RedrawCanvas();
    }

    private void btnDrawRectangle_Click(object sender, RoutedEventArgs e)
    {
        var rectangle = shapeFactory.CreateRectangle(
            Brushes.Blue,
            2,
            new Point(63, 20),
            98,
            80,
            Brushes.Black);

        shapes.Add(rectangle);
        RedrawCanvas();
    }

    private void btnDrawEllipse_Click(object sender, RoutedEventArgs e)
    {
        var ellipse = shapeFactory.CreateEllipse(
            Brushes.Red,
            2,
            new Point(150, 100),
            100,
            150,
            Brushes.Red);

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
            Brushes.Green,
            2,
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
            Brushes.Purple,
            2,
            points,
            Brushes.Black);

        shapes.Add(polygon);

        var points2 = new List<Point>
            {
                new Point(380, 20),
                new Point(450, 150),
                new Point(500, 300),
                new Point(420, 290)
            };


        var polygon2 = shapeFactory.CreatePolygon(
            Brushes.Purple,
            2,
            points2);  

        shapes.Add(polygon2);
        RedrawCanvas();
    }
}