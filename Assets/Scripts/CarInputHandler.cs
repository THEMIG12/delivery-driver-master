using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    public int PlayerNumber = 1;
    public bool isUIInput = false;

    Vector2 inputVector = Vector2.zero;

    TopDownCarController topDownCarController;
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    void Update()
    {
        if (!isUIInput)
        {
            inputVector = Vector2.zero;

            switch (PlayerNumber)
            {
                case 1:
                    inputVector.x = Input.GetAxis("Horizontal_P1");
                    inputVector.y = Input.GetAxis("Vertical_P1");
                    break;
                case 2:
                    inputVector.x = Input.GetAxis("Horizontal_P2");
                    inputVector.y = Input.GetAxis("Vertical_P2");
                    break;
                case 3:
                    inputVector.x = Input.GetAxis("Horizontal_P3");
                    inputVector.y = Input.GetAxis("Vertical_P3");
                    break;
                case 4:
                    inputVector.x = Input.GetAxis("Horizontal_P4");
                    inputVector.y = Input.GetAxis("Vertical_P4");
                    break;
            }
        }

        topDownCarController.SetInputVector(inputVector);

        if (Input.GetButtonDown("Jump"))
        {
            topDownCarController.Jump(1.0f, 0.0f);
        }
    }

    public void SetInput(Vector2 newInput)
    {
        inputVector = newInput;
    }
}
