
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
	// Default pool size for each pool item
	public static int DEFAULT_POOL_COUNT = 1;

	// Data Setup
	[SerializeField] private List<PoolItem> poolableObjects = new List<PoolItem>();

//less safe but more perfomative, could throw an error if we aren't careful about putting things back into the pool
	[SerializeField] private bool collectionChecks = false;

	// Maximum collection size allocated per pooled object
	[SerializeField] private int maxPoolSize = 10;

	private Dictionary<string, IObjectPool<PooledGameObject>> pool;
	public Dictionary<string, IObjectPool<PooledGameObject>> Pool => pool;

	// Pool is ready to be used
	public bool IsReady { get; private set; }

	public Action OnCleanup;
	public Action OnRelease;

	

	public void InstantiatePool()
	{
		if (IsReady)
		{
			return;
		}

		pool = DictionaryPool<string, IObjectPool<PooledGameObject>>.Get();

		// Memory allocation and initialize GameObject
		poolableObjects.ForEach(
			item =>
			{
			// Create stack in dictionary
			CreateObjectsPool(item);
				InitializeObjectsPool(item, item.PoolCount);
			}
		);

		// Disable all pooled objects
		OnRelease?.Invoke();
		IsReady = true;
	}
	private void InitializeObjectsPool(PoolItem item, int count)
	{
		if (!item.Prefab)
		{
			throw new Exception("Pool Item prefab is null");
		}

		var id = item.Prefab.name;
		if (!pool.TryGetValue(id, out var objectPool))
		{
			CreateObjectsPool(item);
			objectPool = pool[id];
		}

		// Object shouldn't be release at this point else it will just take the same item from the top of the stack
		for (var i = 0; i < count; ++i)
		{
			objectPool.Get();
		}

	}


	public void ReleasePooledObject()
	{
		if (!IsReady)
		{
			return;
		}

		OnRelease?.Invoke();
	}

	private void CreateObjectsPool(PoolItem item)
	{
		if (!item.Prefab)
		{
			throw new Exception("Pool Item prefab is null");
		}

		var id = item.Prefab.name;

		// Game object doesn't get created at this point, we need to call pool.Get() to do so
		pool.Add(
			id,
			new ObjectPool<PooledGameObject>(
				() => CreatePooledItem(item.Prefab),
				OnGetFromPool,
				OnReleaseToPool,
				OnDestroyPoolObject,
				collectionChecks,
				item.PoolCount,
				maxPoolSize
			)
		);
	}

	private PooledGameObject CreatePooledItem(GameObject prefab)
	{
		GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
		go.name = prefab.name;

		var pooledGO = go.AddComponent<PooledGameObject>();
		pooledGO.Initialize(this, prefab.name);
		return pooledGO;
	}

	private void OnGetFromPool(PooledGameObject pooledGO)
	{
		pooledGO.SetActive(true);
	}

	private void OnReleaseToPool(PooledGameObject releaseGO)
	{
		releaseGO.transform.SetParent(transform);
		releaseGO.SetActive(false);
	}

	private void OnDestroyPoolObject(PooledGameObject destroyedGO)
	{
		Destroy(destroyedGO.gameObject);
	}

}
