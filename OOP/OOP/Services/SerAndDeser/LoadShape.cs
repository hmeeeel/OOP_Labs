using OOP.Commands;
using OOP.Core.Interfaces;
using System.Windows.Controls;
using System.Windows;
using System.IO;

namespace OOP.Services.SerAndDeser
{
    public class LoadShape : ICommand
    {
        private readonly Canvas canvas;
        private readonly List<IDraw> shapes;
        private readonly string filePath;
        private readonly UndoOrRedo commandManager;
        private CompositeCommand loaded;
        private List<IDraw> prev;

        public LoadShape(Canvas canvas, List<IDraw> shapes, string filePath, UndoOrRedo commandManager)
        {
            this.canvas = canvas;
            this.shapes = shapes;
            this.filePath = filePath;
            this.commandManager = commandManager;
            this.prev = new List<IDraw>(shapes);
        }

        public void Execute()
        {
            try
            {
                prev = new List<IDraw>(shapes);
                canvas.Children.Clear();
                shapes.Clear();


                List<IDraw> loadedShapes = ShapeDeserializer.LoadFromFile(filePath, canvas);
                loaded = new CompositeCommand();

                canvas.Children.Clear();

                foreach (var shape in loadedShapes)
                {
                    var addCommand = new AddShape(canvas, shape, shapes);
                    loaded.AddCommand(addCommand);
                }

                loaded.Execute();
                MessageBox.Show($"Загружено {loadedShapes.Count} фигур из файла {Path.GetFileName(filePath)}", "Загрузка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ОШИБКА: {ex.Message}","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Undo()
        {
            if (loaded != null)
            {
                loaded.Undo();
            }

            canvas.Children.Clear();
            shapes.Clear();

            foreach (var shape in prev)
            {
                shape.Draw(canvas);
                shapes.Add(shape);
            }
        }
    }
}