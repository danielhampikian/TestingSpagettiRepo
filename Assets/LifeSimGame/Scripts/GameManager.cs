using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{
    private int cubeScore;
    private int sphereScore;
    private int cylinderScore;

    public bool testTie = true;
    public int roundCount = 0;
    public int shapeCount = 10;
    public float x = 10;
    public float z = 10;
    public float mapShrinkRate = 1;
    public float roundLength = 10f;
    public float currentTime = 0;
    public float currentX;
    public float currentZ;

    private Action<float, float> OnMapUpdate;
    [SerializeField] 
    private PoolManager poolManager;
    [SerializeField] 
    private UIManager uiManager;

    public void Start()
    {
        
        StartRound();
    }

    public void StartRound()
    {
        currentTime = 0;
        currentX = x;
        currentZ = z;

        // Allocate memory and create pooled GameObjects

            poolManager.InstantiatePool();


        // Get GameObject from pool 
        var cube = poolManager.Spawn("Cube");
        var sphere = poolManager.Spawn("Sphere");
        var cylinder = poolManager.Spawn("cylinder");


        ObjectGenerator.Instance.GenerateShapes(OnShapeDeath, shapeCount, x, z);

        StartCoroutine(ShrinkMap());
    }


    private IEnumerator ShrinkMap()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            
            currentX = Mathf.Clamp(currentX - mapShrinkRate, x / 4, x);
            currentZ = Mathf.Clamp(currentZ - mapShrinkRate, z / 4, x);
            var shapes = FindObjectsOfType<Shape>();

            if (shapes != null && shapes.Length > 0)
            {
                foreach (Shape shape in shapes)
                {
                    if (shape.gameObject.activeInHierarchy)
                    {
                        shape.MapSizeUpdate(currentX, currentZ); //this is the current mechanism to get shapes to interact, but it's not really what's moving them 
                    }
                }
            }

            currentTime += 0.01f;

            if (currentTime > roundLength)
            {
                RoundComplete();
            }
        }
    }

    private void OnShapeDeath(PrimitiveType type)
    {
        if (type == PrimitiveType.Cube)
        {
            cubeScore++;
            uiManager.updateCubeScoreUI(cubeScore);
        }

        if (type == PrimitiveType.Sphere)
        {
            sphereScore++;
            uiManager.updateSphereScoreUI(sphereScore); 
        }

        if (type == PrimitiveType.Cylinder)
        {
            cylinderScore++;
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

        if(roundCount > 0)
        {
            StartRound();
        }
        else
        {
            if (testTie)
            {
                cubeScore = sphereScore = cylinderScore = 1;
            }
            if (cubeScore < sphereScore && cubeScore < cylinderScore)
            {
                uiManager.updateWinnerText("Cube Won!");
            }

            if (sphereScore < cubeScore && sphereScore < cylinderScore)
            {
                uiManager.updateWinnerText("Sphere Won!");
            }

            if (cylinderScore < sphereScore && cylinderScore < cubeScore)
            {
                uiManager.updateWinnerText("Cylinder Won!");
            }
            if ((cylinderScore == cubeScore) && (cylinderScore == sphereScore))
            {
                uiManager.updateWinnerText("An unlinkely, perhaps impossible, tie! " +
                    "Everyone won, and no one won!!!  " +
                    "But was it every really just about winning?");

            }
           
        }
    }
}
