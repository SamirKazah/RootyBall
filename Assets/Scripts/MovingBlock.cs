using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] Vector3 startPosition, endPosition;
    [SerializeField] float timeNeeded = 2;

    void Start()
    {
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        float t = 0;
        while (t < timeNeeded)
        {
            t += Time.deltaTime;
            yield return null;
            transform.position = Vector3.Lerp(startPosition, endPosition, t / timeNeeded);
        }
        yield return new WaitForSeconds(1);
        t = 0;
        while (t < timeNeeded)
        {
            t += Time.deltaTime;
            yield return null;
            transform.position = Vector3.Lerp(endPosition, startPosition, t / timeNeeded);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(MoveObject());
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            startPosition = transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, .5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPosition, .5f);
    }
}
