/*
  ============================================
	Author	: jinnin0105
	Time 	: 2020-04-26 오후 3:26:06
  ============================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZR;

public class Tooltips_Judge : Tooltips
{
	/* [PUBLIC VARIABLE]					*/

	public GameDifficult.JudgeDifficult judgeDifficult;

	/* [PROTECTED && PRIVATE VARIABLE]		*/


	/*----------------[PUBLIC METHOD]------------------------------*/


	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	protected override void Start()
	{
		base.Start();

		TipsText = string.Format(TipsText, judgeDifficult.GetJudge());
	}

}