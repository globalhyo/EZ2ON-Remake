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
using EZR;

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
			PlayManager.PlaybackType++;
		else if (pointerEventData.button == PointerEventData.InputButton.Right)
			PlayManager.PlaybackType--;

		if (PlayManager.PlaybackType > PlaybackType.NC_SC)
			PlayManager.PlaybackType = PlaybackType.NONE;

		else if (PlayManager.PlaybackType < PlaybackType.NONE)
			PlayManager.PlaybackType = PlaybackType.NC_SC;

		Change(PlayManager.PlaybackType);
		MemorySound.PlaySound("e_count_1");
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Start()
	{
		Change(PlayManager.PlaybackType);
	}

	private void Change(PlaybackType type)
	{
		switch (type)
		{
			default:
				_text.text = PlaybackSpeed.NONE_Name;
				break;
			case PlaybackType.NC_HD:
				_text.text = PlaybackSpeed.NCHD_Name;
				break;
			case PlaybackType.NC_SC:
				_text.text = PlaybackSpeed.NCSC_Name;
				break;
		}
	}
}