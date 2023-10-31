using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Bullet
{

    public delegate void ReturnedEventHandler();
    public event ReturnedEventHandler OnReturned;

    public Transform shooter;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float maxFlightTime = 3f;

    enum State{ Fired, Returning}
    State currentState = State.Fired;


    float currentFlightTime = 0;

    protected override void FixedUpdate()
    {
        if(currentState == State.Fired)
        {
            currentFlightTime += Time.fixedDeltaTime;

            if (Vector3.Distance(shooter.position, transform.position) >= maxDistance || currentFlightTime >= maxFlightTime)
            {
                StartReturning();
            }
        }
        else if (currentState == State.Returning)
        {
            Vector3 directionToShooter = (shooter.position - transform.position).normalized;
            rigBody.velocity = directionToShooter * speed;
            if(Vector3.Distance(transform.position, shooter.position) <= 1f)
            {
                DisableObject();
            }
        }

    }


    protected override void OnCollisionEnter(Collision collision)
    {
        if (currentState == State.Fired && (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall")))
        {
            StartReturning();
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            HealthMeter targetHealth = collision.gameObject.GetComponent<HealthMeter>();
            targetHealth.Hurt(damagePayload);
        }

    }


    void StartReturning()
    {
        currentState = State.Returning;
        currentFlightTime = 0;
        GetComponent<Collider>().enabled = false; // Disable the collider on returning to prevent it getting stuck on things      
    }


    protected override void AdditionalActivationOperations()
    {
        GetComponent<Collider>().enabled = true;        
        currentState = State.Fired;    
    }

    protected override void AdditionalDisableOperations()
    {

        OnReturned?.Invoke();
        var invocationList = OnReturned?.GetInvocationList();
        if (invocationList != null)
        {
            foreach (var handler in invocationList)
            {
                OnReturned -= (ReturnedEventHandler)handler;
            }
        }        

        GetComponent<Collider>().enabled = true;        
        currentState = State.Fired;
    }


}
