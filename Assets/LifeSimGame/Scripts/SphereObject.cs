using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Timeout object.
/// It return itself to object pool after provided timeout.
/// </summary>
public class SphereObject : Shape
{
    [SerializeField]
    private GameObject spherePrefab;

 
    private void OnTriggerEnter(Collider other)
    { //seperate this out sinice for each object type we know what it's type is in the pool
        //we're also doing duplicate checks here, need to optimize so only one is checking when they encounter each other so no race conditions
        if (other.GetComponent<Shape>() != null)
        {
            if (other.GetComponent<Shape>().type == PrimitiveType.Cylinder)
            {
                base.Attack(other.GetComponent<Shape>());
            }

        }
    }

}

