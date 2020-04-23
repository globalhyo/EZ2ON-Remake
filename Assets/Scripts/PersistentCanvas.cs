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
using EZR;

public class PersistentCanvas : MonoBehaviour
{
	/* [PUBLIC VARIABLE]					*/

	public static PersistentCanvas Instance;

	public Text Text_FPS;

	private Option _option;

	private float _timeleft;

	/* [PROTECTED && PRIVATE VARIABLE]		*/


	/*----------------[PUBLIC METHOD]------------------------------*/


	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

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
}