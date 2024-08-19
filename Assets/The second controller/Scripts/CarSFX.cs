using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using Unity.Mathematics;
using UnityEngine;

public class CarSFX : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource tiresScreechingAudioSources;
    public AudioSource engineAudioSources;
    public AudioSource carHitAudioSources;

    //local variables
    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;

    //Component
    TopDownCarController topDownCarController;

    void Start()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
    }

    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechSFX();
    }

    void UpdateEngineSFX()
    {
        //Handle engine SFX
        float velocityMagnitude = topDownCarController.GetVelocityMagnitude();

        //Increase the engine volume as the car goes faster
        float desiredEngineVolume = velocityMagnitude * 0.05f;

        //But keep a minimum level so ot playes even if the car is idle
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1f);

        engineAudioSources.volume = Mathf.Lerp(engineAudioSources.volume, desiredEngineVolume, Time.deltaTime * 10);

        //to add more variation to the engine sound we also change the pitch
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudioSources.pitch = Mathf.Lerp(engineAudioSources.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }
    void UpdateTiresScreechSFX()
    {
        //Handle tire screeching SFX
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            //if the car is braking we want the tire screech to be louder and also change the pitch
            if (isBraking)
            {
                tiresScreechingAudioSources.volume = Mathf.Lerp(tiresScreechingAudioSources.volume, 1f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                //If we are not braking we still want to play this screech sound if the player is drifting
                tiresScreechingAudioSources.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        //Fade out the tire screech SFX if we are not screeching
        else tiresScreechingAudioSources.volume = Mathf.Lerp(tiresScreechingAudioSources.volume, 0f, Time.deltaTime * 10);
    }


    void OnCollisionEnter2D(Collision2D collision2D)
    {
        //Get the relative velocity of the Collision
        float relativeVelocity = collision2D.relativeVelocity.magnitude;

        float volume = relativeVelocity * 0.1f;

        carHitAudioSources.pitch = Random.Range(0.95f, 1.05f);
        carHitAudioSources.volume = volume;

        if (!carHitAudioSources.isPlaying)
            carHitAudioSources.Play();
    }
}
