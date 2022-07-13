using UnityEngine;
using System.Collections.Generic;

public abstract class GenericObjectPool<T> : MonoBehaviour where T : Object
{
    [SerializeField]
    protected bool _initOnWake = false;
    [SerializeField]
    protected List<T> _poolObjectPrefab = null;
    [SerializeField]
    protected int _defaultPoolSize = 0;
    [SerializeField]
    protected int _maxPoolSize = 0;
    [SerializeField]
    protected int _incrementSize = 0;
    [SerializeField]
    protected List<T> _activeObjects = null;
    [SerializeField]
    protected Stack<T> _inactiveObjects = null;

    protected bool _initialized = false;
    protected int _maxUsed = 0;

    public int MaxAvailable => _maxPoolSize - _activeObjects.Count;

    protected abstract void InitializeNewObject(T obj);
    protected abstract void ActivateObject(T obj);
    protected abstract void DeactivateObject(T obj);

    protected virtual void Awake()
    {
        if (_initOnWake)
        {
            InitPool();
        }
    }

    protected virtual void InitPool(int defaultSizeOverride = -1, int maxSizeOverride = -1)
    {
        if (_poolObjectPrefab != null && _poolObjectPrefab.Count > 0)
        {
            if (defaultSizeOverride > 0)
            {
                _defaultPoolSize = defaultSizeOverride;
            }

            if (maxSizeOverride > 0)
            {
                _maxPoolSize = maxSizeOverride;
            }

            if (!_initialized)
            {
                _activeObjects = new List<T>();
                _inactiveObjects = new Stack<T>();

                for (int i = 0; i < _defaultPoolSize; i++)
                {
                    T prefab = _poolObjectPrefab[Random.Range(0, _poolObjectPrefab.Count)];
                    T poolObject = Instantiate(prefab, gameObject.transform);
                    poolObject.name = prefab.name + " " + i.ToString();
                    InitializeNewObject(poolObject);
                    DeactivateObject(poolObject);
                    _inactiveObjects.Push(poolObject);
                }

                _initialized = true;
            }
        }
    }

    public virtual T RequestObject()
    {
        InitPool();

        if (_inactiveObjects.Count > 0)
        {
            T returnObject = _inactiveObjects.Pop();
            _activeObjects.Add(returnObject);
            ActivateObject(returnObject);

            if (_activeObjects.Count > _maxUsed)
            {
                _maxUsed = _activeObjects.Count;
            }

            return returnObject;
        }
        else if (_activeObjects.Count < _maxPoolSize)
        {
            if (_poolObjectPrefab != null && _poolObjectPrefab.Count > 0)
            {
                int itemsToAdd = Mathf.Max(1, Mathf.Min(_incrementSize, _maxPoolSize - _activeObjects.Count));

                for (int i = 0; i < itemsToAdd; i++)
                {
                    T prefab = _poolObjectPrefab[Random.Range(0, _poolObjectPrefab.Count)];
                    T poolObject = Instantiate(prefab, gameObject.transform);
                    poolObject.name = prefab.name + " " + (_activeObjects.Count + i).ToString();
                    InitializeNewObject(prefab);
                    DeactivateObject(prefab);
                    _inactiveObjects.Push(prefab);
                }

                T returnObject = _inactiveObjects.Pop();
                _activeObjects.Add(returnObject);
                ActivateObject(returnObject);

                if (_activeObjects.Count > _maxUsed)
                {
                    _maxUsed = _activeObjects.Count;
                }

                return returnObject;
            }
        }

        return null;
    }

    public virtual void ReleaseObject(T poolObject)
    {
        DeactivateObject(poolObject);
        _inactiveObjects.Push(poolObject);

        int indexOf = _activeObjects.IndexOf(poolObject);
        if (indexOf >= 0)
        {
            _activeObjects[indexOf] = _activeObjects[_activeObjects.Count - 1];
            _activeObjects.RemoveAt(_activeObjects.Count - 1);
        }
    }

    public virtual void AddPrefabToList(T prefab)
    {
        _poolObjectPrefab.Add(prefab);
    }

    public virtual void ClearPrefabList()
    {
        _poolObjectPrefab.Clear();
    }

    public virtual List<T> GetActiveObjects()
    {
        return _activeObjects;
    }

    public virtual int GetNumberAvailable()
    {
        return _inactiveObjects.Count;
    }
}
