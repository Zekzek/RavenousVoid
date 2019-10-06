using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    public int xPos;
    public int zPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Genesis.CheckAndGenerateNear(xPos, zPos);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Genesis.CheckAndGenerateNear(xPos, zPos);
        }
    }

}
