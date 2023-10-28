using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Bullet
{

    public Transform shooter;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float maxFlightTime = 3f;
    [SerializeField] float returnCurveHeight = 5f;


    enum State{Idle, Fired, Returning}
    State currentState = State.Fired;

    float t = 0; // Parameter for curve

    Vector3 startPosition;
    Vector3 endPoint;
    Vector3 controlPoint;

    float currentFlightTime = 0;

    protected override void FixedUpdate()
    {
        if(currentState == State.Fired)
        {
            //rigBody.MovePosition(rigBody.position + velocity * Time.fixedDeltaTime * speed);    
            //rigBody.velocity = transform.forward * speed;
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
        print("start returning");
        rigBody.velocity = -rigBody.velocity;
        currentState = State.Returning;
        t = 0;
        controlPoint = (startPosition + endPoint) / 2 + Vector3.up * returnCurveHeight;
        currentFlightTime = 0;
        GetComponent<Collider>().enabled = false; // Disable the collider        
    }


    protected override void AdditionalActivationOperations()
    {
        currentState = State.Fired;
        startPosition = transform.position;
        endPoint = shooter.position;        
    }



}
