using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vines : MonoBehaviour
{
	[SerializeField] UnityEvent onReachedTop;
	public Transform toMove;
	[SerializeField] Vector3 startPosition, endPosition;
	[SerializeField] float speed = 1;

	Coroutine isMoving;
	List<Root> roots = new List<Root>();

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log(collision.transform.name);
	}

	public void AddRoot(Root root)
	{
		roots.Add(root);
		StartMovingUp();
	}

	public void RemoveRoot(Root root)
	{
		roots.Remove(root);
		if (roots.Count == 0)
		{
			StartMovingDown();
		}
	}

	public void StartMovingUp()
	{
		if (isMoving != null)
			StopCoroutine(isMoving);

		isMoving = StartCoroutine(MoveUp());
	}

	public void StartMovingDown()
	{
		if (isMoving != null)
			StopCoroutine(isMoving);

		isMoving = StartCoroutine(MoveDown());
	}

	IEnumerator MoveUp()
	{
		while ((toMove.position - endPosition).sqrMagnitude > 1)
		{
			yield return null;
			toMove.position = Vector3.MoveTowards(toMove.position, endPosition, speed * Time.deltaTime);
		}
		onReachedTop.Invoke();
	}
	IEnumerator MoveDown()
	{
		while ((toMove.position - startPosition).sqrMagnitude > 1)
		{
			yield return null;
			toMove.position = Vector3.Lerp(toMove.position, startPosition, speed * Time.deltaTime);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!Application.isPlaying)
			startPosition = toMove.position;
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(startPosition, .5f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(endPosition, .5f);
	}
}
