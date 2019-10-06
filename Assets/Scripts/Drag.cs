using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    new public Rigidbody rigidbody;
    public float dragPercent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 xzVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        if (xzVelocity.sqrMagnitude > 0.001f)
            rigidbody.AddForce(xzVelocity * -dragPercent, ForceMode.Acceleration);
    }
}
