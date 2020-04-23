/*
  ============================================
	Author	: KJH
	Time 	: 2020-04-20 오후 10:10:57
  ============================================
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Popup_PressKey : MonoBehaviour
{
	/* [PUBLIC VARIABLE]					*/


	/* [PROTECTED && PRIVATE VARIABLE]		*/

	private Text _targetText;

	private Canvas _canvas;

	private int _keyType;
	private int _keyIndex;

	private KeyCodeVK _keyCode;
	private bool _isPressed;

	private Action<int, int, int> _onKeyPressed;

	/*----------------[PUBLIC METHOD]------------------------------*/

	public void Show(Text text, int keyType, int keyIndex, Action<int, int, int> onAction)
	{
		_targetText = text;
		_keyType = keyType;
		_keyIndex = keyIndex;

		_onKeyPressed = onAction;

		_canvas.enabled = true;
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void Awake()
	{
		_canvas = GetComponent<Canvas>();

		EZR.Master.InputVirtualKeyEvent += InputVirtualKeyEvent;
	}

	private void InputVirtualKeyEvent(KeyCodeVK keyCode, bool isPressed)
	{
		if (isPressed == false) return;
		if (_targetText == null) return;

		_keyCode = keyCode;
		_isPressed = isPressed;
	}

	private void OnGUI()
	{
		if (_targetText == null) return;

		if (_isPressed)
		{
			KeyCodeVK code = _keyCode;
			if (code == KeyCodeVK.Escape)
			{
				Close();
				return;
			}
			else if (code == KeyCodeVK.BackSpace)
			{
				_targetText.text = "";
				_onKeyPressed?.Invoke(_keyType, _keyIndex, 0);
				Close();
				return;
			}

			_targetText.text = code.ToString();
			_onKeyPressed?.Invoke(_keyType, _keyIndex, (int)code);
			Close();
		}
	}

	private void Close()
	{
		_targetText = null;
		_isPressed = false;
		_canvas.enabled = false;
	}
}