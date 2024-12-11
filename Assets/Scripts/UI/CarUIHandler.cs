using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class CarUIHandler : MonoBehaviour
{
    public Image carImage;
    Animator animator = null;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        carImage = GetComponentInChildren<Image>();
    }

    public void SetupCar(CarData carData)
    {
        carImage.sprite = carData.CarUISprite;
    }

    public void StartCarEnterAnimation(bool isAppearingOnTheRightSide)
    {
        if (isAppearingOnTheRightSide)
            animator.Play("Car Slide in from the right");
        else
            animator.Play("Car Slide in from the left");
    }
    public void StartCarExitAnimation(bool isExitOnTheRightSide)
    {
        if (isExitOnTheRightSide)
            animator.Play("Car Slide out to the right");
        else
            animator.Play("Car Slide out to the left");
    }
    public void onCarExitAnimationCompleted()
    {
        Destroy(gameObject);
    }
}
