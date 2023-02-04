using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{
	Player p;
	private void OnEnable()
	{
		p = target as Player;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Update Spring"))
		{
			var spring = p.GetComponents<SpringJoint2D>();
			for (int i = 1; i < spring.Length; i++)
			{
				SpringJoint2D s = spring[i];
				s.frequency = spring[0].frequency;
			}
		}
	}

}

#endif

public class Player : MonoBehaviour
{
	[SerializeField, Header("Root Not Reaching")] Transform rootNotReaching;
	[SerializeField] float lerpTime = .5f;
	[SerializeField] GameObject cutRootVisuals;
	Coroutine rootNotreachingAnimation;

	[SerializeField, Header("Camera")] CinemachinePathBase path;
	[SerializeField] Camera mainCamera;

	[SerializeField, Header("Roots")] int unlockedRoots = 1;
	[SerializeField] float lerpSpeed = 5;
	[SerializeField] float rootHookDistance = 5;
	[SerializeField] Root[] springJoint2Ds;
	[SerializeField, Range(0, 1)] float pullDistance = .8f;
	[SerializeField] Transform particleHit, rootHit;

	// Start is called before the first frame update
	void Start()
	{
		foreach (var spring in springJoint2Ds)
		{
			DisableRoot(spring.root, true);
		}
		rootNotReaching.gameObject.SetActive(false);
	}

	void UnReachableArea(RaycastHit2D hit)
	{
		if (rootNotreachingAnimation != null)
			StopCoroutine(rootNotreachingAnimation);
		rootNotreachingAnimation = StartCoroutine(RootNotReaching(hit));
	}

	void EnableRoot(Root root, RaycastHit2D hit)
	{
		var direction = (transform.position - (Vector3)hit.point);
		var mag = direction.magnitude;
		if (mag > rootHookDistance)
		{
			UnReachableArea(hit);
			return;
		}
		var p = Instantiate(particleHit);
		p.position = transform.position;
		p.forward = direction;
		Destroy(p.gameObject, 1);
		var pHit = Instantiate(particleHit);
		Destroy(pHit.gameObject, 1);
		pHit.position = hit.point;
		pHit.forward = -direction;
		Instantiate(rootHit, hit.point, Quaternion.identity);
		root.sprint.connectedBody.transform.position = hit.point;
		root.sprint.connectedBody.gameObject.SetActive(true);
		root.sprint.enabled = true;
		root.sprint.distance = mag * pullDistance;
	}

	IEnumerator RootNotReaching(RaycastHit2D hit)
	{
		rootNotReaching.gameObject.SetActive(true);
		float t = 0;
		var p = Instantiate(particleHit);
		p.position = transform.position;
		p.forward = -(transform.position - (Vector3)hit.point).normalized;
		Destroy(p.gameObject, 1);
		while (t < lerpTime)
		{
			t += Time.deltaTime;
			yield return null;
			Vector3 direction = -(transform.position - (Vector3)hit.point).normalized;
			float lerp = Mathf.Lerp(0, rootHookDistance, t / lerpTime);
			rootNotReaching.localPosition = transform.position + direction * lerp / 2;
			rootNotReaching.localScale = new Vector3(.5f, lerp, .5f);
			rootNotReaching.up = direction;
		}
		t = 0;
		while (t < lerpTime)
		{
			t += Time.deltaTime;
			yield return null;
			Vector3 direction = -(transform.position - (Vector3)hit.point).normalized;
			float lerp = Mathf.Lerp(rootHookDistance, 0, t / lerpTime);
			rootNotReaching.localPosition = transform.position + direction * lerp / 2;
			rootNotReaching.localScale = new Vector3(.5f, lerp, .5f);
			rootNotReaching.up = direction;
		}
		rootNotReaching.gameObject.SetActive(false);
	}

	void DisableRoot(Transform r, bool cleaner = false)
	{
		foreach (var root in springJoint2Ds)
		{
			if (root.root == r)
			{
				root.sprint.connectedBody.gameObject.SetActive(false);
				root.sprint.enabled = false;
				if (!cleaner)
				{
					var cutRoot = Instantiate(cutRootVisuals, root.root.position, root.root.rotation);
					cutRoot.transform.right = root.root.up;
				}
			}
			root.timeStamp = Time.time;
		}
	}

	void Update()
	{
		rootNotReaching.position = transform.position;
		float p = path.FindClosestPoint(transform.position, 0, -1, 10000);
		mainCamera.transform.position = path.EvaluatePosition(p) + Vector3.forward * -5;
		foreach (var root in springJoint2Ds)
		{
			if (root.Enable)
			{
				Vector3 direction = transform.position - root.root.position;
				float distance = direction.magnitude;
				direction.Normalize();
				root.root.up = direction;
				root.visuals.localScale = Vector3.Lerp(root.visuals.localScale, new Vector3(.5f, distance, .5f), lerpSpeed * Time.deltaTime);
				root.visuals.localPosition = Vector3.Lerp(root.visuals.localPosition, new Vector3(0, distance / 2, 0), lerpSpeed * Time.deltaTime);
			}
		}
		if (Input.GetMouseButton(0))
		{
			var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			var hit = Physics2D.Raycast(ray.origin, Vector2.one, .01f, 1 << LayerMask.NameToLayer("Root"));
			if (hit.collider)
			{
				DisableRoot(hit.transform);
				return;
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			var hit = Physics2D.Raycast(ray.origin, Vector2.one, .01f, 1 << LayerMask.NameToLayer("Default"));
			if (hit.transform)
			{
				for (int i = 0; i < Mathf.Min(unlockedRoots, springJoint2Ds.Length); i++)
				{
					var root = springJoint2Ds[i];
					if (!root.Enable)
					{
						EnableRoot(root, hit);
						break;
					}
				}
			}
			else
			{
				hit.point = ray.origin;
				UnReachableArea(hit);
			}
		}
	}
}