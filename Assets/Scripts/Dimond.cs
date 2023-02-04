using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimond : MonoBehaviour
{
    [SerializeField] GameObject deathParticl;
    private void OnDestroy()
    {
        var p = Instantiate(deathParticl, transform.position, transform.rotation);
        Destroy(p, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>().AddRoot();
        Destroy(gameObject);
    }
}
