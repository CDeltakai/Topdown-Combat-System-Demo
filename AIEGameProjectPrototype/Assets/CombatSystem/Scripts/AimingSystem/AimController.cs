using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Aiming system designed for top-down perspective. Rotates the object it is attached to
/// towards the mouse/pointer position. Also supports a reticle object if given one.
/// </summary>
public class AimController : MonoBehaviour
{
    [SerializeField] Camera cameraPosition;
    [SerializeField] GameObject reticle;

[Tooltip("The speed at which the object rotates towards the mouse.")]    
    [SerializeField] float rotationSpeed = 7;

[Tooltip("The transform that dictates the hieght at which the reticle will be placed. Should normally coincide with the firing point of the weapon of the player.")]
    [SerializeField] Transform referenceTransform;

[Tooltip("The max X distance the reticle object can be from the reference transform.")]    
    [Min(0)]
    [SerializeField] float clampXDistance = 20;

[Tooltip("The max Z distance the reticle object can be from the reference transform.")]    
    [Min(0)]
    [SerializeField] float clampZDistance = 20;

    [SerializeField] float reticleSensitivity = 1;

    public bool CursorIsVisible;
    public bool reticleIsVisible;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        RotateTowardsMouse();
        PositionReticle();

        if(CursorIsVisible)
        {
            HideCursor(false);
        }else
        {
            HideCursor(true);
        }

        if(reticleIsVisible)
        {
            reticle.SetActive(true);
        }else
        {
            reticle.SetActive(false);
        }

    }

    void HideCursor(bool condition)
    {
        if(condition)
        {
            Cursor.visible = false;
        }else
        {
            Cursor.visible = true;
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 targetPoint = Vector3.zero;

        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, referenceTransform.position);

        // Handle touch or mouse input
        if (Pointer.current != null)
        {
            Ray ray = cameraPosition.ScreenPointToRay(Pointer.current.position.ReadValue());
            float hitdist;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                targetPoint = ray.GetPoint(hitdist);
            }
        }

        // Determine the target rotation. This is the rotation if the transform looks at the target point.
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - referenceTransform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    void PositionReticle()
    {
        if (!reticle || !referenceTransform) return;

        Plane firePointPlane = new Plane(Vector3.up, referenceTransform.position);
        Ray ray;
        Vector3 hitPosition = Vector3.zero;

        // Handle touch or mouse input
        if (Pointer.current != null)
        {
            ray = cameraPosition.ScreenPointToRay(Pointer.current.position.ReadValue());
            float hitdist;
            if (firePointPlane.Raycast(ray, out hitdist))
            {
                hitPosition = ray.GetPoint(hitdist);
            }
        }

        Vector3 offset = hitPosition - referenceTransform.position;
        offset *= reticleSensitivity;

        // Clamps the position of the reticle to a certain distance from the reference transform
        // Clamp each component of the offset separately to create a rectangular clamp
        offset.x = Mathf.Clamp(offset.x, -clampXDistance, clampXDistance);
        offset.z = Mathf.Clamp(offset.z, -clampZDistance, clampZDistance);

        hitPosition = referenceTransform.position + offset;

        if (referenceTransform != null)
        {
            hitPosition.y = referenceTransform.position.y;
        }

        reticle.transform.position = hitPosition; // Position the reticle at the hit point
    }


}
