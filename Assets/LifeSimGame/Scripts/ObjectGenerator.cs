using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectGenerator : Singleton<ObjectGenerator>
{
    public GameObject spherePrefab;

    public void GenerateShapes(Action<PrimitiveType> OnDeath, int count, float x, float z)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Vector3.zero;

            GameObject cube = Instantiate(cubePrefab);
            GameObject shpere = Instantiate(spherePrefab);
            GameObject cylinder = Instantiate(cylinderPrefab);

            pos.x = Random.Range(-x, x);
            pos.z = Random.Range(-z, z);
            shpere.transform.position = pos;

            pos.x = Random.Range(-x, x);
            pos.z = Random.Range(-z, z);
            cube.transform.position = pos;

            pos.x = Random.Range(-x, x);
            pos.z = Random.Range(-z, z);
            cylinder.transform.position = pos;

            cube.GetComponent<Shape>().Init(3, x, z, OnDeath);
            shpere.GetComponent<Shape>().Init(3, x, z, OnDeath);
            cylinder.GetComponent<Shape>().Init(3, x, z, OnDeath);

        }
    }

    public GameObject cubePrefab;
    public GameObject cylinderPrefab;

    

}
