using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataVisualization.Plotter
{
    public class TimeSeriesGraph
    {
        public List<PlotPoint> PlotPoints { get; set; }

        public List<string> TimePoints { get; set; }

        public float XMax { get; set; }
        public float YMax { get; set; }
        public float ZMax { get; set; }

        public float XMin { get; set; }
        public float YMin { get; set; }
        public float ZMin { get; set; }

        public float XMid { get; set; }
        public float YMid { get; set; }
        public float ZMid { get; set; }

        /*
         * Constructor used when no initial data is provided
         */ 
        public TimeSeriesGraph()
        {
            PlotPoints = new List<PlotPoint>();
        }

        /*
         * Constructor used when initial data is provided
         */ 
        public TimeSeriesGraph(List<PlotPoint> points)
        {
            PlotPoints = points;

            CalculateMaxPoints();
            CalculateMinPoints();
            CalculateMidPoints();
        }

        public void AddPlotPoint(PlotPoint point)
        {
            PlotPoints.Add(point);

            CalculateMaxPoints();
            CalculateMinPoints();
            CalculateMidPoints();
        }

        public void AddTimePoints(List<string> timePoints)
        {
            TimePoints = timePoints;
        }

        private void CalculateMaxPoints()
        {
            float xmax, ymax, zmax;

            xmax = PlotPoints[0].XMax;
            ymax = PlotPoints[0].YMax;
            zmax = PlotPoints[0].ZMax;

            foreach (var point in PlotPoints)
            {
                float current_x = point.XMax;
                float current_y = point.YMax;
                float current_z = point.ZMax;

                if (current_x > xmax) xmax = current_x;
                if (current_y > ymax) ymax = current_y;
                if (current_y > zmax) zmax = current_z;
            }

            XMax = xmax;
            YMax = ymax;
            ZMax = zmax;
        }

        private void CalculateMinPoints()
        {
            float xmin, ymin, zmin;

            xmin = PlotPoints[0].XMin;
            ymin = PlotPoints[0].YMin;
            zmin = PlotPoints[0].ZMin;

            foreach (var point in PlotPoints)
            {
                float current_x = point.XMin;
                float current_y = point.YMin;
                float current_z = point.ZMin;

                if (current_x < xmin) xmin = current_x;
                if (current_y < ymin) ymin = current_y;
                if (current_z < zmin) zmin = current_z;
            }

            XMin = xmin;
            YMin = ymin;
            ZMin = zmin;
        }

        private void CalculateMidPoints()
        {
            XMid = Util.FindMiddle(XMax, XMin);
            YMid = Util.FindMiddle(YMax, YMin);
            ZMid = Util.FindMiddle(ZMax, ZMin);
        }

        public bool isTimeGraph()
        {
            return TimePoints != null;
        }

        // For debug

        public string LogMax()
        {
            return "(XMax, YMax, ZMax): " + XMax + "," + YMax + "," + ZMax;
        }

        public string LogMin()
        {
            return "(XMin, YMin, ZMin): " + XMin + "," + YMin + "," + ZMin;
        }

        public string LogMid()
        {
            return "(XMid, YMid, ZMid): " + XMid + "," + YMid + "," + ZMid;
        }
    }
}