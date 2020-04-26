﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZR;

public class SinglePlay : MonoBehaviour
{
    bool isFinished = false;
    EZR.DisplayLoop displayLoop;

	private KeyCodeVK _lastKeyCode;
	private bool _isKeyPressed;

	private EZR.Option _option;

	void Start()
    {
#if (!UNITY_EDITOR)
        // 关闭内存回收
        UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Disabled;
#endif

		_option = EZR.UserSaveData.GetOption();
		EZR.JudgmentDelta.GlobalScale = _option.JudgeDifficult.GetJudge();

		EZR.PlayManager.Reset();
        EZR.PlayManager.LoadPattern();

        EZR.PlayManager.LoopStop += loopStop;
		EZR.Master.InputVirtualKeyEvent += InputVirtualKeyEvent;

        displayLoop = GetComponent<EZR.DisplayLoop>();
        displayLoop.enabled = true;
    }

	private void OnDestroy()
	{
		EZR.Master.InputVirtualKeyEvent -= InputVirtualKeyEvent;
	}

	private void InputVirtualKeyEvent(KeyCodeVK keyCode, bool pressed)
	{
		_lastKeyCode = keyCode;
		_isKeyPressed = pressed;
	}

	Coroutine speedPressedCoroutine;
    void Update()
    {
		if (isFinished)
        {
            isFinished = false;
            finished(true);
            return;
        }

		if (_isKeyPressed)
		{
			if (_lastKeyCode == (KeyCodeVK)_option.KeyMapping[5, 0])
			{
				EZR.PlayManager.IsAutoPlay = !EZR.PlayManager.IsAutoPlay;
				EZR.MemorySound.PlaySound("e_count_1");
				_isKeyPressed = false;
			}
			else if (_lastKeyCode == (KeyCodeVK)_option.KeyMapping[5, 1])
			{
				speedAdd(0.25f);
				_isKeyPressed = false;
			}
			else if (_lastKeyCode == (KeyCodeVK)_option.KeyMapping[5, 2])
			{
				speedAdd(-0.25f);
				_isKeyPressed = false;
			}
		}


		// 关门
		if (EZR.PlayManager.HP == 0)
        {
            finished(true);
            EZR.MemorySound.PlaySound("e_die");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            finished(false);
        }

		// Removed, Use keymapping
		//if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		//{
		//    if (Input.GetKeyDown(KeyCode.UpArrow))
		//    {
		//        speedAddSmall(0.01f);
		//        if (speedPressedCoroutine != null) StopCoroutine(speedPressedCoroutine);
		//        speedPressedCoroutine = StartCoroutine(speedPressed(0.01f));
		//    }
		//    else if (Input.GetKeyUp(KeyCode.UpArrow))
		//    {
		//        if (speedPressedCoroutine != null) StopCoroutine(speedPressedCoroutine);
		//    }
		//    if (Input.GetKeyDown(KeyCode.DownArrow))
		//    {
		//        speedAddSmall(-0.01f);
		//        if (speedPressedCoroutine != null) StopCoroutine(speedPressedCoroutine);
		//        speedPressedCoroutine = StartCoroutine(speedPressed(-0.01f));
		//    }
		//    else if (Input.GetKeyUp(KeyCode.DownArrow))
		//    {
		//        if (speedPressedCoroutine != null) StopCoroutine(speedPressedCoroutine);
		//    }
		//}
		//else
		//{
		//    if (speedPressedCoroutine != null) StopCoroutine(speedPressedCoroutine);
		//    if (Input.GetKeyDown(KeyCode.UpArrow))
		//    {
		//        speedAdd(0.25f);
		//    }
		//    if (Input.GetKeyDown(KeyCode.DownArrow))
		//    {
		//        speedAdd(-0.25f);
		//    }
		//}

		if (Input.GetKeyDown(KeyCode.F12))
        {
            var Info = GameObject.Find("DebugCanvas").transform.Find("Info").gameObject;
            Info.SetActive(!Info.activeSelf);
            EZR.MemorySound.PlaySound("e_count_1");
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            var bga = GameObject.Find("Canvas").transform.Find("BGA");
            if (bga != null && bga.gameObject.activeSelf)
            {
                Destroy(bga.gameObject);
                EZR.MemorySound.PlaySound("e_count_1");
            }
        }
    }

    IEnumerator speedPressed(float val)
    {
        yield return new WaitForSeconds(0.2f);
        for (; ; )
        {
            yield return new WaitForSeconds(0.075f);
            speedAddSmall(val);
        }
    }

    void speedAddSmall(float val)
    {
        EZR.PlayManager.FallSpeed += val;
        EZR.MemorySound.PlaySound("e_count_1");
    }

    void speedAdd(float val)
    {
        var decimalPart = EZR.PlayManager.FallSpeed % 1;
        float closest;
        if (val > 0)
            closest = EZR.Utils.FindClosestNumber(decimalPart, EZR.PlayManager.FallSpeedStep, true);
        else
            closest = EZR.Utils.FindClosestNumber(decimalPart, EZR.PlayManager.FallSpeedStep, false);
        if (Mathf.Abs(((int)EZR.PlayManager.FallSpeed + closest) - EZR.PlayManager.FallSpeed) > 0.009f)
            EZR.PlayManager.FallSpeed = ((int)EZR.PlayManager.FallSpeed + closest);
        else
            EZR.PlayManager.FallSpeed = ((int)EZR.PlayManager.FallSpeed + closest) + val;
        EZR.MemorySound.PlaySound("e_count_1");

		EZR.PlayManager.OnUpdateSpeed?.Invoke(EZR.PlayManager.FallSpeed);
	}

    void loopStop()
    {
        EZR.PlayManager.LoopStop -= loopStop;
        isFinished = true;
        EZR.PlayManager.Score.IsClear = true;
    }

    void finished(bool isResult)
    {
        EZR.PlayManager.LoopStop -= loopStop;
        displayLoop.Stop();

        EZR.MemorySound.Main.stop();
        EZR.MemorySound.BGM.stop();
        EZR.MemorySound.UnloadAllSound();
		EZR.UserSaveData.SetSpeed(EZR.PlayManager.SongName, EZR.PlayManager.GameMode, EZR.PlayManager.FallSpeed);
		EZR.UserSaveData.SaveData();

		if (isResult)
            SceneManager.LoadScene("SingleResult");
        else
            SceneManager.LoadScene("SingleSelectSongs");

#if (!UNITY_EDITOR)
                // 开启内存回收
                UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Enabled;
                System.GC.Collect();
#endif
    }
}