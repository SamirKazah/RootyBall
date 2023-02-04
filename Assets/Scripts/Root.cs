using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
	public SpringJoint2D sprint;
	public Transform root;
	public Transform visuals;
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

}
