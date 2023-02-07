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

	BoxCollider2D movingCollider;
	Vector2 colliderOffset, colliderSize;
	float distance;

	private void Start()
	{
		movingCollider = toMove.GetComponent<BoxCollider2D>();
		colliderOffset = movingCollider.offset;
		colliderSize = movingCollider.size;
		distance = (startPosition - endPosition).magnitude;
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
		if (gameObject.activeInHierarchy)
			isMoving = StartCoroutine(MoveUp());
	}

	public void StartMovingDown()
	{
		if (isMoving != null)
			StopCoroutine(isMoving);
		if (gameObject.activeInHierarchy)
			isMoving = StartCoroutine(MoveDown());
	}

	IEnumerator MoveUp()
	{
		Vector2 delta = toMove.position - startPosition;
		float mag = distance - delta.magnitude;
		while (mag > .01f)
		{
			delta = toMove.position - startPosition;
			mag = distance - delta.magnitude;
			yield return null;
			float t = mag / distance;
			toMove.position = Vector3.MoveTowards(toMove.position, endPosition, speed * Time.deltaTime);
			movingCollider.size = Vector2.Lerp(Vector2.zero, colliderSize, t);
			movingCollider.offset = colliderOffset - delta * .5f;
		}
		onReachedTop.Invoke();
	}
	IEnumerator MoveDown()
	{
		Vector2 delta = toMove.position - startPosition;
		float mag = delta.magnitude;
		while (mag > .01f)
		{
			delta = toMove.position - startPosition;
			mag = delta.magnitude;
			yield return null;
			float t = mag / distance;
			toMove.position = Vector3.Lerp(toMove.position, startPosition, speed * Time.deltaTime);
			movingCollider.size = Vector2.Lerp(Vector2.zero, colliderSize, 1 - t);
			movingCollider.offset = colliderOffset - delta * .5f;
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
