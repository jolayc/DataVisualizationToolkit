using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataVisualization.Plotter
{
    public class DroneAndSineExample : MonoBehaviour
    {
        public TextAsset DroneData;
        public GameObject DronePrefab;

        public TextAsset SineData;
        public GameObject SinePrefab;

        public GameObject TextObject;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Awake()
        {
            DynamicGraph droneGraph = CreateDroneGraph();
            DynamicPlotter dronePlotter = CreateDronePlotter(droneGraph, "Drone Graph");

            DynamicGraph sineGraph = CreateSineGraph();
            DynamicPlotter sinePlotter = CreateSinePlotter(sineGraph, "Sine Graph");
        }

        private DynamicGraph CreateDroneGraph()
        {
            DynamicGraph graph = new DynamicGraph();

            string droneString = DroneData.ToString();
            DataParser parser = new DataParser(droneString);

            List<float> x_values = parser.GetListFromColumn(4); // lat 
            List<float> y_values = parser.GetListFromColumn(2); // alt
            List<float> z_values = parser.GetListFromColumn(3); // long
            List<string> time_values = parser.GetTimePoints(1); // time

            PlotPoint new_point = new PlotPoint(x_values, y_values, z_values);
            graph.AddPlotPoint(new_point);
            graph.AddTimePoints(time_values);

            return graph;
        }

        private DynamicPlotter CreateDronePlotter(DynamicGraph graph, string title)
        {
            GameObject plot = new GameObject();

            DynamicPlotter plotter = plot.AddComponent<DynamicPlotter>();

            plotter.Graph = graph;
            plotter.PointHolder = plot;

            // Set up plotting resources
            plotter.PointPrefab = DronePrefab.transform;

            plotter.Text = TextObject;

            plotter.XAxisName = "Latitude";
            plotter.YAxisName = "Altitude (m)";
            plotter.ZAxisName = "Longitude";

            plotter.PlotTitle = title;

            plotter.Init();

            return plotter;
        }

        private DynamicGraph CreateSineGraph()
        {
            DynamicGraph graph = new DynamicGraph();

            string sineString = SineData.ToString();
            DataParser parser = new DataParser(sineString);

            List<float> x_values = parser.GetListFromColumn(0); // lat 
            List<float> y_values = parser.GetListFromColumn(1); // alt
            List<float> z_values = parser.GetListFromColumn(2); // long

            PlotPoint new_point = new PlotPoint(x_values, y_values, z_values);
            graph.AddPlotPoint(new_point);
            return graph;
        }

        private DynamicPlotter CreateSinePlotter(DynamicGraph graph, string title)
        {
            GameObject plot = new GameObject();

            DynamicPlotter plotter = plot.AddComponent<DynamicPlotter>();

            plotter.Graph = graph;
            plotter.PointHolder = plot;

            // Set up plotting resources
            plotter.PointPrefab = SinePrefab.transform;

            plotter.Text = TextObject;

            plotter.XAxisName = "Radians (rad)";
            plotter.YAxisName = "Amplitude";
            plotter.ZAxisName = "";

            plotter.PlotTitle = title;

            plotter.Init();

            return plotter;
        }
    }
}
