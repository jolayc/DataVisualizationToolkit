using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Microsoft.MixedReality.Toolkit.UI;

namespace DataVisualization.Plotter
{
    public class DataPlotter : MonoBehaviour
    {
        [Tooltip("X values")]
        public List<float> Xpoints = new List<float>();
        [Tooltip("Y values")]
        public List<float> Ypoints = new List<float>();
        [Tooltip("Z values leave empty if you want a 2D Plot")]
        public List<float> Zpoints = new List<float>();

        // Full column names
        [Tooltip("X axis label")]
        public String xName;
        [Tooltip("Y axis label")]
        public String yName;
        [Tooltip("z axis label")]
        public String zName;

        //Title Text
        [Tooltip("Title of plot")]
        public String titleName;

        [Tooltip("changes size scale of plot ie 1 the plot will be 1 m in size")]
        public float plotScale = 10;

        // The prefab for the data points that will be instantiated
        [Tooltip("The prefab for the data points that will be instantiated")]
        public GameObject PointPrefab;

        // Object which will contain instantiated prefabs in hiearchy
        [Tooltip("Object which will contain instantiated prefabs in hiearchy")]
        public GameObject PointHolder;

        [Tooltip("(optional) AppBar which will be used for plot use only the APPBar found in MRTK")]
        public GameObject appBar;

        // Object which will contain text in hiearchy
        [Tooltip("Object which will contain text in hiearchy")]
        public GameObject Text;

        [Tooltip("Material applied to handles when they are not in a grabbed state (Optional)")]
        public Material handleMaterial;

        [Tooltip("Material applied to handles while they are a grabbed (Optional)")]
        public Material handelGrabbedMaterial;

        [Tooltip("Prefab used to display rotation handles. If not set a sphere will be displayed instead")]
        public GameObject rotationHandle;

        [Tooltip("Prefab used to display scale handles in corners. If not set, boxes will be displayed instead")]
        public GameObject scaleHandle;

        private Boolean twoD=false;

        // Use this for initialization
        void Start()
        {
            //if Zpoints was left out initalize it to zeros (2D scatter plot)
            if (Zpoints.Count == 0)
            {
                for(var i=0; i<Xpoints.Count; i++)
                {
                    Zpoints.Add(0);
                }
                twoD = true;
            }

            // Get maxes of each axis
            float xMax = FindMaxValue(Xpoints);
            float yMax = FindMaxValue(Ypoints);
            float zMax = FindMaxValue(Zpoints);

            // Get minimums of each axis
            float xMin = FindMinValue(Xpoints);
            float yMin = FindMinValue(Ypoints);
            float zMin = FindMinValue(Zpoints);

            //find mean values so we can postion text at center points of generated plot
            float xMid = FindMiddle(xMax, xMin);
            float zMid = FindMiddle(zMax, zMin);
            float yMid = FindMiddle(yMax, yMin);

            //center the pivot of object to middle of plot 
            PointHolder.transform.position = new Vector3(normalize(xMid, xMax, xMin), normalize(yMid, yMax, yMin), normalize(zMid, zMax, zMin)) * plotScale;

            //Loop through Pointlist
            for (var i = 0; i < Xpoints.Count; i++)
            {
                // Get value in poinList at ith "row", in "column" Name, normalize
                float x = normalize(Xpoints[i], xMax, xMin);

                float y = normalize(Ypoints[i], yMax, yMin);

                float z = normalize(Zpoints[i], zMax, zMin);


                // Instantiate as gameobject variable so that it can be manipulated within loop
                GameObject dataPoint = Instantiate(
                        PointPrefab,
                        new Vector3(x, y, z) * plotScale,
                        Quaternion.identity);

                // Make child of PointHolder object, to keep points within container in hiearchy
                dataPoint.transform.parent = PointHolder.transform;

                // Assigns original values to dataPointName
                string dataPointName =
                    "(" + "x:" +
                    Xpoints[i] + ","
                    + "y:" + Ypoints[i] + ","
                    + "z:" + Zpoints[i] + ")";

                // Assigns name to the prefab
                dataPoint.transform.name = dataPointName;

                // Scale dataPoint depending on Plot scale
                dataPoint.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f) * plotScale;
                // Gets material color and sets it to a new RGB color we define
                dataPoint.GetComponent<Renderer>().material.color =
                    new Color(x, y, z, 1.0f);
            }

            GameObject title = Instantiate(Text, new Vector3(normalize(xMid, xMax, xMin), normalize(yMax, yMax, yMin), normalize(zMid, zMax, zMin)) * plotScale, Quaternion.identity);
            //add title 
            title.transform.parent = PointHolder.transform;
            title.GetComponent<TextMesh>().text = titleName;
            title.transform.name = "title";
            //scale the size of text depending on PlotScale
            title.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * plotScale;
            //place title so it is just above plot
            title.transform.position = title.transform.position + new Vector3(0,title.GetComponent<Renderer>().bounds.size.y/2, 0);

