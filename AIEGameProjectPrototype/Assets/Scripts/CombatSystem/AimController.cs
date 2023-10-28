using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Aiming system designed for top-down perspective. Rotates the object it is attached to
/// towards the mouse position. Also supports a reticle object if given one.
/// </summary>
public class AimController : MonoBehaviour
{
    [SerializeField] Camera cameraPosition;
    [SerializeField] GameObject reticle;

[Tooltip("The speed at which the object rotates towards the mouse.")]    
    [SerializeField] float rotationSpeed = 7;
    [SerializeField] Transform aimpointTransform;



    void Update()
    {
        RotateTowardsMouse();
        PositionReticle();
    }

    void RotateTowardsMouse()
    {
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, aimpointTransform.position);

        Ray ray = cameraPosition.ScreenPointToRay(Mouse.current.position.ReadValue());

        // Determine the point where the cursor ray intersects the plane.
        float hitdist;
        if (playerPlane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);

            // Determine the target rotation. This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - aimpointTransform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void PositionReticle()
    {
        if(!reticle) { return; }
        if(!aimpointTransform) { return; }

        Plane firePointPlane = new Plane(Vector3.up, aimpointTransform.position);
        Ray ray = cameraPosition.ScreenPointToRay(Mouse.current.position.ReadValue());

        float hitdist;
        if (firePointPlane.Raycast(ray, out hitdist))
        {
            Vector3 reticlePosition = ray.GetPoint(hitdist);

            if(aimpointTransform != null)
            {
                reticlePosition.y = aimpointTransform.position.y;
            }

            reticle.transform.position = reticlePosition; // Position the reticle at the hit point
        }        
    }


}
