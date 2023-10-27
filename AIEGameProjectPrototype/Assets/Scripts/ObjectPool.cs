using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : IPoolable, new()
{
    public delegate void SetupObjectDelegate(IPoolable pooledObject);
    public event SetupObjectDelegate OnPoolIsDry;
    public Stack<IPoolable> readyPool = new Stack<IPoolable>();


    public void AddObject(IPoolable pooledObject, int amount, SetupObjectDelegate setupDelegate = null)
    {
        for(int i = 0; i < amount; i++) 
        {
            setupDelegate?.Invoke(pooledObject);
            readyPool.Push(pooledObject);
        }
    }

    public void UseObject(IPoolable pooledObject, int amount)
    {
        if(readyPool.Count <= amount)
        {
            for(int i = 0; i < amount; i++) 
            {
                OnPoolIsDry?.Invoke(pooledObject);    
            }
        }


        readyPool.Peek().OnDisableObject += ReadyObject;
        readyPool.Peek().ActivateObject();
        readyPool.Pop();


    }

    public void ReadyObject(IPoolable pooledObject)
    {
        readyPool.Push(pooledObject);
    }



}
