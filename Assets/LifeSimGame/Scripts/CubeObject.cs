using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Timeout object.
/// It return itself to object pool after provided timeout.
/// </summary>
public class CubeObject : Shape
{
    
    /// <summary>
    /// Unity's method called on object enable
    /// </summary
    public override void PrepareToUse()
    {
        base.PrepareToUse();
        //if you need things like rigid body assign variables here, scale and Color, etc
    }
    /// <summary>
    /// Unity's method called every frame
    /// </summary>
    private void Update()
    {
        //Movement code goes here

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Shape>() != null)
        {

            if (other.GetComponent<Shape>().type==PrimitiveType.Sphere)
            {
                Attack(other.GetComponent<Shape>());
            }
        }
    }


}

