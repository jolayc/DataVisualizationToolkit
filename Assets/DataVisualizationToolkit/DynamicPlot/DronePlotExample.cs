using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataVisualization.Plotter
{
    public class DronePlotExample : MonoBehaviour
    {
        public TextAsset DroneData;
        public GameObject PointPrefab;
        public GameObject TextObject;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void Awake()
        {
            DynamicGraph droneGraph = CreateDroneGraph();
            DynamicPlotter dronePlotter = CreateDronePlotter(droneGraph, "Drone Graph");
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

        private DynamicPlotter CreateDronePlotter(DynamicGraph graph, string name)
        {
            GameObject plot = new GameObject();

            DynamicPlotter plotter = plot.AddComponent<DynamicPlotter>();

            plotter.Graph = graph;
            plotter.PointHolder = plot;

            // Set up plotting resources
            plotter.PointPrefab = PointPrefab.transform;

            plotter.Text = TextObject;

            plotter.XAxisName = "Latitude";
            plotter.YAxisName = "Altitude (m)";
            plotter.ZAxisName = "Longitude";

            plotter.PlotTitle = name;

            plotter.Init();

            return plotter;
        }
    }

}
