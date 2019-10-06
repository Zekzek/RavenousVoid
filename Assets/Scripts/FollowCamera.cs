using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float rotateSpeed;

    private float horizontalDistance;

    // Start is called before the first frame update
    void Start()
    {
        horizontalDistance = new Vector3(offset.x, 0, offset.z).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Rotate(horizontal * Time.deltaTime * rotateSpeed);

        transform.position = target.transform.position + offset;
    }

    private void Rotate(float amount)
    {
        offset = Quaternion.Euler(0, amount, 0) * offset;
    }

    protected virtual Vector2 MoverInput
    {
        get
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(horizontal, vertical);
            if (movement.SqrMagnitude() < 0.1f)
                return Vector2.zero;
            else
                return movement.normalized * Mathf.Min(1f, movement.magnitude);
        }
    }
}
