using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Point> dataPoints;
        private List<Point> graphPoints;
        private int dotSize = 10;
        private int xScale;
        private int yScale;
        private int xOffset;
        private int yOffset;
        private bool drawDots;
        private bool drawLines;
        private bool drawLabels;

        public MainWindow()
        {
            InitializeComponent();
            dataPoints = new List<Point>();
            graphPoints = new List<Point>();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            xScale = int.Parse(xScaleTextBox.Text);
            yScale = int.Parse(yScaleTextBox.Text);
            xOffset = int.Parse(xOffsetTextBox.Text);
            yOffset = int.Parse(yOffsetTextBox.Text);
            drawDots = drawDotsCheckBox.IsChecked.Value;
            drawLines = drawLinesCheckBox.IsChecked.Value;
            drawLabels = drawLabelsCheckBox.IsChecked.Value;

            StoreData();
            DrawGraph();
        }

        private void Window_SizeChanged(object sender, EventArgs e)
        {
            if (dataPoints.Count > 0)
            {
                DrawGraph();
            }
            
        }

        private void StoreData()
        {
            char[] separators = new char[] { ' ', '\n' };
            string[] dataText = dataTextBox.Text.Split(separators);
            double[] dataArr = Array.ConvertAll(dataText, double.Parse);

            dataPoints.Clear();
            foreach (double data in dataArr)
            {
                Point dataPoint = new Point();
                dataPoint.X = dataPoints.Count + 1;
                dataPoint.Y = data;
                dataPoints.Add(dataPoint);
            }
            
        }

        private void CalculateGraphPoints()
        {
            graphPoints.Clear();
            foreach (Point dataPoint in dataPoints)
            {
                Point graphPoint = ValueToGraphPoint(dataPoint);
                graphPoints.Add(graphPoint);
            }
        }

        private Point ValueToGraphPoint(Point valuePoint)
        {
            double xScalingFactor = graphCanvas.ActualWidth / xScale;
            double yScalingFactor = graphCanvas.ActualHeight / yScale;
            Point graphPoint = new Point();
            graphPoint.X = valuePoint.X * xScalingFactor + xOffset * xScalingFactor;
            graphPoint.Y = graphCanvas.ActualHeight - (valuePoint.Y * yScalingFactor + yOffset * yScalingFactor);
            return graphPoint;
        }

        private Ellipse CreateDot()
        {
            Ellipse dot = new Ellipse();
            SolidColorBrush solidColorBrush = new SolidColorBrush();

            solidColorBrush.Color = Colors.ForestGreen;
            dot.Fill = solidColorBrush;
            dot.StrokeThickness = 1;
            dot.Stroke = Brushes.Black;

            dot.Width = dot.Height = dotSize;

            return dot;
        }
        private void DrawGraph()
        {
            graphCanvas.Children.Clear();
            CalculateGraphPoints();
            for (int i = 0; i < dataPoints.Count; i++)
            {
                Point dataPoint = dataPoints[i];
                Point graphPoint = graphPoints[i];

                if (drawDots)
                {
                    Ellipse dot = CreateDot();
                    Canvas.SetLeft(dot, graphPoint.X - dotSize / 2);
                    Canvas.SetTop(dot, graphPoint.Y - dotSize / 2);
                    graphCanvas.Children.Add(dot);
                }

                if (drawLabels)
                {
                    TextBlock dataLabel = new TextBlock();
                    dataLabel.Text = string.Format("({0},{1})", dataPoint.X, dataPoint.Y);
                    Canvas.SetLeft(dataLabel, graphPoint.X - 10);
                    Canvas.SetTop(dataLabel, graphPoint.Y - (dotSize / 2 + 20));
                    graphCanvas.Children.Add(dataLabel);
                }

                if (drawLines && i > 0)
                {
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = graphPoint.X;
                    line.Y1 = graphPoint.Y;
                    line.X2 = graphPoints[i - 1].X;
                    line.Y2 = graphPoints[i - 1].Y;
                    graphCanvas.Children.Add(line);
                }
            }

            DrawAxis();
        }

        private void DrawAxis()
        {
            Line xAxis = new Line();
            Line yAxis = new Line();

            xAxis.Stroke = yAxis.Stroke = Brushes.Black;
            xAxis.StrokeThickness = yAxis.StrokeThickness = 2;

            xAxis.X1 = 0 - xOffset;
            xAxis.Y1 = 0;
            xAxis.X2 = xScale - xOffset;
            xAxis.Y2 = 0;

            yAxis.X1 = 0;
            yAxis.Y1 = 0 - yOffset;
            yAxis.X2 = 0;
            yAxis.Y2 = yScale - yOffset;

            int xMin = (int)xAxis.X1;
            int xMax = (int)xAxis.X2;

            int yMin = (int)yAxis.Y1;
            int yMax = (int)yAxis.Y2;

            Point xPoint1 = ValueToGraphPoint(new Point(xAxis.X1, xAxis.Y1));
            Point xPoint2 = ValueToGraphPoint(new Point(xAxis.X2, xAxis.Y2));

            Point yPoint1 = ValueToGraphPoint(new Point(yAxis.X1, yAxis.Y1));
            Point yPoint2 = ValueToGraphPoint(new Point(yAxis.X2, yAxis.Y2));

            xAxis.X1 = xPoint1.X;
            xAxis.Y1 = xPoint1.Y;
            xAxis.X2 = xPoint2.X;
            xAxis.Y2 = xPoint2.Y;

            yAxis.X1 = yPoint1.X;
            yAxis.Y1 = yPoint1.Y;
            yAxis.X2 = yPoint2.X;
            yAxis.Y2 = yPoint2.Y;

            graphCanvas.Children.Add(xAxis);
            graphCanvas.Children.Add(yAxis);

            for (int i = xMin; i <= xMax; i++)
            {
                Line xGuide = new Line();
                xGuide.Stroke = Brushes.Black;

                Point xGuidePoint = new Point(i, 0);
                Point xGuideGraphPoint = ValueToGraphPoint(xGuidePoint);

                xGuide.X1 = xGuideGraphPoint.X;
                xGuide.Y1 = xGuideGraphPoint.Y - 5;
                xGuide.X2 = xGuideGraphPoint.X;
                xGuide.Y2 = xGuideGraphPoint.Y + 5;

                graphCanvas.Children.Add(xGuide);
            }

            for (int i = yMin; i <= yMax; i++)
            {
                Line yGuide = new Line();
                yGuide.Stroke = Brushes.Black;

                Point yGuidePoint = new Point(0, i);
                Point yGuideGraphPoint = ValueToGraphPoint(yGuidePoint);

                yGuide.X1 = yGuideGraphPoint.X - 5;
                yGuide.Y1 = yGuideGraphPoint.Y;
                yGuide.X2 = yGuideGraphPoint.X + 5;
                yGuide.Y2 = yGuideGraphPoint.Y;

                graphCanvas.Children.Add(yGuide);
            }
        }
    }
}
