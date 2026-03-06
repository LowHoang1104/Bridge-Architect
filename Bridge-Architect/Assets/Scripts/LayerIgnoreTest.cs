using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerIgnoreTest : MonoBehaviour
{
    void Awake()
    {
        int cars = LayerMask.NameToLayer("Cars");
        int supports = LayerMask.NameToLayer("Supports");
        Physics2D.IgnoreLayerCollision(cars, supports, true);
    }
}
