using JetBrains.Annotations;
using UnityEngine;

public class CarUIInputHandler : MonoBehaviour
{
    public CarInputHandler playerCarInputHandler;
    public CarInputHandler[] carInputHandlers;
    Vector2 inputVector = Vector2.zero;

    void Awake()
    {
        carInputHandlers = FindObjectsOfType<CarInputHandler>();

        foreach (CarInputHandler carInputHandler in carInputHandlers)
        {
            if (carInputHandler.isUIInput)
            {
                playerCarInputHandler = carInputHandler;
                break;
            }
        }
    }

    public void onAcceleratePress()
    {
        inputVector.y = 1.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
    public void onBrakePress()
    {
        inputVector.y = -1.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
    public void onAccelerateBrakeRelease()
    {
        inputVector.y = 0.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
    public void onSteerLeftPress()
    {
        inputVector.x = -1.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
    public void onSteerRightPress()
    {
        inputVector.x = 1.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
    public void onSteerRelease()
    {
        inputVector.x = 0.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
}
