using UnityEngine;

public class ShapeMover : Shape, IMove //Should all just be an interface, the ineritence structure is bad here but I'm running out of time, should refactor this so that the whole thing is an interface
{
    Vector3 direction;

    public ShapeMover( Vector3 direction)
    {
        this.direction = direction;
    
    }
    public void Move()
    {
        Vector3 newPos = transform.position + (direction * speed);
        if (Mathf.Abs(newPos.x) >= Mathf.Abs(maxX))  //greater than or equal handles the zero cases
        {
            direction.x = GenerateNewDirection(direction.x);
        }
        if (Mathf.Abs(newPos.z) >= Mathf.Abs(maxZ))
        {
            direction.z = GenerateNewDirection(direction.z);
        }

        else
        {
            transform.position = newPos;
        }
    }


    private float GenerateNewDirection(float positiveOrNegativeDirection)
    {
        float newDir = Random.Range(-1f, 0);
        if (positiveOrNegativeDirection <= 0)
        {
            return -newDir;
        }

        return newDir;
    }
}

internal interface IMove
{
    public void Move();
}