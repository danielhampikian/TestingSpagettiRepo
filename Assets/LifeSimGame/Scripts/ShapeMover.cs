using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ShapeMover : MonoBehaviour, IMove {

    float maxVal;
    Vector3 curPos;
    Vector3 direction;

    public Vector3 Move(Vector3 direction, float speed)
    {
        return Move(curPos, direction, speed, maxVal);
    }
    public Vector3 Move(Vector3 curPosition, Vector3 direction, float speed, float maxVal)
    {
        this.maxVal = maxVal;
        this.curPos = curPosition;

        Vector3 newPos = curPosition + (direction * speed);
        if (Mathf.Abs(newPos.x) >= maxVal)  //greater than or equal handles the zero cases
        {
            direction.x = GenerateNewDirection(direction.x);
            Move(direction, speed);
        }
        if (Mathf.Abs(newPos.z) >= maxVal)
        {
            direction.z = GenerateNewDirection(direction.z);
            Move(direction, speed);
        }

         return newPos;
    }

    
    public float GenerateNewDirection(float isPositiveDirection)
    {
        float newDir = UnityEngine.Random.Range(-1f, 0);
        return isPositiveDirection > 0 ? -newDir : newDir;
    }
}

public interface IMove
{
    public Vector3 Move(Vector3 pos, Vector3 dir, float speed, float maxVal);
}
 
