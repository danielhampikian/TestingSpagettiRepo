using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPoolGenerator : Singleton<ObjectPoolGenerator>
{
    //ShapeSpawner : PoolerBase<Shape>
    Stack<Shape> poolStackCube;
    Stack<Shape> poolStackCylinder;
    Stack<Shape> poolStackSphere;
 
    [SerializeField]
    private GameObject spherePrefab;
    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private GameObject cylinderPrefab;
    [SerializeField]
    private GameObject[] mutationPrefabs; //allow mutations with varyinig rules - stretch goal

    private bool prepoolAllObjects;
    public Dictionary<PrimitiveType, Stack<Shape>> poolDictionary;
    private Action<PrimitiveType> OnDeath;
    [SerializeField]
    private int firstObjectCount = 10;
    [SerializeField]
    private float max;
    [SerializeField]
    private int initialChildCount = 3;

    //these can all be set up in a prefab so that the only thing we have to change are:
    //child count, scale, color, starting position, but we'll need seperate lists for each
    //MapManager.OnMapReduced += MapSizeUpdate;


    public Dictionary<PrimitiveType,Stack<Shape>> GenerateShapesForPool(int count)
    {
        firstObjectCount = count;
        poolDictionary = new Dictionary<PrimitiveType, Stack<Shape>>();
        PrimitiveType type;
        if (prepoolAllObjects)
        {
            count = count * 9 * 8 * 7;
        }

            poolStackCube = new Stack<Shape>();
            poolStackSphere = new Stack<Shape>();
            poolStackCylinder = new Stack<Shape>();
        for (int i = 0; i < count; i++)
        {
            bool parentSetUp = false;
            for (int j = 0; j < initialChildCount; j++)
            { //on the first run through for each counted shape, the parent is set up for pooling but left active in the scene and Init is called, 
                //the children (3 passees) are 
                GenerateShape(parentSetUp, PrimitiveType.Cube, initialChildCount);
                GenerateShape(parentSetUp, PrimitiveType.Sphere, initialChildCount);
                GenerateShape(parentSetUp, PrimitiveType.Cylinder, initialChildCount);
                parentSetUp = true;
            }
        }
        poolDictionary.Add(PrimitiveType.Sphere, poolStackSphere);
        poolDictionary.Add(PrimitiveType.Cube, poolStackCube);
        poolDictionary.Add(PrimitiveType.Cylinder, poolStackCylinder);
        return poolDictionary;
        }

        public Shape GenerateShape(bool prePool, PrimitiveType type, int childCount, bool returnShape)
        
        {
        GameObject go = GenerateShape(prePool, type, childCount);
        return go.GetComponent<Shape>();
        }
        //Generates a shape but does not yet link it to the pool, this is handled by the Pool Mangaer class
        public GameObject GenerateShape(bool prePool, PrimitiveType type, int childCount)
        { 
        //we assume this gets called =after Gennerate SHapes which initializes values, but really thhis should be done statically and in the editor

        GameObject go = null;
        Shape s = null;
        for (int i = 0; i < childCount; i++)
        {
            switch (type)
            {
                case PrimitiveType.Sphere:
                    {
                        go = Instantiate(spherePrefab);
                        s = go.GetComponent<Shape>();
                        s.Orgin = poolStackSphere;

                        if (prePool)
                        {
                            go.SetActive(false);
                            poolStackSphere.Push(go.GetComponent<Shape>());
                        }
                        break;
                    }

                case PrimitiveType.Cylinder:
                    {
                        go = Instantiate(spherePrefab);
                        s = go.GetComponent<Shape>();
                        s.Orgin = poolStackSphere;

                        if (prePool)
                        {
                            go.SetActive(false);
                            poolStackSphere.Push(go.GetComponent<Shape>());
                        }
                        break;
                    }
                case PrimitiveType.Cube:

                    go = Instantiate(cubePrefab);
                    s = go.GetComponent<Shape>();
                    s.Orgin = poolStackCube;

                    if (prePool)
                    {
                        go.SetActive(false);
                        poolStackCube.Push(go.GetComponent<Shape>());
                    }
                    break;
            }
            if (!prePool)
            {
                go.SetActive(true);
                s.Init(childCount);
            }
        }
        return go;  
    }
}
