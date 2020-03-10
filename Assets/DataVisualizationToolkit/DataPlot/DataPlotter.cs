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

        [Tooltip("The color of each point (Optional) default is colour gradient)")]
        public List<Color> colours;

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
        void Awake()
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
            float xMax = Mathf.Max(Xpoints.ToArray());
            float yMax = Mathf.Max(Ypoints.ToArray());
            float zMax = Mathf.Max(Zpoints.ToArray());

            // Get minimums of each axis
            float xMin = Mathf.Min(Xpoints.ToArray());
            float yMin = Mathf.Min(Ypoints.ToArray());
            float zMin = Mathf.Min(Zpoints.ToArray());

            //find mean values so we can postion text at center points of generated plot
            float xMid = Util.FindMiddle(xMax, xMin);
            float zMid = Util.FindMiddle(zMax, zMin);
            float yMid = Util.FindMiddle(yMax, yMin);

            //center the pivot of object to middle of plot 
            PointHolder.transform.position = new Vector3(Util.Normalize(xMid, xMax, xMin), Util.Normalize(yMid, yMax, yMin), Util.Normalize(zMid, zMax, zMin)) * plotScale;

            //Loop through Pointlist
            for (var i = 0; i < Xpoints.Count; i++)
            {
                // Get value in poinList at ith "row", in "column" Name, normalize
                float x = Util.Normalize(Xpoints[i], xMax, xMin);

                float y = Util.Normalize(Ypoints[i], yMax, yMin);

                float z = Util.Normalize(Zpoints[i], zMax, zMin);


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
                //set point colours
                if (colours != null && colours.Count > 0)
                {
                    dataPoint.GetComponent<Renderer>().material.color = colours[i];
                }
                else
                {
                    // Default is colour gradient based on coordinates
                    dataPoint.GetComponent<Renderer>().material.color =
                        new Color(x, y, z, 1.0f);
                }
            }

            GameObject title = Instantiate(Text, new Vector3(Util.Normalize(xMid, xMax, xMin), Util.Normalize(yMax, yMax, yMin), Util.Normalize(zMid, zMax, zMin)) * plotScale, Quaternion.identity);
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
                xLabel = Instantiate(Text, new Vector3(Util.Normalize(xMid, xMax, xMin), Util.Normalize(yMin, yMax, yMin), Util.Normalize(zMin, zMax, zMin)) * plotScale, Quaternion.Euler(90, 0, 0));
            }
            else
            {
                xLabel = Instantiate(Text, new Vector3(Util.Normalize(xMid, xMax, xMin), Util.Normalize(yMin, yMax, yMin), Util.Normalize(zMin, zMax, zMin)) * plotScale, Quaternion.identity);
            }
            xLabel.transform.parent = PointHolder.transform;
            xLabel.GetComponent<TextMesh>().text = xName;
            xLabel.transform.name = "x-label";
            //scale the size of text depending on PlotScale
            xLabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * plotScale;

            if (!twoD)
            {
                //add z label
                GameObject zLabel = Instantiate(Text, new Vector3(Util.Normalize(xMin, xMax, xMin), Util.Normalize(yMin, yMax, yMin), Util.Normalize(zMid, zMax, zMin)) * plotScale, Quaternion.Euler(90, 90, 0));
                zLabel.transform.parent = PointHolder.transform;
                zLabel.GetComponent<TextMesh>().text = zName;
                zLabel.transform.name = "z-label";
                zLabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * plotScale;
            }

            //add y label
            GameObject yLabel = Instantiate(Text, new Vector3(Util.Normalize(xMin, xMax, xMin), Util.Normalize(yMid, yMax, yMin), Util.Normalize(zMax, zMax, zMin)) * plotScale, Quaternion.Euler(0, 0, 90));
            yLabel.transform.parent = PointHolder.transform;
            yLabel.GetComponent<TextMesh>().text = yName;
            yLabel.transform.name = "y-label";
            //scale the size of text depending on PlotScale
            yLabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * plotScale;

            InitalizeInteraction(xMax, yMax, zMax, xMin, yMin, zMin);

            //center the plot in middle of the screen
            PointHolder.transform.position = new Vector3(0, 0, 0);
        }

        private void InitalizeInteraction(float xMax, float yMax, float zMax, float xMin, float yMin, float zMin)
        {
            float xMid = Util.FindMiddle(xMax, xMin);
            float zMid = Util.FindMiddle(zMax, zMin);
            float yMid = Util.FindMiddle(yMax, yMin);

            BoxCollider boxCollider = PointHolder.AddComponent<BoxCollider>();
            PointHolder.transform.gameObject.GetComponent<BoxCollider>().size = new Vector3(Util.Normalize(xMax, xMax, xMin), Util.Normalize(yMax, yMax, yMin), Util.Normalize(zMax, zMax, zMin)) * plotScale;

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
        }
    }
}
