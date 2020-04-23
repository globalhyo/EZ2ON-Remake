/*
  ============================================
	Author	: KJH
	Time 	: 2020-04-21 오후 4:27:33
  ============================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Percent : MonoBehaviour
{
	/* [PUBLIC VARIABLE]					*/


	/* [PROTECTED && PRIVATE VARIABLE]		*/

	private Text _text_Percent;
	private bool _isRequireUpdate;

	/*----------------[PUBLIC METHOD]------------------------------*/


	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Awake()
	{
		_text_Percent = GetComponent<Text>();
	}

	private void Start()
	{
		EZR.PlayManager.OnUpdateScore += OnUpdateScore;
	}

	private void OnDestroy()
	{
		EZR.PlayManager.OnUpdateScore -= OnUpdateScore;
	}

	private void OnUpdateScore(EZR.JudgmentType obj)
	{
		_isRequireUpdate = true;
	}

	private void LateUpdate()
	{
		if (_isRequireUpdate)
		{
			_text_Percent.text = string.Format("{0:0.00}", EZR.PlayManager.Score.Percent);
			_isRequireUpdate = false;
		}
	}
}