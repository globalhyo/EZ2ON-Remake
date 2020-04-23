/*
  ============================================
	Author	: KJH
	Time 	: 2020-04-22 오후 7:02:21
  ============================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NoteButton : MonoBehaviour
{
	/* [PUBLIC VARIABLE]					*/

	[SerializeField] private Sprite[] _sprites;

	/* [PROTECTED && PRIVATE VARIABLE]		*/

	private Image _image;

	/*----------------[PUBLIC METHOD]------------------------------*/

	public void OnClickButton(BaseEventData baseEventData)
	{
		var pointerEventData = (PointerEventData)baseEventData;
		if (pointerEventData.button == PointerEventData.InputButton.Left)
			EZR.PlayManager.NotePattern++;
		else if (pointerEventData.button == PointerEventData.InputButton.Right)
			EZR.PlayManager.NotePattern--;

		if (EZR.PlayManager.NotePattern > EZR.NotePattern.Random)
			EZR.PlayManager.NotePattern = EZR.NotePattern.None;
		else if (EZR.PlayManager.NotePattern < EZR.NotePattern.None)
			EZR.PlayManager.NotePattern = EZR.NotePattern.Random;

		EZR.MemorySound.PlaySound("e_count_1");
		Change(EZR.PlayManager.NotePattern);
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

	private void Start()
	{
		Change(EZR.PlayManager.NotePattern);
	}

	private void Change(EZR.NotePattern notePattern)
	{
		_image.sprite = _sprites[(int)notePattern];
	}

}