using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphDrawer
{
    class GraphAxis
    {
        public Line xAxis;
        public Line yAxis;

        public GraphAxis()
        {
            xAxis = new Line();
            yAxis = new Line();

            xAxis.Stroke = yAxis.Stroke = Brushes.Red;
        }

        public void SetXAxis(double xMin, double xMax)
        {
            xAxis.X1 = xMin;
            xAxis.X2 = xMax;
            xAxis.Y1 = 0;
            xAxis.Y2 = 0;
        }

        public void SetYAxis(double yMin, double yMax)
        {
            yAxis.X1 = 0;
            yAxis.X2 = 0;
            yAxis.Y1 = yMin;
            yAxis.Y2 = yMax;
        }
    }
}
