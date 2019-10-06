using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private GoToMover goToMover;

    public void SpeedUp()
    {
        goToMover.SpeedUp();
    }

    public void SlowDown()
    {
        goToMover.SlowDown();
    }

    private void Start()
    {
        goToMover = GetComponent<GoToMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 target = other.transform.position;
            transform.LookAt(new Vector3(target.x, 0, target.z));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 target = other.transform.position;
            transform.LookAt(new Vector3(target.x, 0, target.z));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        goToMover.target = null;
        Debug.Log("OnTriggerExit");
        Vector3 target = other.transform.position;
        transform.LookAt(new Vector3(target.x, 0, target.z));
    }
}
