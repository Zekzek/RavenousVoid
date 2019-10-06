using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtiliZek;

public class ColorCube : MonoBehaviour
{
    public GoToMover goToMover;
    public Colorize colorize;
    public LineRenderer attackGraphic;
    public int powerContained;
    public int powerDrained;

    private bool following;
    private List<Shadow> attackTargets = new List<Shadow>();
    private Shadow primaryTarget;

    private float timeSinceLastAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (!following && other.gameObject.tag == "Player")
        {
            HudDisplay.AddPower(this, powerContained);
            following = true;
            transform.SetParent(other.transform);

            goToMover.target = other.transform;
            colorize.LightUp = true;
            MusicBehaviour.SpeedUp();
            return;
        }

        Shadow otherShadow = other.gameObject.GetComponent<Shadow>();
        if (following && otherShadow != null && !attackTargets.Contains(otherShadow))
        {
            attackTargets.Add(otherShadow);
            if (primaryTarget == null)
            {
                primaryTarget = otherShadow;
                otherShadow.SlowDown();
                MusicBehaviour.SpeedUp();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (following && other.gameObject.tag == "Enemy")
        {
            Shadow otherShadow = other.gameObject.GetComponent<Shadow>();
            if (attackTargets.Contains(otherShadow))
            {
                attackTargets.Remove(otherShadow);
                if (primaryTarget.Equals(otherShadow))
                {
                    primaryTarget.SpeedUp();
                    MusicBehaviour.SlowDown();
                    primaryTarget = attackTargets.Count == 0 ? null : attackTargets[0];
                }
            }
        }
    }

    private void Update()
    {
        if (following)
        {
            if (timeSinceLastAttack > 0)
                timeSinceLastAttack -= Time.deltaTime;

            if (attackTargets.Count == 0)
            {
                attackGraphic.enabled = false;
            }
            else
            {
                attackGraphic.enabled = true;
                attackGraphic.SetPosition(0, transform.position);
                attackGraphic.SetPosition(1, primaryTarget.transform.position);

                if (timeSinceLastAttack <= 0)
                {
                    timeSinceLastAttack += 1f;
                    HudDisplay.AddPower(this, -powerDrained);
                    powerContained -= powerDrained;
                    if (powerContained <= 0)
                        Destroy(gameObject);
                }
            }
        }
    }
}
