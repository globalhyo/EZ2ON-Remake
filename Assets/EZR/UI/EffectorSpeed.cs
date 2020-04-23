/*
  ============================================
	Author	: KJH
	Time 	: 2020-04-21 오후 4:56:00
  ============================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectorSpeed : MonoBehaviour
{
	/* [PUBLIC VARIABLE]					*/


	/* [PROTECTED && PRIVATE VARIABLE]		*/

	private Text _text_Speed;
	private bool _isRequireUpdate;

	/*----------------[PUBLIC METHOD]------------------------------*/


	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Awake()
	{
		_text_Speed = GetComponent<Text>();

		EZR.PlayManager.OnUpdateSpeed += OnUpdateSpeed;
	}

	private void Start()
	{
		_text_Speed.text = EZR.PlayManager.FallSpeed.ToString("0.00");
	}

	private void OnDestroy()
	{
		EZR.PlayManager.OnUpdateSpeed -= OnUpdateSpeed;
	}

	private void OnUpdateSpeed(float speed)
	{
		_isRequireUpdate = true;
	}

	private void LateUpdate()
	{
		if (_isRequireUpdate)
		{
			_text_Speed.text = EZR.PlayManager.FallSpeed.ToString("0.00");
			_isRequireUpdate = false;
		}
	}
}