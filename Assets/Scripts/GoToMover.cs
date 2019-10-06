using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtiliZek;

public class GoToMover : MonoBehaviour
{
    new public Rigidbody rigidbody;
    public float speed;
    protected float sprintingModifier = 1f;
    public Transform target;
    public float preferredSqrRange;
    public bool sprintToCatchUp;

    public void SpeedUp()
    {
        sprintingModifier *= 1.25f;
    }

    public void SlowDown()
    {
        sprintingModifier *= 0.8f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = other.transform;
            Colorize colorize = gameObject.GetComponent<Colorize>();
            if (colorize != null)
                colorize.LightUp = true;
        }
    }

    void Update()
    {
        rigidbody.AddForce(MoveForce, ForceMode.Acceleration);
    }

    private Vector3 MoveForce
    {
        get
        {
            //Vector2 input = MoverInput;
            //Vector3 force = new Vector3(input.x, 0, input.y);
            return MoverInput * speed * sprintingModifier;
        }
    }

    protected Vector3 MoverInput
    {
        get
        {
            if (target == null)
                return Vector2.zero;
            else
            {
                Vector3 relativePosition = target.position - transform.position;

                if (relativePosition.sqrMagnitude < preferredSqrRange)
                    return Vector2.zero;
                else
                {
                    sprintingModifier = sprintToCatchUp ? Mathf.Clamp(relativePosition.sqrMagnitude / 10, 1f, 30f) : 1f;
                    relativePosition.Normalize();
                    return relativePosition;
                    return new Vector2(relativePosition.x, relativePosition.z);
                }
            }
        }
    }
}
