using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    public bool objectIsPooled;

    public float lifetime = 2;
    public Rigidbody rigBody { get; private set; }

    public DamagePayload damagePayload;

    public float speed = 50;
    public Vector3 velocity = new Vector3();

    public event IPoolable.DisableObjectEvent OnDisableObject;
    public event IPoolable.ActivateObjectEvent OnActivateObject;

    Coroutine CR_SelfDestruct = null;
    public TrailRenderer trailRenderer { get; private set; }

    protected virtual void Awake() 
    {
        rigBody = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        CR_SelfDestruct = StartCoroutine(SelfDestruct(lifetime));
    }

    protected virtual void FixedUpdate() 
    {
        //Move bullet with given velocity
        rigBody.MovePosition(rigBody.position + velocity * Time.fixedDeltaTime * speed);    
    }

    
    protected virtual void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Wall"))
        {
            if(objectIsPooled)
            {
                DisableObject();
            }else
            {
                Destroy(gameObject);
            }
            return;
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            HealthMeter targetHealth = collision.gameObject.GetComponent<HealthMeter>();
            if(targetHealth)
            {
                targetHealth.Hurt(damagePayload.baseDamage);

                if(objectIsPooled)
                {
                    DisableObject();
                }else
                {
                    Destroy(gameObject);
                }
            }
        }

    }



    protected IEnumerator SelfDestruct(float delay = 2)
    {
        yield return new WaitForSeconds(delay);

        if(objectIsPooled)
        {
            DisableObject();
        }else
        {
            Destroy(gameObject);
        }

    }

    public void ActivateObject()
    {
        gameObject.SetActive(true);

        if (trailRenderer) 
        {
            // Clear the trail
            float originalTime = trailRenderer.time;
            trailRenderer.time = 0;
            trailRenderer.Clear();
            trailRenderer.time = originalTime;

            trailRenderer.enabled = true;
            trailRenderer.emitting = true; 
        }        

        StartCoroutine(SelfDestruct(lifetime));
        OnActivateObject?.Invoke(gameObject);
    }

    public void DisableObject()
    {
        if(CR_SelfDestruct != null)
        {
            StopCoroutine(CR_SelfDestruct);
        }

        if (trailRenderer) 
        { 
            trailRenderer.emitting = false;
            trailRenderer.enabled = false;
        }

        gameObject.SetActive(false);
        OnDisableObject?.Invoke(gameObject);
    }
}
