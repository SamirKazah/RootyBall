using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Wind : MonoBehaviour
{
    float gravity;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rigidB = collision.GetComponent<Rigidbody2D>();
        gravity = rigidB.gravityScale;
        rigidB.gravityScale = -gravity;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var rigidB = other.GetComponent<Rigidbody2D>();
        rigidB.gravityScale = gravity;
    }
}
