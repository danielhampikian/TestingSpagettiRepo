using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

	private static T instance = null;

	public static T Instance
	{
		get
		{
			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance != null)
		{
			Debug.LogError(name + "error: already initialized", this);
		}

		instance = (T)this;
	}
}
