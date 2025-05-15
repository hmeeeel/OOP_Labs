using Microsoft.Win32;
using OOP.Commands;
using OOP.Core.Interfaces;
using OOP.Services.SerAndDeser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ICommand = OOP.Core.Interfaces.ICommand;


namespace OOP.Services.SerAndDeser
{
    public class SaveShape: ICommand
    {
        private readonly Canvas canvas;
        private readonly List<IDraw> shapes;
        private readonly string filePath;

        public SaveShape(Canvas canvas, List<IDraw> shapes, string filePath)
        {
            this.canvas = canvas;
            this.shapes = shapes;
            this.filePath = filePath;
        }

        public void Execute()
        {
            try
            {
                ShapeSerializer.SaveShapesToFile(shapes, filePath);
                MessageBox.Show($"УСПЕХ в файл {Path.GetFileName(filePath)}", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ОШИБКА: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Undo()
        {}
    }
}