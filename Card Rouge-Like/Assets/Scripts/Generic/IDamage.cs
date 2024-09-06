using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class IDamage : MonoBehaviour
{
    private Item item;
    private bool damageDealt;

    public float itemDamage, itemKockbackForce;

    public void SetItem(Item inItem)
    {
        item = inItem;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<IHealth>() && item && other.GetComponent<Enemy>())
        {
            if (!other.GetComponent<Enemy>().enemyHit)
            {
                if (item)
                {
                    other.gameObject.GetComponent<IHealth>().SetHealth(-item.damage);
                    other.gameObject.GetComponent<Enemy>().SetKnockbackForce(item.knockbackForce);
                }
            }
        }
        else if (other.gameObject.GetComponent<IHealth>() && other.GetComponent<Enemy>())
        {
            if (!other.GetComponent<Enemy>().enemyHit)
            {
                other.gameObject.GetComponent<IHealth>().SetHealth(-itemDamage);
                other.gameObject.GetComponent<Enemy>().SetKnockbackForce(itemKockbackForce);
            }
        }
    }
}
