using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameManager : Singleton<GameManager>
{
    private int cubeScore;
    public int totalShapes = 0;
    public int maxShapesOnScreen = 0;
    public float timeWithinTenOfMaxShapesOnScreen = 0;
    private int sphereScore;
    private int cylinderScore;
    private int sphereTotalScore;
    private int cylinderTotalScore;
    private int cubeTotalScore;
    private float cubeSurvivalScore;
    private float sphereSurvivalScore;
    private float cylinderSurvivalScore;

    public int roundCount = 0;
    public int shapeCount = 10;
    public float mapScaleMax = 10; // the starting max map dimensions on X and Z are the same, and decreasae symetically as the map reduces so no need for two variables for x and z
    public float mapScaleMin = 4;
    public float mapShrinkRate = 1;
    public float roundLength = 10f;
    public float currentTime = 0;
    public float currentX;
    public float currentZ;
    public float chanceToWin = .75f;
    public float chanceToLose = .25f;
    //MapManager.OnMapReduced += MapSizeUpdate;




    [SerializeField] 
    private PoolManager poolManager;
    [SerializeField] 
    private UIManager uiManager;

    public void Start()
    {
        MapManager.setMapScaleMax(mapScaleMax);
        MapManager.setMapScaleMin(mapScaleMin);
        // Allocate memory and create pooled GameObjects
        // the total amount of objects should be 10 * 9 * 8 * 7
        StartRound();
    }

    public void StartRound()
    {
        currentTime = 0;
        PoolManager poolManager = new PoolManager(ObjectPoolGenerator.Instance.GenerateShapesForPool(shapeCount));

        StartCoroutine(ShrinkMap());
    }


    private IEnumerator ShrinkMap()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            MapManager.Instance.ReduceMap(currentX, mapShrinkRate);

            currentTime += 0.01f;

            if (currentTime > roundLength)
            {
                RoundComplete();
            }
        }
    }
/*    public void ReduceMap(float valToReduce, float mapShrinkRate)
    {
        currentMapScale = Mathf.Clamp(valToReduce - mapShrinkRate, mapScaleMax / mapScaleMin, mapScaleMax);
        OnMapReduced(currentMapScale);
    }*/

    public void OnShapeDeath(PrimitiveType type, float timeAlive)
    {
        if (type == PrimitiveType.Cube)
        {
            cubeScore++;
            cubeSurvivalScore += timeAlive;
            cubeTotalScore = cubeScore - Mathf.RoundToInt(cubeSurvivalScore);
            uiManager.updateCubeScoreUI(cubeScore);
        }

        if (type == PrimitiveType.Sphere)
        {
            sphereScore++;
            sphereSurvivalScore += timeAlive;
            sphereTotalScore = sphereScore = Mathf.RoundToInt(timeAlive);

            uiManager.updateSphereScoreUI(sphereScore); 
        }

        if (type == PrimitiveType.Cylinder)
        {
            cylinderScore++;
            cylinderSurvivalScore += timeAlive;
            cylinderTotalScore = cylinderScore - Mathf.RoundToInt(timeAlive);
            uiManager.updateCylinderScoreUI(cylinderScore);
        }
    }

    private void RoundComplete()
    {
        StopAllCoroutines();
        var shapes = Object.FindObjectsOfType<Shape>();

        if (shapes != null)
        {
            foreach(Shape shape in shapes)
            {
                Destroy(shape.gameObject);
            }
        }

        roundCount--;

        if (roundCount > 0)
        {
            StartRound();
        }
        else
        {
            //game over: 
            totalShapes = cubeScore + sphereScore + cylinderScore;
            string winMessage = "Total deaths: " + totalShapes + " and surival time of cube, sphere, and cylinder is: C=" + cubeSurvivalScore + " S= " + sphereSurvivalScore + " C= " + cylinderSurvivalScore;
            if (cubeScore < sphereScore && cubeScore < cylinderScore)
            {

                uiManager.updateWinnerText("Cube Won! Cube deaths: " + cubeScore + " out of " + winMessage);
            }

            if (sphereScore < cubeScore && sphereScore < cylinderScore)
            {
                uiManager.updateWinnerText("Sphere Won! Sphere kills: " + " out of " + winMessage);
            }

            if (cylinderScore < sphereScore && cylinderScore < cubeScore)
            {
                uiManager.updateWinnerText("Cylinder Won! Cylinder kills: " + " out of " + winMessage);
            }
            if ((cylinderScore == cubeScore) || (cylinderScore == sphereScore) || (cubeScore == sphereScore))
            {
                uiManager.updateWinnerText("An unlinkely tie occurred: cube kills: " + cubeScore + " shpere kills: "
                    + sphereScore + " cylinder kills: " + cylinderScore);

            }
           
        }
    }
}
