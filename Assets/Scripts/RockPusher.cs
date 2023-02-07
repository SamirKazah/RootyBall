using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RockPusher : MonoBehaviour
{
	[SerializeField] GameObject trigger;
	[SerializeField] float addedForce = 100;
	[SerializeField] UnityEvent onAddForce;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var r = collision.transform.GetComponent<Rigidbody2D>();
		if(r)
			r.AddForce(Vector2.up * addedForce, ForceMode2D.Impulse);
		foreach (Transform child in transform)
		{
			var rigid = child.GetComponent<Rigidbody2D>();
			if (rigid)
			{
				rigid.isKinematic = false;
				rigid.AddForce(Vector2.up * addedForce, ForceMode2D.Impulse);
			}
		}
		onAddForce.Invoke();
	}

	public void EnableTrigger()
	{
		StartCoroutine(Trigger());
	}

	IEnumerator Trigger()
	{
		yield return new WaitForSeconds(1);
		trigger.SetActive(true);
	}
}
