using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarLayerHandler : MonoBehaviour
{
    List<SpriteRenderer> defaultLayerSpriteRenderers = new List<SpriteRenderer>();
    //State
    bool isDrivingON = false;

    List<Collider2D> overpassColliderList = new List<Collider2D>();

    List<Collider2D> underpassColliderList = new List<Collider2D>();

    Collider2D carCollider;



    void Awake()
    {
        foreach (SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            if (spriteRenderer.sortingLayerName == "Car")
                defaultLayerSpriteRenderers.Add(spriteRenderer);
        }

        foreach (GameObject overpassColliderGameObject in GameObject.FindGameObjectsWithTag("PassOverCollider"))
        {
            overpassColliderList.Add(overpassColliderGameObject.GetComponent<Collider2D>());
        }
        foreach (GameObject underpassColliderGameObject in GameObject.FindGameObjectsWithTag("PassUnderCollider"))
        {
            underpassColliderList.Add(underpassColliderGameObject.GetComponent<Collider2D>());
        }
        carCollider = GetComponentInChildren<Collider2D>();
    }

    void Start()
    {
        UpdateSortingAndCollisionLayer();
    }

    void UpdateSortingAndCollisionLayer()
    {
        if (isDrivingON)
        {
            SetSortingLayer("Bridge");
        }
        else
        {
            SetSortingLayer("Car");
        }
        SetCollisionWithOverPass();
    }

    void SetCollisionWithOverPass()
    {
        foreach (Collider2D collider2D in overpassColliderList)
        {
            Physics2D.IgnoreCollision(carCollider, collider2D, !isDrivingON);
        }

        foreach (Collider2D collider2D in underpassColliderList)
        {
            if (isDrivingON)
                Physics2D.IgnoreCollision(carCollider, collider2D, true);
            else Physics2D.IgnoreCollision(carCollider, collider2D, false);
        }
    }

    void SetSortingLayer(string layerName)
    {
        foreach (SpriteRenderer spriteRenderer in defaultLayerSpriteRenderers)
        {
            spriteRenderer.sortingLayerName = layerName;
        }
    }

    public bool IsDrivingON()
    {
        return isDrivingON;
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("PassUnder"))
        {
            //Debug.Log("PassUnder");
            isDrivingON = false;
            UpdateSortingAndCollisionLayer();
        }
        else if (collider2D.CompareTag("PassOver"))
        {
            //Debug.Log("PassOver");
            isDrivingON = true;
            UpdateSortingAndCollisionLayer();
        }
    }
}
