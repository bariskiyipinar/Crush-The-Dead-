using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffectMove : MonoBehaviour
{
    [SerializeField] private float speed = 2f;



    private void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }
}
