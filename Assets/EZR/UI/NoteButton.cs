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
using EZR;

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
			PlayManager.NotePattern++;
		else if (pointerEventData.button == PointerEventData.InputButton.Right)
			PlayManager.NotePattern--;

		if (PlayManager.NotePattern > NotePattern.Random)
			PlayManager.NotePattern = NotePattern.None;

		else if (PlayManager.NotePattern < NotePattern.None)
			PlayManager.NotePattern = NotePattern.Random;

		Change(PlayManager.NotePattern);
		MemorySound.PlaySound("e_count_1");
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

	private void Start()
	{
		Change(PlayManager.NotePattern);
	}

	private void Change(NotePattern notePattern)
	{
		_image.sprite = _sprites[(int)notePattern];
	}

}