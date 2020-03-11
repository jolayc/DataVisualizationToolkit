using System;
using UnityEngine;
using Mapbox.Unity.Map;
using Microsoft.MixedReality.Toolkit.UI;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
namespace DataVisualization.Plotter
{
    public class mrMap : MonoBehaviour
    {
        [Header("Mix Reality Map Settings")]
        [Tooltip("Latitude Longitude")]
        [Geocode]
        public string location;
        [Tooltip("mapZoomLevel smallest allowed is 2")]
        public int zoom;
        [Tooltip("changes size scale of plot")]
        public float plotScale = 3;
        // Object which will contain instantiated prefabs in hiearchy
        [Tooltip("Object which will contain instantiated prefabs in hiearchy")]
        public GameObject MapHolder;
        // Object which will contain text in hiearchy
        [Tooltip("Object which will contain text in hiearchy (only required if there is a titleName)")]
        public GameObject Text;
        [Tooltip("Title of plot (optional)")]
        public string titleName;
        [Tooltip("enabled 3D terrain default is flat (Beta feature may be buggy)")]
        public bool enable3DTerrain = false;

        //histogram creation settings
        [Header("Histogram Settings")]
        [Tooltip("The location to place data points on")]
        [Geocode]
        public string[] locationStrings;
        [Tooltip("The location height of each histogram bar")]
        public List<float> heightValues;
        [Tooltip("The color of each histogram bar (default is white)")]
        public List<Color> colours;
        [Tooltip("changes size scale of the data points")]
        public float spawnScale = 0.015f;
        [Tooltip("changes max height of the bar")]
        public float HeightScaleMax = 0.95f;
        [Tooltip("changes min height of the bar")]
        public float HeightScaleMin = 0.05f;

        //used to hold the entire map itself
        private GameObject map;
        // Start is called before the first frame update
        void Awake()
        {
            map = new GameObject("Map");
            //centre the map and mapHolder

            map.transform.position = new Vector3(0, 0, 0);
            MapHolder.transform.position = new Vector3(0, 0, 0);
            map.AddComponent<AbstractMap>();
            MapOptions options = map.GetComponent<AbstractMap>().Options;
            options.locationOptions.latitudeLongitude = location;
            options.locationOptions.zoom = zoom;
            options.scalingOptions.unityTileSize = plotScale;
            IImageryLayer image = map.GetComponent<AbstractMap>().ImageLayer;
            image.SetLayerSource(ImagerySourceType.MapboxDark);
            IVectorDataLayer layer = map.GetComponent<AbstractMap>().VectorData;
            layer.SetLayerSource(VectorSourceType.MapboxStreets);
            layer.AddPolygonFeatureSubLayer("buildings", "building");
            layer.FindFeatureSubLayerWithName("buildings").materialOptions.SetStyleType(StyleTypes.Light);
            if (enable3DTerrain)
            {
                ITerrainLayer terrain = map.GetComponent<AbstractMap>().Terrain;
                terrain.SetElevationType(ElevationLayerType.LowPolygonTerrain);
            }
            map.transform.parent = MapHolder.transform;
            map.GetComponent<AbstractMap>().SetPlacementType(MapPlacementType.AtTileCenter);
            InitalizeInteraction();

            if (!String.IsNullOrEmpty(titleName) && Text != null)
            {
                GameObject title = Instantiate(Text, new Vector3(0, 2.5f, 0) * plotScale, Quaternion.identity);
                //place title so it is just above plot
                title.transform.position = title.transform.position + new Vector3(0, title.GetComponent<Renderer>().bounds.size.y / 2, 0) * plotScale;
                //add title 
                title.transform.parent = MapHolder.transform;
                title.GetComponent<TextMesh>().text = titleName;
                title.transform.name = "title";

                //scale the size of text depending on PlotScale
                title.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * plotScale;
                //place title so it is just above plot
                title.transform.position = title.transform.position + new Vector3(0, title.GetComponent<Renderer>().bounds.size.y / 2, 0);
            }

            map.AddComponent<HistoGramOnMap>();
            map.GetComponent<HistoGramOnMap>().locationStrings = locationStrings;
            map.GetComponent<HistoGramOnMap>().heightValues = heightValues;
            map.GetComponent<HistoGramOnMap>().colours = colours;
            map.GetComponent<HistoGramOnMap>().spawnScale = spawnScale;
            map.GetComponent<HistoGramOnMap>().HeightScaleMax = HeightScaleMax;
            map.GetComponent<HistoGramOnMap>().HeightScaleMin = HeightScaleMin;
            map.GetComponent<HistoGramOnMap>().map = map.GetComponent<AbstractMap>();

        }

        private void InitalizeInteraction()
        {
            MapHolder.AddComponent<BoxCollider>();
            MapHolder.transform.gameObject.GetComponent<BoxCollider>().size = new Vector3(3, 2.5f, 3) * plotScale;
            MapHolder.transform.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 1.25f, 0) * plotScale;
            MapHolder.AddComponent<BoundingBox>();
            MapHolder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
            MapHolder.AddComponent<ManipulationHandler>();
        }
       
        // Update is called once per frame
        void Update()
        {
            //allows realtime zoom and location changing on map
            //smallest allowed zoom
            if (zoom < 2)
            {
                zoom = 2;
            }
            //max allowed zoom
            if(zoom > 22)
            {
                zoom = 22;
            }
            map.GetComponent<AbstractMap>().UpdateMap(Conversions.StringToLatLon(location), zoom);
            if (enable3DTerrain)
            {
                ITerrainLayer terrain = map.GetComponent<AbstractMap>().Terrain;
                terrain.SetElevationType(ElevationLayerType.LowPolygonTerrain);
            }
            else
            {
                ITerrainLayer terrain = map.GetComponent<AbstractMap>().Terrain;
                terrain.SetElevationType(ElevationLayerType.FlatTerrain);
            }
        }
    }
}
