using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class PoolItem
{
    public GameObject Prefab;
    public int PoolCount;
    public List<Component> ComponentToAddList;
    public Color color;
    public float size;
    public PrimitiveType type;

}