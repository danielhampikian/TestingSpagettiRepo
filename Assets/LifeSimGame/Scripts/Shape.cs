using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shape : GenericPoolableObject
{
    private float timeAliveStartingTime;
    private int availableShapesInPool;
    public bool nextGenerationInPool;
    private float timeAliveEndingTime;
    public float timeAlive;
    public PrimitiveType type;
    private Shape shape;
    private ShapeMover shapeMover;
    public int  childCount;
    public delegate void OnDeathAction(PrimitiveType type, float timeAlive);
    public static event OnDeathAction OnShapeDeath;
    private Color colors = Color.white;
    public Vector3 direction;
    public float floor;
    private float yLevel = 0;
    private bool isEnteringScene;
    // OnMapReduced(currentMapScale);
    private void OnEnable()
    {
        MapManager.OnMapReduced += MapSizeUpdate;
        OnShapeDeath += GameManager.Instance.OnShapeDeath;
        timeAliveStartingTime = Time.time;
    }
  
    void OnDisable()
    {
        MapManager.OnMapReduced -= MapSizeUpdate;
        timeAliveEndingTime = Time.time;
        timeAlive = timeAliveEndingTime - timeAliveStartingTime;
        OnShapeDeath(type, timeAlive);
    }
    public void OnReturnToPool()
    {
        availableShapesInPool++;
        Debug.Log("There are " + availableShapesInPool + " in the pool");
    }
    public override void OnDeath()
    {
        base.OnDeath();
        ReturnToPool();
    }

    public void Init(int childCount)
    {

//reference to it's pool is through .Origin and then through the dictionary in poolManager
        this.shape = GetComponent<Shape>();
        this.floor = MapManager.getMapScaleMax();
        this.direction.y = yLevel; //(add logic for stacking upwards like -floor)
        this.childCount = childCount;
        this.shapeMover = GetComponent<ShapeMover>();
        this.direction.x = Random.Range(-1, 1);
        this.direction.z = Random.Range(-1, 1);
    }
    public override void PrepareToUse()
    {
        shape.transform.rotation = transform.rotation;
        shape.transform.position = transform.position + (-direction * speed * (1 + childCount) * 4);
        Color col = GetColor(childCount);
        shape.GetComponent<Shape>().GetComponent<Renderer>().material.color = col;
        if (childCount > 1)
        {
            AddChildrenToPool(false);
        }
        else AddChildrenToPool(true);
    }
    public void AddChildrenToPool(bool oneGenerationLeft)
    {
        if (!oneGenerationLeft)
        {
            ObjectPoolGenerator.Instance.GenerateShape(false, type, childCount);
            oneGenerationLeft = true;
        }
        else 
        {
            ObjectPoolGenerator.Instance.GenerateShape(false, type, childCount);
        }

    }
    public void MapSizeUpdate(float x)
    {
        
        floor = x;
        if(transform.position.x > floor || transform.position.x < -floor)
        {
            Vector3 newPos = transform.position;
            newPos.x = floor;
            transform.position = newPos;
        }

        if (transform.position.z > floor || transform.position.z < -floor)
        {
            Vector3 newPos = transform.position;
            newPos.z = floor;
            transform.position = newPos;
        }
    }


    private void Update()
    {
        Move();
    }


    public float speed=0.01f;
    public void Move()
    {
        shapeMover.Move(transform.position, direction, speed, floor);
    }

   public void Attack(Shape shape)
    {
        direction *= -1;
        if (shape.gameObject.activeInHierarchy)
        {
            GameManager.Instance.OnShapeDeath(shape.type, timeAlive);
        }
    }

    private Color GetColor(int childCount)
    {
        Color color = Color.white;

        if (childCount == 1)
        {
            color = Color.yellow;
        }
        else if (childCount == 2)
        {
            color = Color.green;
        }
        else if (childCount == 3)
        {
            color = Color.red;
        }
        else if (childCount == 4)
        {
            color = Color.blue;
        }
        return color;
    }

    public void AssembleShapeFromPrimitive(PrimitiveType type)
    {
        GameObject shape = GameObject.CreatePrimitive(type);
        shape.AddComponent<Shape>();
        shape.AddComponent<Rigidbody>();
        shape.GetComponent<Rigidbody>().useGravity = false;
        shape.GetComponent<Rigidbody>().isKinematic = true;
        shape.GetComponent<Collider>().isTrigger = true;
    }

}