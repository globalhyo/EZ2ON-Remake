/*
  ============================================
	Author	: KJH
	Time 	: 2020-04-20 오후 3:59:57
  ============================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EZR;

public class PersistentCanvas : MonoBehaviour
{
	/* [PUBLIC VARIABLE]					*/

	public static PersistentCanvas Instance;

	public Text Text_FPS;
	public RawImage EyeCatchFader;

	private Option _option;

	private float _timeleft;

	/* [PROTECTED && PRIVATE VARIABLE]		*/


	/*----------------[PUBLIC METHOD]------------------------------*/

	public void LoadPlayScene()
	{
		StartCoroutine(CoLoadPlayScene());
	}


	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private IEnumerator CoLoadPlayScene()
	{
		AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("SinglePlay");
		sceneLoad.allowSceneActivation = true;

		while (sceneLoad.isDone == false)
			yield return null;

		EyeCatchFader.color = Color.white;

		Color color = EyeCatchFader.color;
		while (color.a > 0f)
		{
			color.a = Mathf.Clamp01(color.a - (Time.deltaTime * 2f));
			EyeCatchFader.color = color;
			yield return null;
		}
	}

	private void Awake()
	{
		Instance = this;

		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		if (Text_FPS.enabled)
		{
			if (_timeleft <= 0f)
			{
				Text_FPS.text = string.Format("{0}", (int)(1.0f / Time.deltaTime));

				_timeleft = Time.deltaTime + 0.35f;
			}

			_timeleft -= Time.deltaTime;
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 60, 100), "Major: " + System.Environment.OSVersion.Version.Major + "\nMinor: " + System.Environment.OSVersion.Version.Minor);
	}
}