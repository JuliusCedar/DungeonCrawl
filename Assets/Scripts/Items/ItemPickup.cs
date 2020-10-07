    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public AudioSource pickupSound;

    private void Update()
    {
        if (item == null || item.GetCount() <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void playPickup()
    {
        AudioSource.PlayClipAtPoint(pickupSound.clip, gameObject.transform.position, 1);
    }
}