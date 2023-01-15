using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ShapeMover : Shape, IMove //Should all just be an interface, the ineritence structure is bad here but I'm running out of time, should refactor this so that the whole thing is an interface
{
    public ShapeMover()
    {
    }
    public void Move()
    {
        Vector3 newPos = base.transform.position + (base.direction * base.speed);
        if (Mathf.Abs(newPos.x) >= Mathf.Abs(base.floorX))  //greater than or equal handles the zero cases
        {
            base.direction.x = GenerateNewDirection(direction.x);
        }
        if (Mathf.Abs(newPos.z) >= Mathf.Abs(base.floorz))
        {
            base.direction.z = GenerateNewDirection(direction.z);
        }
        else
        {
            base.transform.position = newPos;
        }
    }
    private float GenerateNewDirection(float isPositiveDirection)
    {
        float newDir = UnityEngine.Random.Range(-1f, 0);
        return isPositiveDirection > 0 ? -newDir : newDir;
    }
}

public interface IMove
{
    public void Move();
}
 
