using System;
using UnityEngine;
using UnityEngine.Pool;

public class ShapePool : MonoBehaviour
{
    private PooledObject<Shape> shapePool;

    public ShapePool(PrimitiveType objectType, int maxObjAmt)
    {
        shapePool = new PooledObject<Shape>();
        
    }

    public ShapePool()
    {
    }

    public static GameObject GetObjectFromQueue(PrimitiveType type)
    {
        
        throw new NotImplementedException();
    }

    internal static void PutObjectBackInPoolQueue(PrimitiveType type)
    {
        throw new NotImplementedException();
    }

    internal static void PutObjectBackInPoolQueue(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}