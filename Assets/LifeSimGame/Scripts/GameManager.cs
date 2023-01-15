using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{
    private int cubeScore = 0;
    public TextMeshProUGUI cubeScoreText;
    private int sphereScore;
    public TextMeshProUGUI sphereScoreText;
    private int cylinderScore;
    public TextMeshProUGUI cylinderScoreText;

    public TextMeshProUGUI winnerText;

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

    public void Start()
    {
        
        StartRound();
    }

    public void StartRound()
    {
        currentTime = 0;
        currentX = x;
        currentZ = z;
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
            var shapes = UnityEngine.Object.FindObjectsOfType<Shape>();

            if (shapes != null && shapes.Length > 0)
            {
                foreach (Shape shape in shapes)
                {
                    if (shape.gameObject.activeInHierarchy)
                    {
                        shape.MapSizeUpdate(currentX, currentZ);
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
            cubeScoreText.text = cubeScore.ToString();        }

        if (type == PrimitiveType.Sphere)
        {
            sphereScore++;
            sphereScoreText.text = sphereScore.ToString();
        }

        if (type == PrimitiveType.Cylinder)
        {
            cylinderScore++;
            cylinderScoreText.text = cylinderScore.ToString();
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
            if (cubeScore <= sphereScore && cubeScore <= cylinderScore)
            {
                winnerText.text = "Cube Won!";
            }

            if (sphereScore <= cubeScore && sphereScore <= cylinderScore)
            {
                winnerText.text = "Sphere Won!";
            }

            if (cylinderScore <= sphereScore && cylinderScore <= cubeScore)
            {
                winnerText.text = "Cylinder Won!";
            }
        }
    }
}
