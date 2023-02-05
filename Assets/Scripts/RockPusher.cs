using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPusher : MonoBehaviour
{
	[SerializeField] GameObject trigger;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var r = collision.transform.GetComponent<Rigidbody2D>();
		if(r)
			r.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
		foreach (Transform child in transform)
		{
			var rigid = child.GetComponent<Rigidbody2D>();
			if (rigid)
			{
				rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
			}
		}
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
