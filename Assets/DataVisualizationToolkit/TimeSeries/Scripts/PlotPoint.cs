using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Data structure that the DataPlotter will refer to
 * to plot and update a point in its graph
 */

public class PlotPoint
{ 
    public List<float> XPoints { get; internal set; }
    public List<float> YPoints { get; internal set; }
    public List<float> ZPoints { get; internal set; }

    public float XMax;
    public float XMin;
    public float YMax;

    public float YMin;
    public float ZMax;
    public float ZMin;

    public int currentPointIndex { get; set; }

    public PlotPoint(List<float> xpoints, List<float> ypoints, List<float> zpoints)
    {
        currentPointIndex = 0;
        XPoints = xpoints;
        YPoints = ypoints;
        ZPoints = zpoints;
        CalculateMaxPoints();
        CalculateMinPoints();
    }

    private void CalculateMaxPoints()
    {
        XMax = XPoints.Max();
        YMax = YPoints.Max();
        ZMax = ZPoints.Max();
    }

    private void CalculateMinPoints()
    {
        XMin = XPoints.Min();
        YMin = YPoints.Min();
        ZMin = ZPoints.Min();
    }
}
