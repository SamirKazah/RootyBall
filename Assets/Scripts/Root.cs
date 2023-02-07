using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
	public Vines vine;
	public SpringJoint2D sprint;
	public Transform root;
	public Transform visuals, cutRootVisuals;
	public float timeStamp;
	public bool Enable
	{
		get
		{
			if (Time.time - timeStamp < .1f)
			{
				return true;
			}
			return sprint.enabled;
		}
	}
	Vector3 relativePos;
	Transform _parent;
	public Transform parent
	{
		set
		{
			_parent = value;
			if (value)
				relativePos = transform.position - _parent.position;
		}
		get => _parent;
	}

	private void Update()
	{
		if (_parent)
		{
			transform.position = _parent.position + relativePos;
			if (cutRootVisuals)
				cutRootVisuals.position = _parent.position + relativePos;
		}
	}
}
