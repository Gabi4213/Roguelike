using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFragment : MonoBehaviour
{
    public bool rotateItem = true;
    public GameObject pickUpFX;

    private void Start()
    {
        if (rotateItem)
        {
            transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only if the thing colliding is a player
        if (other.tag == "Player")
        {
            PlayerStatistics.instance.SetSoulFragments(1);

            if (pickUpFX)
            {
                Instantiate(pickUpFX, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);           
        }
    }

}
