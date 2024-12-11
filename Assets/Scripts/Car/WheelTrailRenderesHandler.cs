using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderesHandler : MonoBehaviour
{
    public bool isOverpassEmitter = false;
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;
    CarLayerHandler carLayerHandler;
    // Start is called before the first frame update
    void Awake()
    {
        //Get the top down car controller
        topDownCarController = GetComponentInParent<TopDownCarController>();

        carLayerHandler = GetComponentInParent<CarLayerHandler>();
        //Get the trail renderer
        trailRenderer = GetComponent<TrailRenderer>();

        //Set the trail renderer to not emit in the start
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        trailRenderer.emitting = false;
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (carLayerHandler.IsDrivingON() && isOverpassEmitter)
                trailRenderer.emitting = true;
            if (!carLayerHandler.IsDrivingON() && !isOverpassEmitter)
                trailRenderer.emitting = true;
        }
    }
}
