/*
  ============================================
	Author	: KJH
	Time 	: 2020-04-22 오후 11:14:49
  ============================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NCButton : MonoBehaviour
{
	/* [PUBLIC VARIABLE]					*/


	/* [PROTECTED && PRIVATE VARIABLE]		*/

	[SerializeField] private Text _text;

	/*----------------[PUBLIC METHOD]------------------------------*/

	public void OnClickButton(BaseEventData baseEventData)
	{
		var pointerEventData = (PointerEventData)baseEventData;
		if (pointerEventData.button == PointerEventData.InputButton.Left)
			EZR.PlayManager.PlaybackSpeed += 0.25f;
		else if (pointerEventData.button == PointerEventData.InputButton.Right)
			EZR.PlayManager.PlaybackSpeed -= 0.25f;

		if (EZR.PlayManager.PlaybackSpeed > 1.5f)
			EZR.PlayManager.PlaybackSpeed = 1f;
		if (EZR.PlayManager.PlaybackSpeed < 1f)
			EZR.PlayManager.PlaybackSpeed = 1.5f;

		EZR.MemorySound.PlaySound("e_count_1");
		Change(EZR.PlayManager.PlaybackSpeed);
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Start()
	{
		Change(EZR.PlayManager.PlaybackSpeed);
	}

	private void Change(float speed)
	{
		switch (speed)
		{
			case 1f:
				_text.text = "NONE";
				break;
			case 1.25f:
				_text.text = "NC HD";
				break;
			case 1.5f:
				_text.text = "NC SC";
				break;
		}
	}
}