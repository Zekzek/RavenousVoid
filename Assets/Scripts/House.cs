using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public GameObject[] controls;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ShowControls(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ShowControls(true);

            if (HudDisplay.Progressable)
            {
                HudDisplay.ShowProgress();
            }
        }
    }

    private void ShowControls(bool value)
    {
        foreach (GameObject control in controls)
            control.SetActive(value);
    }
}