            //add x label
            GameObject xLabel;
            if (!twoD)
            {
                xLabel = Instantiate(Text, new Vector3(normalize(xMid, xMax, xMin), normalize(yMin, yMax, yMin), normalize(zMin, zMax, zMin)) * plotScale, Quaternion.Euler(90, 0, 0));
            }
            else
            {
                xLabel = Instantiate(Text, new Vector3(normalize(xMid, xMax, xMin), normalize(yMin, yMax, yMin), normalize(zMin, zMax, zMin)) * plotScale, Quaternion.identity);
            }
            xLabel.transform.parent = PointHolder.transform;
            xLabel.GetComponent<TextMesh>().text = xName;
            xLabel.transform.name = "x-label";
            //scale the size of text depending on PlotScale
            xLabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * plotScale;

            if (!twoD)
            {
                //add z label
                GameObject zLabel = Instantiate(Text, new Vector3(normalize(xMin, xMax, xMin), normalize(yMin, yMax, yMin), normalize(zMid, zMax, zMin)) * plotScale, Quaternion.Euler(90, 90, 0));
                zLabel.transform.parent = PointHolder.transform;
                zLabel.GetComponent<TextMesh>().text = zName;
                zLabel.transform.name = "z-label";
                zLabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * plotScale;
            }

            //add y label
            GameObject yLabel = Instantiate(Text, new Vector3(normalize(xMin, xMax, xMin), normalize(yMid, yMax, yMin), normalize(zMax, zMax, zMin)) * plotScale, Quaternion.Euler(0, 0, 90));
            yLabel.transform.parent = PointHolder.transform;
            yLabel.GetComponent<TextMesh>().text = yName;
            yLabel.transform.name = "y-label";
            //scale the size of text depending on PlotScale
            yLabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * plotScale;

            InitalizeInteraction(xMax, yMax, zMax, xMin, yMin, zMin);

            //center the plot in middle of the screen
            PointHolder.transform.position = new Vector3(0, 0, 0);
        }

        private float FindMaxValue(List<float> values)
        {
            //set initial value to first value
            float maxValue = values[0];

            //Loop through, overwrite existing maxValue if new value is larger
            for (var i = 0; i < values.Count; i++)
            {
                if (maxValue < values[i])
                    maxValue = values[i];
            }

            //Spit out the max value
            return maxValue;
        }

        private float FindMinValue(List<float> values)
        {

            float minValue = values[0];

            //Loop through, overwrite existing minValue if new value is smaller
            for (var i = 0; i < values.Count; i++)
            {
                if (values[i] < minValue)
                    minValue = values[i];
            }

            return minValue;
        }

        private float FindMeanValue(List<float> values)
        {
            float total = 0;
            //Loop through, overwrite existing minValue if new value is smaller
            for (var i = 0; i < values.Count; i++)
            {
                total = total + values[i];
            }
            return total / values.Count;
        }

        private float FindMiddle(float max, float min)
        {
            return (max + min) / 2;
        }

        private float normalize(float value, float max, float min)
        {
            //if values are all zero or constant
            if (max - min == 0)
            {
                return value;
            }
            else
            {
                return (value - min) / (max - min);
            }
        }

        private void InitalizeInteraction(float xMax, float yMax, float zMax, float xMin, float yMin, float zMin)
        {
            float xMid = FindMiddle(xMax, xMin);
            float zMid = FindMiddle(zMax, zMin);
            float yMid = FindMiddle(yMax, yMin);

            BoxCollider boxCollider = PointHolder.AddComponent<BoxCollider>();
            PointHolder.transform.gameObject.GetComponent<BoxCollider>().size = new Vector3(normalize(xMax, xMax, xMin), normalize(yMax, yMax, yMin), normalize(zMax, zMax, zMin)) * plotScale;

            PointHolder.AddComponent<BoundingBox>();
            PointHolder.GetComponent<BoundingBox>().WireframeEdgeRadius= PointHolder.GetComponent<BoundingBox>().WireframeEdgeRadius * plotScale;
            PointHolder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
            PointHolder.AddComponent<ManipulationHandler>();

            //scale handle sizes
            PointHolder.GetComponent<BoundingBox>().ScaleHandleSize= PointHolder.GetComponent<BoundingBox>().ScaleHandleSize * plotScale;
            PointHolder.GetComponent<BoundingBox>().RotationHandleSize= PointHolder.GetComponent<BoundingBox>().RotationHandleSize * plotScale;


            //Optional handle prefab Models
            PointHolder.GetComponent<BoundingBox>().HandleGrabbedMaterial=handelGrabbedMaterial;
            PointHolder.GetComponent<BoundingBox>().HandleMaterial= handleMaterial;
            PointHolder.GetComponent<BoundingBox>().ScaleHandlePrefab=scaleHandle;
            PointHolder.GetComponent<BoundingBox>().RotationHandleSlatePrefab=rotationHandle;

            //Optional add appBar
            if (appBar != null)
            {
                GameObject objectBar = Instantiate(appBar, new Vector3(0, 0, 0), Quaternion.identity);
                objectBar.transform.parent = PointHolder.transform.parent;
                objectBar.transform.localScale = new Vector3(2f, 2f, 2f) * plotScale;
                var so= new SerializedObject(objectBar.GetComponent<AppBar>());
                so.FindProperty("boundingBox").objectReferenceValue = PointHolder.GetComponent<BoundingBox>();
                so.ApplyModifiedProperties();
            }
        }
    }
}
