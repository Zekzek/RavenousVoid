using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    new public Light light;
    public Flicker[] flickers;

    private int flickerIndex = 0;
    private float timeToNextFlicker;
    private float timeToFlickerCompletion;

    // Update is called once per frame
    void Update()
    {
        if (timeToFlickerCompletion <= 0)
        {
            light.enabled = true;
            ReadyNextFlicker();
        }
        else if (timeToNextFlicker <= 0)
        {
            light.enabled = false;
            timeToFlickerCompletion -= Time.deltaTime;
        }
        else
        {
            timeToNextFlicker -= Time.deltaTime;
        }
    }

    private void ReadyNextFlicker()
    {
        flickerIndex = Random.Range(0, flickers.Length);
        timeToNextFlicker = flickers[flickerIndex].Delay;
        timeToFlickerCompletion = flickers[flickerIndex].Duration;
    }

    [System.Serializable]
    public class Flicker
    {
        public float minDelay;
        public float maxDelay;
        public float minDuration;
        public float maxDuration;

        public float Delay { get { return Random.Range(minDelay, maxDelay); } }
        public float Duration { get { return Random.Range(minDuration, maxDuration); } }
    }
}
