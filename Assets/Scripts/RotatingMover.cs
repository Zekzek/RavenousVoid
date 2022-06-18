using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMover : MonoBehaviour
{
    new public Rigidbody rigidbody;
    public float speed;
    public float rotationSpeed;

    void Start()
    {
        rigidbody.freezeRotation = true;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (HudDisplay.MovementAllowed)
        {
            float vertical = GetAxisWithDeadZone("Vertical", 0.05f) * speed;
            float horizontal = GetAxisWithDeadZone("Horizontal", 0.05f) * rotationSpeed;

            if (Mathf.Abs(horizontal) > 0.01f)
                rigidbody.AddRelativeTorque(new Vector3(0, horizontal, 0), ForceMode.Acceleration);

            if (Mathf.Abs(vertical) > 0.01f)
                rigidbody.AddRelativeForce(new Vector3(0, 0, vertical), ForceMode.Acceleration);
        }
    }

    private float GetAxisWithDeadZone(string axisName, float deadzone)
    {
        float axisValue = Input.GetAxis(axisName);
        return axisValue < deadzone && axisValue > -deadzone ? 0 : axisValue;
    }
}
