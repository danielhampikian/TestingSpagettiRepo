using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private static float currentMapScale;
    private static float mapScaleMax;
    public delegate void MapReduceAction(float x);
    public static event MapReduceAction OnMapReduced;


    public static void setMapScaleMax(float max)
    {
        mapScaleMax = max;
    }
    public static float getMapScaleMax()
    {
        return mapScaleMax;
    }
    private static float mapScaleMin;
    public static void setMapScaleMin(float min)
    {
        mapScaleMin = min;
    }
    public static float getMapScaleMin()
    {
        return mapScaleMin;
    }
    public static float getCurrentMapScale()
    {
        return currentMapScale;
    }
 
    public void ReduceMap(float valToReduce, float mapShrinkRate)
    {
        currentMapScale = Mathf.Clamp(valToReduce - mapShrinkRate, mapScaleMax / mapScaleMin, mapScaleMax);
        OnMapReduced(currentMapScale);
    }

}
