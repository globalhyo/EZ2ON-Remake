/*
  ============================================
	Author	: KJH
	Time 	: 2020-04-20 오후 8:19:20
  ============================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeUi : MonoBehaviour
{
	/* [PUBLIC VARIABLE]					*/

	public Text _Text_KOOL;
	public Text _Text_COOL;
	public Text _Text_GOOD;
	public Text _Text_MISS;
	public Text _Text_FAIL;

	/* [PROTECTED && PRIVATE VARIABLE]		*/

	private int _lastKool;
	private int _lastCool;
	private int _lastGood;
	private int _lastMiss;
	private int _lastFail;

	/*----------------[PUBLIC METHOD]------------------------------*/



	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Update()
	{
		if (_lastKool != EZR.PlayManager.Score.Kool)
			OnUpdateScore(EZR.JudgmentType.Kool);

		if (_lastCool != EZR.PlayManager.Score.Cool)
			OnUpdateScore(EZR.JudgmentType.Cool);

		if (_lastGood != EZR.PlayManager.Score.Good)
			OnUpdateScore(EZR.JudgmentType.Good);

		if (_lastMiss != EZR.PlayManager.Score.Miss)
			OnUpdateScore(EZR.JudgmentType.Miss);

		if (_lastFail != EZR.PlayManager.Score.Fail)
			OnUpdateScore(EZR.JudgmentType.Fail);

		_lastKool = EZR.PlayManager.Score.Kool;
		_lastCool = EZR.PlayManager.Score.Cool;
		_lastGood = EZR.PlayManager.Score.Good;
		_lastMiss = EZR.PlayManager.Score.Miss;
		_lastFail = EZR.PlayManager.Score.Fail;
	}

	private void OnUpdateScore(EZR.JudgmentType type)
	{
		switch (type)
		{
			case EZR.JudgmentType.Kool:
				_Text_KOOL.text = string.Format("{0:D4}", EZR.PlayManager.Score.Kool);
				break;
			case EZR.JudgmentType.Cool:
				_Text_COOL.text = string.Format("{0:D4}", EZR.PlayManager.Score.Cool);
				break;
			case EZR.JudgmentType.Good:
				_Text_GOOD.text = string.Format("{0:D4}", EZR.PlayManager.Score.Good);
				break;
			case EZR.JudgmentType.Miss:
				_Text_MISS.text = string.Format("{0:D4}", EZR.PlayManager.Score.Miss);
				break;
			case EZR.JudgmentType.Fail:
				_Text_FAIL.text = string.Format("{0:D4}", EZR.PlayManager.Score.Fail);
				break;
		}
	}
}