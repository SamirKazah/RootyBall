using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameManager.instance.GotBackToCheckPoint();
		var player = collision.gameObject.GetComponent<Player>();
		if (player)
			player.ClearAllRoots();
	}
}
