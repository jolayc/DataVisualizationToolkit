using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static float FindMeanValue(List<float> values)
    {
        float total = 0;
        //Loop through, overwrite existing minValue if new value is smaller
        for (var i = 0; i < values.Count; i++)
        {
            total = total + values[i];
        }
        return total / values.Count;
    }

    public static float FindMiddle(float max, float min)
    {
        return (max + min) / 2;
    }

    public static float FindMaxValue(List<float> values)
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

    public static float FindMinValue(List<float> values)
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

    public static float Normalize(float value, float max, float min)
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

    public static float NormalizeToRange(float a, float b, float value, float max, float min)
    {
        // Range [a, b]
        float normalized_value = Normalize(value, max, min);
        float result = ((b - a) * normalized_value) + a;
        return result;
    }
}
