using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//7pm night before, worked about 5 bhours on this a day on average and got to this point,
//where the structure I am finally pretty proud of being stufdy and flxeible but it's still
//not yet functioning at all, and I am worried that I will like during the timed test, spend
//my time on a low priority.  I think I can show you all that I can code and put togethre a
//devent project showcasing som AR stuff, creative thinking, strategic thinking and awareness
//of the agile method prioritizinng feedback and incrememental but demoable working things
//to show to the product owner.  So I'm msiwtchign gears and going to outsourece a lot of
//the optimization to code that I don't carefully research and write from scratch and
//undsrdatnd in and out,

/// <summary>
/// with the plan that I will work on this same project just form 
/// the other end starting with functionality that I can show, 
/// hopefully connecting both ends this structure 
/// and the showcase by tomorrow evening 
/// but if not at least having something to demo.`w
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    private GameObject prefab;
    private CubeObject cubeShape;
    private SphereObject sphereShape;
    private CylinderObject cylinderShape;
    private Stack<Shape> reusableInstances;
    private Stack<Shape> reusableSphereInstances;
    private Stack<Shape> reusableCubeInstances;
    private Stack<Shape> reusableCylinderInstances;

    private Dictionary<PrimitiveType, Stack<Shape>> poolsDict;

    public PoolManager(Dictionary<PrimitiveType,Stack<Shape>> poolSD)
    {
        poolsDict = poolSD;  
        reusableCubeInstances = poolSD[PrimitiveType.Cube];
        reusableSphereInstances = poolSD[PrimitiveType.Sphere];
        reusableCylinderInstances = poolSD[PrimitiveType.Cylinder];
    }

    public Shape GetPrefabInstance(PrimitiveType type, int childCount)
    {
        Shape inst = null;
        var specificPoolRef = poolsDict[type];
        // if we have object in our pool we can use them
        if (specificPoolRef.Count > 0)
        {
            // get object from pool, we have some to spare
            inst = specificPoolRef.Pop();
            inst.Init(childCount);
            // activate object
        }
        else
        {
            GameObject go = ObjectPoolGenerator.Instance.GenerateShape(false, type, childCount);
            inst = go.GetComponent<Shape>();
        }
        // set reference to pool
        inst.Orgin = this.SetPoolToUse(inst.type);
        // and prepare instance for use
        if (!inst.nextGenerationInPool && inst.childCount > 1)
        {
            ObjectPoolGenerator.Instance.GenerateShape(true, inst.type, inst.childCount);
            inst.childCount--;
        }
        return inst;
    }
   
    //potential coroutine implementation for async loading
    //
    /* void AddChildrenToPool(PrimitiveType type, int numChildren)
    {
        var nextGenerationNumChildren = numChildren - 1;
        while (numChildren>nextGenerationNumChildren) //basically do it once
        {
            Shape inst = ObjectGenerator.Instance.GenerateShapeForPool(type);
            inst.Orgin = this.SetPoolToUse(type);
            inst.gameObject.SetActive(false);
            inst.AddToPoolPrep();
            inst.ReturnToPool();
            yield return new WaitForEndOfFrame();
        }
        numChildren++; //add back to the number of children since we didn't actually die yet, just got ready for death
        yield return new WaitForEndOfFrame();
    }*/

    private Stack<Shape> SetPoolToUse(PrimitiveType type)
    {
        switch (type)
        {
            case PrimitiveType.Sphere:
                {
                    reusableInstances = reusableSphereInstances;
                    return reusableInstances; 
                }
            case PrimitiveType.Cylinder:
                {
                    reusableInstances = reusableCylinderInstances;
                    return reusableInstances;
                }
            case PrimitiveType.Cube:
                {
                    reusableInstances = reusableCubeInstances;
                    return reusableInstances;
                 }
                    Debug.LogError("Stack for object pool is not available," +
                        " add to Generic Object pool the poolable object's " +
                        " stack and type");
            default: return null;
        }
    }


        public void ReturnToPool(Shape instance, PrimitiveType poolType)
        {
        // disable object
        instance.gameObject.SetActive(false);
        SetPoolToUse(poolType);
        // add to pool
        reusableInstances.Push(instance);
        }
    
}
