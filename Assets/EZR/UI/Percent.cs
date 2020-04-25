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

	private Animation _animation;
	private Text _text_Percent;

	private float _lastPercent;
	private bool _isRequireUpdate;

	/*----------------[PUBLIC METHOD]------------------------------*/


	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Awake()
	{
		_text_Percent = GetComponent<Text>();
		_animation = GetComponent<Animation>();
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
			float percent = EZR.PlayManager.Score.Percent;
			_text_Percent.text = string.Format("{0:0.00}", percent);

			if (percent > _lastPercent)
			{
				_animation.Stop();
				_animation.Play();
			}

			_lastPercent = percent;
			_isRequireUpdate = false;
		}
	}
}