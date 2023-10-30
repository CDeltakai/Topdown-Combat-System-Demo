using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BeerBottle : Bullet
{
    [SerializeField] SphereCollider explosionRadius;

    [SerializeField] int particleEffectDuration = 3;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask obstacleLayer;

    [SerializeField] GameObject explosionParticleEffect;

    CinemachineImpulseSource impulseSource;
    CapsuleCollider beerCollider;
    Animator spinAnimator;
    MeshRenderer meshRenderer;


    protected override void Awake()
    {
        base.Awake();
        spinAnimator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        beerCollider = GetComponent<CapsuleCollider>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }


    protected override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") )
        {

            ExplodeHits(explosionRadius.transform.position);
            StartCoroutine(ExplosionParticleTimer(particleEffectDuration));

            return;
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            if(entity)
            {
                ExplodeHits(explosionRadius.transform.position);
                StartCoroutine(ExplosionParticleTimer(particleEffectDuration));

            }
        }
    }

    void ExplodeHits(Vector3 explosionPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(explosionPosition, explosionRadius.radius, targetLayer);

        foreach(var hitCollder in hitColliders)
        {
            HealthMeter targetHealth = hitCollder.GetComponent<HealthMeter>();
            if(targetHealth && targetHealth.CompareTag("Enemy"))
            {
                targetHealth.Hurt(damagePayload.baseDamage);
            }
        }
    }


    IEnumerator ExplosionParticleTimer(float duration)
    {
        beerCollider.enabled = false;
        spinAnimator.enabled = false;
        transform.rotation = Quaternion.identity;

        explosionParticleEffect.SetActive(true);
        meshRenderer.enabled = false;
        rigBody.isKinematic = true;

        if (impulseSource) { impulseSource.GenerateImpulseWithForce(0.1f); };

        yield return new WaitForSeconds(duration);
        explosionParticleEffect.SetActive(false);

        if(objectIsPooled)
        {
            DisableObject();
            meshRenderer.enabled = true;
            rigBody.isKinematic = false;
            spinAnimator.enabled = true;           
            beerCollider.enabled = true;

        }else
        {
            Destroy(gameObject);
        }
    }


}
