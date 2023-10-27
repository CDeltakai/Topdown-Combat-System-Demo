using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{

    public delegate void DisableObjectEvent(IPoolable poolableObject);
    public event DisableObjectEvent OnDisableObject;

    public delegate void ActivateObjectEvent(IPoolable poolableObject);
    public event ActivateObjectEvent OnActivateObject;

    public void ActivateObject();
    public void DisableObject();


}
