using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shape : MonoBehaviour
{
    public PrimitiveType type;

    public int  childCount=3;
    private Action<PrimitiveType> OnDeath;

    private Color colors = Color.white;
    public Vector3 direction;

    public void Init(int childCount, float maxX, float maxZ, Action<PrimitiveType> OnDeath)
    {
        GenerateNewDirection();
        this.OnDeath = OnDeath;
        this.floorX = maxX;
        this.floorz = maxZ;
    this.childCount = childCount;

    }

    public void MapSizeUpdate(float x, float z)
    {
        floorX = x;
        floorz = z;

        if(transform.position.x > floorX || transform.position.x < -floorX)
        {
            Vector3 newPos = transform.position;
            newPos.x = floorX;
            transform.position = newPos;
        }

        if (transform.position.z > floorz || transform.position.z < -floorz)
        {
            Vector3 newPos = transform.position;
            newPos.z = floorX;
            transform.position = newPos;
        }


    }

    public float floorX;
    public float floorz;
    private void Update()
    {
        Move();
    }

    public void Awake()
    {
        
    }

    public float speed=0.01f;
    public void Move()
    {
        Vector3 newPos = transform.position + (direction * speed);

        if (direction == Vector3.zero 
            || (newPos.x > floorX || newPos.x < -floorX)
             || (newPos.z > floorz || newPos.z < -floorz))
        {
            GenerateNewDirection();
        }
        else
        {
            transform.position = newPos;
        }
    }

    private void GenerateNewDirection()
    {
        direction.x = Random.Range(-1f, 1f);
        direction.z = Random.Range(-1f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Shape>() != null)
        {
            if(type == PrimitiveType.Cube && other.GetComponent<Shape>().type == PrimitiveType.Sphere)
            {
                Attack(other.GetComponent<Shape>());
            }

            if (type == PrimitiveType.Sphere && other.GetComponent<Shape>().type == PrimitiveType.Cylinder)
            {
                Attack(other.GetComponent<Shape>());
            }

            if (type == PrimitiveType.Cylinder && other.GetComponent<Shape>().type == PrimitiveType.Cube)
            {
                Attack(other.GetComponent<Shape>());
            }
        }
    }

    void Attack(Shape shape)
    {
        direction *= -1;
        if (shape.gameObject.activeInHierarchy)
        {
            shape.Destroy();
        }
    }

    public void Destroy()
    {
        gameObject.SetActive(false);

        CreateChildren();

        OnDeath?.Invoke(type);
        Destroy(gameObject);
    }

    private void CreateChildren()
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

        for (int i = 0; i < childCount; i++)
        {
            GameObject shape = GameObject.CreatePrimitive(type);
            shape.AddComponent<Shape>();
            shape.AddComponent<Rigidbody>();
            shape.GetComponent<Rigidbody>().useGravity = false;
            shape.GetComponent<Rigidbody>().isKinematic = true;
            shape.GetComponent<Collider>().isTrigger = true;
            shape.GetComponent<Shape>().Init(childCount - 1, floorX, floorz, OnDeath);
            shape.GetComponent<Shape>().speed = speed * (1 + (i / childCount));
            shape.GetComponent<Shape>().type = type;

            shape.transform.localScale = transform.localScale;
            shape.transform.localScale *= 0.9f;
            shape.transform.rotation = transform.rotation;
            shape.transform.position = transform.position + (-direction * speed * (1 + i) * 4);

            shape.GetComponent<Shape>().GetComponent<Renderer>().material.color = color;
        }
    }
}