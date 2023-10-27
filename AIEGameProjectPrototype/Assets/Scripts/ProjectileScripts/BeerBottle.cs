using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeerBottle : Bullet
{
    [SerializeField] SphereCollider explosionRadius;

    [SerializeField] int particleEffectDuration = 3;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask obstacleLayer;

    [SerializeField] GameObject explosionParticleEffect;

    CapsuleCollider beerCollider;
    Animator spinAnimator;
    MeshRenderer meshRenderer;


    protected override void Awake()
    {
        base.Awake();
        spinAnimator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        beerCollider = GetComponent<CapsuleCollider>();
    }


    protected override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
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
            Entity entity = hitCollder.GetComponent<Entity>();
            if(entity && entity.CompareTag("Enemy"))
            {
                entity.HurtEntity(damagePayload.baseDamage);
            }
        }
    }


    void OnEnable()
    {
       
    }


    IEnumerator ExplosionParticleTimer(float duration)
    {
        beerCollider.enabled = false;
        spinAnimator.enabled = false;
        transform.rotation = Quaternion.identity;

        explosionParticleEffect.SetActive(true);
        meshRenderer.enabled = false;
        rigBody.isKinematic = true;


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
