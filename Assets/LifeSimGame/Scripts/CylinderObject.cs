using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Timeout object.
/// It return itself to object pool after provided timeout.
/// </summary>
public class CylinderObject : Shape
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Shape>() != null)
        {
            if (other.GetComponent<Shape>().type == PrimitiveType.Cube)
            {
                Attack(other.GetComponent<Shape>());
            }
        }

    }
}

