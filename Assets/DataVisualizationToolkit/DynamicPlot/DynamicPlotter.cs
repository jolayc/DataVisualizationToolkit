using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace DataVisualization.Plotter
{
    public class DynamicPlotter : MonoBehaviour
    {
        // for DynamicPlotter
        public GameObject Text;
        public GameObject PointHolder;

        public Transform PointPrefab;

        public TimeSeriesGraph Graph;

        private List<Transform> Points;

        // Labels
        public string PlotTitle;
        public string XAxisName, YAxisName, ZAxisName;

        // Graph resources
        public Material HandleMaterial;
        public Material HandleGrabbedMaterial;
        public GameObject RotationHandle;
        public GameObject ScaleHandle;
        public float PlotScale;
        private GameObject TimeText;
        private Vector3 GraphRadius;

        // from Graph
        private float GraphXMax, GraphXMid, GraphXMin;
        private float GraphYMax, GraphYMid, GraphYMin;
        private float GraphZMax, GraphZMid, GraphZMin;

        //private int PlotScale = 10;

        // Start is called before the first frame update
        void Start()
        {
            // Lock scene rendering to 60 fps
            //Application.targetFrameRate = 60;
        }

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
            PlotScale = 1;
            Points = new List<Transform>();

            SetMaxMinMid();

            // Drawing graph components
            DrawPlot();
            DrawTitle();
            DrawXAxisLabel();
            DrawYAxisLabel();
            DrawZAxisLabel();
            DrawTime();

            // Graph is initialized so enable it so Update() can be called
            enabled = true;
        }

        private void SetMaxMinMid()
        {
            // Grab max, min and mid points of graph
            GraphXMax = Graph.XMax;
            GraphXMid = Graph.XMid;
            GraphXMin = Graph.XMin;

            GraphYMax = Graph.YMax;
            GraphYMid = Graph.YMid;
            GraphYMin = Graph.YMin;

            GraphZMax = Graph.ZMax;
            GraphZMid = Graph.ZMid;
            GraphZMin = Graph.ZMin;
        }

        private void DrawPlot()
        {
            int numberOfPoints = Graph.PlotPoints.Count;

            PointHolder.transform.position = Vector3.zero; // Center pivot to origin

            // Configure MRTK components of Graph, e.g. BoundingBox and ManipulationHandler
            PointHolder.AddComponent<BoxCollider>();
            PointHolder.AddComponent<BoundingBox>();
            PointHolder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
            PointHolder.AddComponent<ManipulationHandler>();

            GraphRadius = PointHolder.GetComponent<BoxCollider>().size / 2;

            for (int i = 0; i < numberOfPoints; i++)
            {
                Transform current_point = Instantiate(PointPrefab);
                current_point.GetComponent<Renderer>().material.color = Random.ColorHSV(0.0f, 1.0f);
                current_point.SetParent(PointHolder.GetComponent<BoxCollider>().transform);
                current_point.localPosition = PointHolder.GetComponent<BoxCollider>().center;
                current_point.localScale = new Vector3(0.03f, 0.03f, 0.03f);

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

            //Debug.Log("max range: " + max_range + " min range: " + min_range);

            if (currentIndex < pointFromGraph.XPoints.Count)
            {
                updated_position.x = Util.NormalizeToRange(min_range.x, max_range.x, pointFromGraph.XPoints[currentIndex], GraphXMax, GraphXMin);
                updated_position.y = Util.NormalizeToRange(min_range.y, max_range.y, pointFromGraph.YPoints[currentIndex], GraphYMax, GraphYMin);
                updated_position.z = Util.NormalizeToRange(min_range.z, max_range.z, pointFromGraph.ZPoints[currentIndex], GraphZMax, GraphZMin);

                //Debug.Log(updated_position.ToString("F4"));

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
            plotTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            plotTitle.GetComponent<TextMesh>().anchor = TextAnchor.LowerCenter;
            plotTitle.transform.position = new Vector3(0, GraphRadius.y, -GraphRadius.x);
        }

        private void DrawTime()
        {
            TimeText = Instantiate(Text, Vector3.zero, Quaternion.identity);

            // Add time text
            TimeText.transform.SetParent(PointHolder.transform);
            TimeText.GetComponent<TextMesh>().text = "";
            TimeText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
            xlabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
            ylabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
            zlabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            zlabel.GetComponent<TextMesh>().anchor = TextAnchor.LowerCenter;
            zlabel.transform.position = new Vector3(-GraphRadius.x, -GraphRadius.y, 0);
        }
    }
}
