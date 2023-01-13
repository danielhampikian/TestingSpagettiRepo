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
            var shapes = UnityEngine.Object.FindObjectsOfType<Shape>();  //TODO: woof that's going to be a hit, refactor to find without searching every object

            if (shapes != null && shapes.Length > 0)
            {
                foreach (Shape shape in shapes)
                {
                    if (shape.gameObject.activeInHierarchy)
                    {
                        shape.MapSizeUpdate(currentX, currentZ); //TODO: should probably be in a differnt class, or named differentlu
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
    //TODO start refactor by generating all shapes that could be used on startup, unless this is a memory issue, then object pool to avoid this
    private void OnShapeDeath(PrimitiveType type)
    {
        //this is curious as a scoring system becuase you get points for dying the winning seems backwards if this is truly a life simulation game
        if (type == PrimitiveType.Cube)
        {
            cubeScore++;
            cubeScoreText.text = cubeScore.ToString();
            Debug.Log("see if I misunderstood: adding Score to Cube because it died");  
            
        }

        if (type == PrimitiveType.Sphere)
        {
            sphereScore++;
            sphereScoreText.text = sphereScore.ToString();
            Debug.Log("see if I misunderstood: adding Score to Sphere because it died");

        }

        if (type == PrimitiveType.Cylinder)
        {
            cylinderScore++;
            cylinderScoreText.text = cylinderScore.ToString();
            Debug.Log("see if I misunderstood: adding Score to Cylinder because it died but also reproduced? ");

        }
    }

    private void RoundComplete()
    {
        StopAllCoroutines();
        var shapes = Object.FindObjectsOfType<Shape>(); //again we need to object pool here

        if (shapes != null)
        {
            foreach(Shape shape in shapes)
            {
                Destroy(shape.gameObject); //yikes
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
                winnerText.text = "Cube Won!"; //Ah that's interesting so the logic is right, it's just kind of implemented oddly
            }

            if (sphereScore <= cubeScore && sphereScore <= cylinderScore)
            {
                winnerText.text = "Sphere Won!"; //Golf rules heh
            }

            if (cylinderScore <= sphereScore && cylinderScore <= cubeScore)
            {
                winnerText.text = "Cylinder Won!";
            }
            //need to check for and deccide what to do about ties right?  The logic might prevent it think about it before you implement
        }
    }
}
