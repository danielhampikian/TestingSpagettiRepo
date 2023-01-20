using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objects used in generic object pool
/// </summary>
public class GenericPoolableObject : MonoBehaviour
{
    // Pool to return object
    public GenericPoolableObject()
    {
    }


    public Stack<Shape> Orgin { get; set; }
    /// <summary>
    /// Prepares instance to use. when a shape is made, we get ready all it's next generation of children for use (instantiate them and add to the pool)
    /// </summary>
    public virtual void PrepareToUse()
    {
        
    }

    public virtual void AddToPoolPrep()
    {

    }

    /// <summary>
    /// Returns instance to pool.
    /// </summary>
    public virtual void ReturnToPool()
    {
        // prepare object for return.
        // you can add additional code here if you want to.
    }

    public virtual void OnDeath()
    {
        gameObject.SetActive(false);
        ReturnToPool();
    }


}
