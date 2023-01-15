using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shape : MonoBehaviour
{
    public PrimitiveType type;
    private ShapeMover shapeMover;
    public int  childCount=3;
    private Action<PrimitiveType> OnDeath;
    protected float speed;
    private Color colors = Color.white;
    public Vector3 direction;
    protected float maxX;
    protected float maxZ;
    public void Init(int childCount, float maxX, float maxZ, Action<PrimitiveType> OnDeath)
    {
    
        this.OnDeath = OnDeath;
        this.maxX = maxX;
        this.maxZ = maxZ;


        this.childCount = childCount;
        shapeMover = new ShapeMover(direction);
        shapeMover.Move();

    }

    public void MapSizeUpdate(float x, float z)
    {
        maxX= x;
        maxZ = z;

        if(Math.Abs(transform.position.x) > Math.Abs(maxX))
        {
            Vector3 newPos = transform.position;
            newPos.x = maxX;
            transform.position = newPos;
        }

        if (Math.Abs(transform.position.z) > Math.Abs(maxZ))
        {
            Vector3 newPos = transform.position;
            newPos.z = maxZ;
            transform.position = newPos;
        }


    }
   
    private void Update()
    {
        shapeMover.Move();
    }

    //Clean up if it works with shape mover class
    private float GenerateNewDirection()
    {
        return Random.Range(-1f, 1f);
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
        //My idea with the refactoring floorX to just be represented by MaxX was that there's a symetery to the game if it starts at the origin we can take advantage fo with the -min and the +max being the same number between map reductions

        for (int i = 0; i < childCount; i++)
        {
            GameObject shape = GameObject.CreatePrimitive(type);
            shape.AddComponent<Shape>();
            shape.AddComponent<Rigidbody>(); //probably don't need this for this kind of game
            shape.GetComponent<Rigidbody>().useGravity = false;
            shape.GetComponent<Rigidbody>().isKinematic = true;
            shape.GetComponent<Collider>().isTrigger = true;
            shape.GetComponent<Shape>().Init(childCount - 1, maxX, maxZ, OnDeath);
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