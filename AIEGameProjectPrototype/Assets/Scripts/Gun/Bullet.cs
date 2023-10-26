using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float lifetime = 2;
    public Rigidbody rigBody { get; private set; }

    public DamagePayload damagePayload;

    public float speed = 50;
    public Vector3 velocity = new Vector3();


    private void Awake() 
    {
        rigBody = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct(lifetime));
    }

    private void FixedUpdate() 
    {
        //Move bullet with given velocity
        rigBody.MovePosition(rigBody.position + velocity * Time.fixedDeltaTime * speed);    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if(other.CompareTag("Enemy"))
        {
            Entity entity = other.GetComponent<Entity>();
            if(entity)
            {
                entity.HurtEntity(damagePayload.baseDamage);
                Destroy(gameObject);
            }
        }    
    }



    IEnumerator SelfDestruct(float delay = 2)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
