using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace DataVisualization.Plotter
{
    public class DynamicPlotter : MonoBehaviour
    {
        // for DynamicPlotter
        [Tooltip("Text prefab")]
        public GameObject Text;

        [Tooltip("Prefab that will contain instantiated prefabs in hierarchy")]
        public GameObject PointHolder;

        [Tooltip("Plot point prefab")]
        public Transform PointPrefab;

        public DynamicGraph Graph;

        private List<Transform> Points;

        // Labels
        [Tooltip("Title of Plot")]
        public string PlotTitle;

        [Tooltip("X-Axis Label")]
        public string XAxisName;

        [Tooltip("Y-Axis Label")]
        public string YAxisName;

        [Tooltip("Z-Axis Label")]
        public string ZAxisName;

        [Tooltip("Changes size scale of plot")]
        public float PlotScale = 10;

        // Graph resources
        public Material HandleMaterial;
        public Material HandleGrabbedMaterial;
        public GameObject RotationHandle;
        public GameObject ScaleHandle;
        private GameObject TimeText;
        private Vector3 GraphRadius;

        // from Graph
        private float GraphXMax, GraphXMin;
        private float GraphYMax, GraphYMin;
        private float GraphZMax, GraphZMin;

        private void Awake()
        {
            // Disable object until Init() is called
            enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                UpdatePoint(Points[i], i);
            }
        }

        public void Init()
        {
            Points = new List<Transform>();

            SetMaxMinMid();

            // Drawing graph components
            DrawPlot();
            DrawTitle();
            DrawXAxisLabel();
            DrawYAxisLabel();
            DrawZAxisLabel();
            if (Graph.isTimeGraph())
            {
                DrawTime();
            }

            // Graph is initialized so enable it so Update() can be called
            enabled = true;
        }

        private void SetMaxMinMid()
        {
            // Grab max, min and mid points of graph
            GraphXMax = Graph.XMax;
            GraphXMin = Graph.XMin;

            GraphYMax = Graph.YMax;
            GraphYMin = Graph.YMin;

            GraphZMax = Graph.ZMax;
            GraphZMin = Graph.ZMin;
        }

        private void DrawPlot()
        {
            int numberOfPoints = Graph.PlotPoints.Count;

            PointHolder.transform.position = Vector3.zero; // Center pivot to origin

            // Configure MRTK components of Graph, e.g. BoundingBox and ManipulationHandler
            PointHolder.AddComponent<BoxCollider>();
            PointHolder.AddComponent<BoundingBox>();
            PointHolder.GetComponent<BoundingBox>().WireframeEdgeRadius = PointHolder.GetComponent<BoundingBox>().WireframeEdgeRadius;
            PointHolder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
            PointHolder.AddComponent<ManipulationHandler>();

            // Adjust plot size
            PointHolder.transform.gameObject.GetComponent<BoxCollider>().size *= PlotScale;

            // Scale handle sizes
            PointHolder.GetComponent<BoundingBox>().ScaleHandleSize = PointHolder.GetComponent<BoundingBox>().ScaleHandleSize;
            PointHolder.GetComponent<BoundingBox>().RotationHandleSize = PointHolder.GetComponent<BoundingBox>().RotationHandleSize;

            //Optional handle prefab Models
            PointHolder.GetComponent<BoundingBox>().HandleGrabbedMaterial = HandleGrabbedMaterial;
            PointHolder.GetComponent<BoundingBox>().HandleMaterial = HandleMaterial;
            PointHolder.GetComponent<BoundingBox>().ScaleHandlePrefab = ScaleHandle;
            PointHolder.GetComponent<BoundingBox>().RotationHandleSlatePrefab = RotationHandle;

            GraphRadius = PointHolder.GetComponent<BoxCollider>().size / 2;

            for (int i = 0; i < numberOfPoints; i++)
            {
                Transform current_point = Instantiate(PointPrefab);
                current_point.SetParent(PointHolder.GetComponent<BoxCollider>().transform);
                current_point.localPosition = PointHolder.GetComponent<BoxCollider>().center;
                current_point.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;

                Points.Add(current_point);
            }
        }

        private void UpdatePoint(Transform point, int index)
        {
            Vector3 updated_position;

            PlotPoint pointFromGraph = Graph.PlotPoints[index];
            int currentIndex = pointFromGraph.currentPointIndex;

            // Get bounds of BoxCollider to help with normalizing plot point
            Vector3 max_range = GraphRadius;
            Vector3 min_range = -GraphRadius;

            if (currentIndex < pointFromGraph.XPoints.Count)
            {
                updated_position.x = Util.NormalizeToRange(min_range.x, max_range.x, pointFromGraph.XPoints[currentIndex], GraphXMax, GraphXMin);
                updated_position.y = Util.NormalizeToRange(min_range.y, max_range.y, pointFromGraph.YPoints[currentIndex], GraphYMax, GraphYMin);
                updated_position.z = Util.NormalizeToRange(min_range.z, max_range.z, pointFromGraph.ZPoints[currentIndex], GraphZMax, GraphZMin);

                point.localPosition = updated_position;

                if (Graph.isTimeGraph())
                {
                    UpdateTime(currentIndex);
                }

                pointFromGraph.currentPointIndex++;
            }
            else
            {
                pointFromGraph.currentPointIndex = 0;
            }
        }

        private void UpdateTime(int index)
        {
            List<string> timePoints = Graph.TimePoints;
            TimeText.GetComponent<TextMesh>().text = "Time: " + timePoints[index];
        }

        private void DrawTitle()
        {
            GameObject plotTitle = Instantiate(Text, Vector3.zero, Quaternion.identity);
            // Add title text
            plotTitle.transform.SetParent(PointHolder.transform);
            plotTitle.GetComponent<TextMesh>().text = PlotTitle;
            plotTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;
            plotTitle.GetComponent<TextMesh>().anchor = TextAnchor.LowerCenter;
            plotTitle.transform.position = new Vector3(0, GraphRadius.y, -GraphRadius.x);
        }

        private void DrawTime()
        {
            TimeText = Instantiate(Text, Vector3.zero, Quaternion.identity);

            // Add time text
            TimeText.transform.SetParent(PointHolder.transform);
            TimeText.GetComponent<TextMesh>().text = "";
            TimeText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;
            TimeText.GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
            TimeText.transform.position = new Vector3(0, -GraphRadius.y, -GraphRadius.z);
        }

        private void DrawXAxisLabel()
        {
            GameObject xlabel;
            xlabel = Instantiate(Text, Vector3.zero, Quaternion.Euler(90, 0, 0));

            // Add label text
            xlabel.transform.SetParent(PointHolder.transform);
            string text;

            if (XAxisName != null)
            {
                text = XAxisName;
            }
            else
            {
                text = "x-axis";
            }
            xlabel.GetComponent<TextMesh>().text = text;
            xlabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;
            xlabel.GetComponent<TextMesh>().anchor = TextAnchor.LowerCenter;
            xlabel.transform.position = new Vector3(0, -GraphRadius.y, -GraphRadius.z);
        }

        private void DrawYAxisLabel()
        {
            GameObject ylabel;
            ylabel = Instantiate(Text, Vector3.zero, Quaternion.Euler(0, 0, 90));

            // Add label text
            ylabel.transform.SetParent(PointHolder.transform);
            string text;

            if (YAxisName != null)
            {
                text = YAxisName;
            }
            else
            {
                text = "y-axis";
            }
            ylabel.GetComponent<TextMesh>().text = text;
            ylabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;
            ylabel.GetComponent<TextMesh>().anchor = TextAnchor.UpperCenter;
            ylabel.transform.position = new Vector3(-GraphRadius.x, 0, GraphRadius.z);
        }

        private void DrawZAxisLabel()
        {
            GameObject zlabel;

            zlabel = Instantiate(Text, Vector3.zero, Quaternion.Euler(90, 90, 0));

            // Add label text
            zlabel.transform.SetParent(PointHolder.transform);
            string text;
            if (ZAxisName != null)
            {
                text = ZAxisName;
            }
            else
            {
                text = "z-axis";
            }
            zlabel.GetComponent<TextMesh>().text = text;
            zlabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;
            zlabel.GetComponent<TextMesh>().anchor = TextAnchor.LowerCenter;
            zlabel.transform.position = new Vector3(-GraphRadius.x, -GraphRadius.y, 0);
        }
    }
}
