using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluffy : MonoBehaviour
{
    void Start()
    {
        Transform myTransform = this.gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time / 3, 0.3f), transform.position.z);
        //transform.position = new Vector3(transform.position.x, transform.position.y* Mathf.PingPong(Time.time / 3, 0.3f)), transform.position.z);
    }
}
