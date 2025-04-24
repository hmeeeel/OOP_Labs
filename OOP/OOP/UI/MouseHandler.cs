using OOP.Commands;
using OOP.Core.Interfaces;
using OOP.Shape.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static OOP.UI.UIManager;
using System.Windows;

namespace OOP.UI
{
    public class MouseHandler
    {
        private readonly Canvas canvas;
        private readonly List<IDraw> shapes;
        private readonly ShapeCreateNew shapeFactory;
        private readonly UndoOrRedo commandManager;
        private readonly UIManager uiManager;

        private readonly ComboBox cmbPenColor;
        private readonly ComboBox cmbPenWidth;
        private readonly ComboBox cmbFillColor;

        private IDraw currentShape;
        private string currentShapeType = "";
        private bool isDrawing = false;
        private bool isDrawingPoly = false;

        public MouseHandler(Canvas canvas, List<IDraw> shapes, ShapeCreateNew shapeFactory, UndoOrRedo commandManager, UIManager uiManager, ComboBox cmbPenColor, ComboBox cmbPenWidth, ComboBox cmbFillColor)
        {
            this.canvas = canvas;
            this.shapes = shapes;
            this.shapeFactory = shapeFactory;
            this.commandManager = commandManager;
            this.uiManager = uiManager;
            this.cmbPenColor = cmbPenColor;
            this.cmbPenWidth = cmbPenWidth;
            this.cmbFillColor = cmbFillColor;
        }

        public void SetShapeType(string shapeType)
        {
            currentShapeType = shapeType;
        }

        // нажатие
        public void MouseDown(object sender, MouseButtonEventArgs e)
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
                uiManager.RedrawCanvas();
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
                uiManager.RedrawCanvas();
            }
        }

        // движение
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && currentShape != null)
            {
                Point point = e.GetPosition(canvas);
                currentShape.UpdateDraw(point);
                uiManager.RedrawCanvas();
            }
        }

        // отпускание
        public void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing && currentShape != null && !isDrawingPoly)
            {
                Point point = e.GetPosition(canvas);
                currentShape.EndDraw();
                isDrawing = false;
                uiManager.RedrawCanvas();
            }
        }
        public void ResetDrawingModes()
        {
            isDrawing = false;
            isDrawingPoly = false;
            currentShape = null;
        }
    }
}
