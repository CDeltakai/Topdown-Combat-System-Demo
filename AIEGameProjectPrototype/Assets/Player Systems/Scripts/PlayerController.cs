using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _reticlePrefab;

    
    void Start()
    {
        
    }


    void InitReticle()
    {
        GameObject reticle = Instantiate(_reticlePrefab, transform.position, Quaternion.identity);
        
    }


}
