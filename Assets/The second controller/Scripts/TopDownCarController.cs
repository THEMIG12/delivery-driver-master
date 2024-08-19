using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float accelerationFactor = 30f;
    public float driftFactor = 0.95f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20f;

    [Header("Sprites")]
    public SpriteRenderer carSpriteRenderer;
    public SpriteRenderer carShadowRenderer;

    [Header("Jumping")]
    public AnimationCurve jumpCurve;

    //local variables
    public float accelerationInput = 0;
    public float steeringInput = 0;
    public float rotationAngle = 0;
    public float velocityVsUp = 0;
    public bool isJumping = false;

    //Components
    public Rigidbody2D rb;
    Collider2D carCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        carCollider = GetComponent<Collider2D>();
    }
    void FixedUpdate()
    {
        ApplayingForce();
        KillOrthogonalVelocity();
        ApplayingSteering();
    }
    void ApplayingForce()
    {
        //Calculate how much forward we are going in terms of the direction of our velocity
        velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

        //limit so we cannot go faster then the max speed in the forward direction
        if (velocityVsUp > maxSpeed && accelerationInput > 0) return;

        //limit so we cannot go faster then the 50% of max speed in the reverse direction
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) return;

        //limit so we cannot go faster in any direction while accelerating
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0) return;

        //Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
        if (accelerationInput == 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            rb.drag = 0;
        }
        //Create a force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        //Apply force and pushes the car forward
        rb.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplayingSteering()
    {
        //Limit the car ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = rb.velocity.magnitude / 8;
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //Update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        //Apply steering by rotating the car object
        rb.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public float GetLateralVelocity()
    {
        //return how much fast the car is moving sideways
        return Vector2.Dot(transform.right, rb.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        //Check if we are moving forward and if the player is hitting the brakes. In that case the tries should screech
        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }
        //If we have a lot of side movment then the tires should be screeching
        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
            return true;
        return false;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude()
    {
        return rb.velocity.magnitude;
    }

    public void Jump(float jumpHeightScale, float jumpPushScale)
    {
        if (!isJumping)
            StartCoroutine(JumpCo(jumpHeightScale, jumpPushScale));
    }

    private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale)
    {
        isJumping = true;

        float jumpStartTime = Time.time;
        float jumpDuration = GetVelocityMagnitude() * 0.05f;

        jumpHeightScale = jumpHeightScale * GetVelocityMagnitude() * 0.05f;
        jumpHeightScale = Mathf.Clamp(jumpHeightScale, 0.0f, 1.0f);

        carCollider.enabled = false;

        //Push the object forward as we passed a jump
        rb.AddForce(rb.velocity.normalized * jumpPushScale * 10, ForceMode2D.Impulse);

        while (isJumping)
        {
            //Percentage 0 - 1.0 of where we are in the jumping process
            float jumpCompletedPercentage = (Time.time - jumpStartTime) / jumpDuration;
            jumpCompletedPercentage = Mathf.Clamp01(jumpCompletedPercentage);

            //take the base scale of 1 and add how much we should increase the scale with
            carSpriteRenderer.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            //Change the shadow scale also but make it a bit smaller. in the real world this should be the opposite, the higher an object gets the larger its shadow gets but this is a game
            carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale * 0.75f;

            //offset the shadow a bit this is not 100% correct either but works good enough in our game
            carShadowRenderer.transform.localPosition = new Vector3(1, -1, 0) * 3 * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            //when we reach 100% we are done
            if (jumpCompletedPercentage == 1.0f)
            {
                break;
            }

            yield return null;
        }

        //Check if landing is ok or not
        if (Physics2D.OverlapCircle(transform.position, 1.5f))
        {
            //Something is below the car so we need to jump again
            isJumping = false;

            //add a small jump and push the car forward a bit
            Jump(0.2f, 0.6f);
        }
        else
        {
            //Handle landing scale back the object
            carSpriteRenderer.transform.localScale = Vector3.one;

            //reset the shadow position and scale
            carShadowRenderer.transform.localPosition = Vector3.zero;
            carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale;

            //we are safe to land
            carCollider.enabled = true;

            isJumping = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Jump"))
        {
            JumpData jumpData = collider2D.GetComponent<JumpData>();
            Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
        }

    }
}
