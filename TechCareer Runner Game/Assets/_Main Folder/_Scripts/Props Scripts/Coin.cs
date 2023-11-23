using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool moving = false;
    float move_speed = 25;

    void Start()
    {
        moving = true;
    }

    void Update()
    {
        if (moving)
        {
            transform.position += new Vector3(0, 0, move_speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            moving = false;
            move_speed = 0f;
        }
    }
}
