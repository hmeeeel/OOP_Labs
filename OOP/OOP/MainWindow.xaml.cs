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
            new Point(50, 50),
            new Point(200, 50));

        shapes.Add(line);
        RedrawCanvas();
    }

    private void btnDrawRectangle_Click(object sender, RoutedEventArgs e)
    {
        var rectangle = shapeFactory.CreateRectangle(
            Brushes.Blue,
            2,
            new Point(50, 100),
            150,
            100);

        shapes.Add(rectangle);
        RedrawCanvas();
    }

    private void btnDrawEllipse_Click(object sender, RoutedEventArgs e)
    {
        var ellipse = shapeFactory.CreateEllipse(
            Brushes.Red,
            2,
            new Point(250, 100),
            150,
            100);

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
}