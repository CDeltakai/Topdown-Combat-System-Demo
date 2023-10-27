using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{

    public delegate void SetUpObjectDelegate(GameObject pooledObject);

    public GameObject prefab; // the prefab to instantiate
    Stack<GameObject> readyPool = new Stack<GameObject>();


    void Start()
    {
        // Check if the prefab has a MonoBehaviour that implements IPoolable
        if (prefab.GetComponent<IPoolable>() == null)
        {
            Debug.LogError("The provided prefab does not contain a MonoBehaviour that implements IPoolable.", this);
        }
    }

    public void AddObject(int amount)
    {
        for(int i = 0; i < amount; i++) 
        {
            GameObject instance = Instantiate(prefab, transform);
            IPoolable poolableObject = instance.GetComponent<IPoolable>();

            poolableObject.DisableObject();
            readyPool.Push(instance);

            poolableObject.OnDisableObject += ReadyObject;
            
        }
    }

    public GameObject GetObject(SetUpObjectDelegate setupDelegate = null)
    {
        GameObject instance;
        if(readyPool.Count  == 0)
        {
            AddObject(1);
        }
        instance = readyPool.Pop();

        setupDelegate?.Invoke(instance);

        instance.GetComponent<IPoolable>().ActivateObject();

        return instance;

    }

    public void ReadyObject(GameObject pooledObject)
    {
        readyPool.Push(pooledObject);
    }



}
