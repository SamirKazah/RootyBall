using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	[SerializeField] Player player;
	[SerializeField] CanvasGroup fader;
	[SerializeField] GameObject endScene, sclicePanel, shootTutorial;
	[SerializeField] float timeNeeded = 0.3f;

	public static GameManager instance;
	public static Vector3 checkPoint;

	private void Start()
	{
		instance = this;
	}

	public void StartGame()
	{
		checkPoint = player.transform.position;
		player.enabled = true;
		sclicePanel.SetActive(true);
	}

	bool shot;
	public void EnableShoot()
	{
		if (shot)
			return;
		shot = true;
		shootTutorial.SetActive(true);
	}

	public void GotBackToCheckPoint()
	{
		StartCoroutine(OpenFader(0,() => player.transform.position = checkPoint));
	}

	public void OpenEndScenario()
	{
		StartCoroutine(OpenFader(1, () => endScene.SetActive(true)));
	}

	IEnumerator OpenFader(float delay, UnityAction onEnd)
	{
		yield return new WaitForSeconds(delay);
		fader.gameObject.SetActive(true);
		float t = 0;
		while (t < timeNeeded)
		{
			t += Time.deltaTime;
			yield return null;
			fader.alpha = Mathf.Lerp(0, 1, t / timeNeeded);
		}
		yield return new WaitForSeconds(.5f);
		onEnd.Invoke();
		yield return new WaitForSeconds(.5f);
		t = 0;
		while (t < timeNeeded)
		{
			t += Time.deltaTime;
			yield return null;
			fader.alpha = Mathf.Lerp(1, 0, t / timeNeeded);
		}
		fader.gameObject.SetActive(false);
	}
}
