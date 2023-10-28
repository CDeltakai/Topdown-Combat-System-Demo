using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    int currentHP = 1;




    public void HurtEntity(int damage)
    {

        currentHP -= damage;

        if(currentHP <= 0 )
        {
            DestroyEntity();
        }

    }



    protected virtual void DestroyEntity()
    {
        Destroy(gameObject);
    }


}
