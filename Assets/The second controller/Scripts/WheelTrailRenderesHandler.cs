using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderesHandler : MonoBehaviour
{
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        //Get the top down car controller
        topDownCarController = GetComponentInParent<TopDownCarController>();

        //Get the trail renderer
        trailRenderer = GetComponent<TrailRenderer>();

        //Set the trail renderer to not emit in the start
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
