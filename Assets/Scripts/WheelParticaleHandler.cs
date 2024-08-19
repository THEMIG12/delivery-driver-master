using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticaleHandler : MonoBehaviour
{
    float particleEmissionRate = 0;
    TopDownCarController topDownCarController;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;

    void Awake()
    {
        //Get the TopDownCarController
        topDownCarController = GetComponentInParent<TopDownCarController>();
        //Get the ParticleSystem
        particleSystemSmoke = GetComponent<ParticleSystem>();
        //Get the EmissionModule
        particleSystemEmissionModule = particleSystemSmoke.emission;
        //Set it to zero emission
        particleSystemEmissionModule.rateOverTime = 0;
    }

    void Update()
    {
        //Reduce the particle System over time
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            //If the car tires are screeching then we'll emitt smoke. if the player is braking then emitt a lot of smoke. 
            if (isBraking) particleEmissionRate = 30;
            //If the player is drifting we'll emitt smoke based on how much the player is drifting.
            else particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
        }
    }
}