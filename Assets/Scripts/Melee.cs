using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float duration = 1f;
    public int damage = 50;

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(duration);
        // Disables the game object after duration
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            // enemy.Health();
            enemy.DealDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
