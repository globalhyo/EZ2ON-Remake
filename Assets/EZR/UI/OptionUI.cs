using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionUI : MonoBehaviour
{
	EZR.Option option;
	Text sliderLimitFPSText;
	Text sliderTimmingText;
	List<Resolution> resolutions = new List<Resolution>();
	Dropdown dropdownResolutions;

	private Button[,] _keyButtons = new Button[6, 8];

	private void Awake()
	{
		Button[] keyButtons = transform.Find("GroupInput").GetComponentsInChildren<Button>(true);

		int len = keyButtons.Length;
		for (int i = 0; i < len; i++)
		{
			Button button = keyButtons[i];
			int key = button.transform.parent.transform.parent.GetSiblingIndex();
			int index = button.transform.GetSiblingIndex();
			_keyButtons[key, index] = button;
		}
	}

	void Start()
	{
		sliderLimitFPSText = transform.Find("GroupSystem/BarPerformance/ChkBoxLimitFPS/SliderLimitFPS/Text").GetComponent<Text>();
		sliderTimmingText = transform.Find("GroupSystem/BarTimePrecision/SliderTimming/Text").GetComponent<Text>();
		dropdownResolutions = transform.Find("GroupSystem/BarResolutions/Dropdown").GetComponent<Dropdown>();
		option = EZR.UserSaveData.GetOption();

		// 找毛玻璃
		var frostedGlass = transform.Find("FrostedGlass").gameObject;
		frostedGlass.SetActive(option.FrostedGlassEffect);

		transform.Find("BtnSystem").GetComponent<EZR.ButtonExtension>().SetSelected(true);

		updateSystem();
		UpdateKeyMappingUiButtons();
	}

	private void UpdateKeyMappingUiButtons()
	{
		int xLen = _keyButtons.GetLength(0);
		int yLen = _keyButtons.GetLength(1);

		for (int x = 0; x < xLen; x++)
		{
			for (int y = 0; y < yLen; y++)
			{
				if (_keyButtons[x, y] == null) continue;

				int key = option.KeyMapping[x, y];
				if (key <= 0) continue;

				_keyButtons[x, y].transform.Find("Text").GetComponent<Text>().text =
					((KeyCodeVK)key).ToString();
			}
		}
	}

	void updateSystem()
	{
		var screenMode = transform.Find("GroupSystem/BarScreenMode");
		switch (option.FullScreenMode)
		{
			case FullScreenMode.ExclusiveFullScreen:
				screenMode.Find("ChkBoxExclusiveFullScreen").GetComponent<Toggle>().isOn = true;
				break;
			case FullScreenMode.FullScreenWindow:
				screenMode.Find("ChkBoxFullScreenWindow").GetComponent<Toggle>().isOn = true;
				break;
			default:
				screenMode.Find("ChkBoxWindowed").GetComponent<Toggle>().isOn = true;
				break;
		}

		updateResolutions();

		var language = transform.Find("GroupSystem/BarLanguage");
		switch (option.Language)
		{
			default:
				language.Find("ChkBoxSChinese").GetComponent<Toggle>().isOn = true;
				break;
			case SystemLanguage.English:
				language.Find("ChkBoxEnglish").GetComponent<Toggle>().isOn = true;
				break;
		}

		var judge = transform.Find("GroupSystem/BarJudge");
		switch (option.JudgeLevel)
		{
			default:
				judge.Find("ChkBoxEasy").GetComponent<Toggle>().isOn = true;
				break;
			case 0.8f:
				judge.Find("ChkBoxNormal").GetComponent<Toggle>().isOn = true;
				break;
			case 0.6f:
				judge.Find("ChkBoxHard").GetComponent<Toggle>().isOn = true;
				break;
			case 0.4f:
				judge.Find("ChkBoxEx").GetComponent<Toggle>().isOn = true;
				break;
		}

		var performance = transform.Find("GroupSystem/BarPerformance");
		performance.Find("ChkBoxFrostedGlass").GetComponent<Toggle>().isOn = option.FrostedGlassEffect;
		performance.Find("ChkBoxVSync").GetComponent<Toggle>().isOn = option.VSync;
		performance.Find("ChkBoxShowFPS").GetComponent<Toggle>().isOn = option.ShowFPS;

		var sliderTimming = transform.Find("GroupSystem/BarTimePrecision/SliderTimming");
		sliderTimming.Find("Slider").GetComponent<Slider>().value = option.TimePrecision;
		sliderTimmingText.text = option.TimePrecision.ToString() + "ms";

		var chkBoxLimitFPS = performance.Find("ChkBoxLimitFPS");
		if (!option.VSync)
		{
			chkBoxLimitFPS.gameObject.SetActive(true);
		}
		else
		{
			chkBoxLimitFPS.gameObject.SetActive(false);
		}
		chkBoxLimitFPS.GetComponent<Toggle>().isOn = option.LimitFPS;
		var sliderLimitFPS = chkBoxLimitFPS.Find("SliderLimitFPS");
		if (option.LimitFPS) sliderLimitFPS.gameObject.SetActive(true);
		else sliderLimitFPS.gameObject.SetActive(false);
		sliderLimitFPS.Find("Text").GetComponent<Text>().text = option.TargetFrameRate.ToString();
		sliderLimitFPS.Find("Slider").GetComponent<Slider>().value = option.TargetFrameRate;
		var panelPosition = transform.Find("GroupSkin/BarPanelPosition");
		switch (option.PanelPosition)
		{
			case EZR.Option.PanelPositionEnum.Left:
				panelPosition.Find("ChkBoxLeft").GetComponent<Toggle>().isOn = true;
				break;
			case EZR.Option.PanelPositionEnum.Center:
				panelPosition.Find("ChkBoxCenter").GetComponent<Toggle>().isOn = true;
				break;
			case EZR.Option.PanelPositionEnum.Right:
				panelPosition.Find("ChkBoxRight").GetComponent<Toggle>().isOn = true;
				break;
		}
		var targetLineType = transform.Find("GroupSkin/BarLineType");
		switch (option.TargetLineType)
		{
			case EZR.Option.TargetLineTypeEnum.New:
				targetLineType.Find("ChkBoxNew").GetComponent<Toggle>().isOn = true;
				break;
			case EZR.Option.TargetLineTypeEnum.Classic:
				targetLineType.Find("ChkBoxClassic").GetComponent<Toggle>().isOn = true;
				break;
		}

		var panelETC = transform.Find("GroupSkin/BarETC");
		panelETC.Find("ChkBoxShowFastSlow").GetComponent<Toggle>().isOn = option.ShowFastSlow;
		panelETC.Find("ChkBoxShowJudgeList").GetComponent<Toggle>().isOn = option.ShowJudgeList;
		panelETC.Find("ChkBoxShowPercent").GetComponent<Toggle>().isOn = option.ShowPercent;
		panelETC.Find("ChkBoxUiJudgeLine").GetComponent<Toggle>().isOn = option.UiJudgeLine;
		updateJudgmentOffset();

		transform.Find("GroupSound/SliderMasterVolume").GetComponent<Slider>().value = option.Volume.Master;
		transform.Find("GroupSound/SliderGameVolume").GetComponent<Slider>().value = option.Volume.Game;
		transform.Find("GroupSound/SliderMainVolume").GetComponent<Slider>().value = option.Volume.Main;
		transform.Find("GroupSound/SliderBGMVolume").GetComponent<Slider>().value = option.Volume.BGM;
		transform.Find("GroupSound/ChkBoxLive3D").GetComponent<Toggle>().isOn = option.Volume.Live3D;

		transform.Find("GroupSkin/BarAlpha/SlideGearAlpha/GearAlpha").GetComponent<Slider>().value = option.GearAlpha;
		transform.Find("GroupSkin/BarAlpha/SlideBGAlpha/BGAlpha").GetComponent<Slider>().value = option.BGAlpha;
		transform.Find("GroupSkin/BarAlpha/SlideHPAlpha/HPAlpha").GetComponent<Slider>().value = option.HPAlpha;
		transform.Find("GroupSkin/BarAlpha/SlideJudgeAlpha/JudgeAlpha").GetComponent<Slider>().value = option.JudgeAlpha;
		transform.Find("GroupSkin/BarAlpha/SlideComboAlpha/ComboAlpha").GetComponent<Slider>().value = option.ComboAlpha;
		transform.Find("GroupBGA/BarPanelBGA/SliderBGABright/BGABright").GetComponent<Slider>().value = option.BGABright;

		transform.Find("GroupSkin/BarAlpha/SlideGearAlpha/GearAlpha").GetComponent<Slider>().transform.Find("Text_Slider").GetComponent<Text>().text = string.Format("{0}%", option.GearAlpha);
		transform.Find("GroupSkin/BarAlpha/SlideBGAlpha/BGAlpha").GetComponent<Slider>().transform.Find("Text_Slider").GetComponent<Text>().text = string.Format("{0}%", option.BGAlpha);
		transform.Find("GroupSkin/BarAlpha/SlideHPAlpha/HPAlpha").GetComponent<Slider>().transform.Find("Text_Slider").GetComponent<Text>().text = string.Format("{0}%", option.HPAlpha);
		transform.Find("GroupSkin/BarAlpha/SlideJudgeAlpha/JudgeAlpha").GetComponent<Slider>().transform.Find("Text_Slider").GetComponent<Text>().text = string.Format("{0}%", option.JudgeAlpha);
		transform.Find("GroupSkin/BarAlpha/SlideComboAlpha/ComboAlpha").GetComponent<Slider>().transform.Find("Text_Slider").GetComponent<Text>().text = string.Format("{0}%", option.ComboAlpha);
		transform.Find("GroupBGA/BarPanelBGA/SliderBGABright/BGABright").GetComponent<Slider>().transform.Find("Text_Slider").GetComponent<Text>().text = string.Format("{0}%", option.BGABright);


		var panelBGA = transform.Find("GroupBGA/BarPanelBGA");
		panelBGA.Find("ChkBoxBGA").GetComponent<Toggle>().isOn = option.ShowBGA;
	}

	void updateResolutions()
	{
		if (dropdownResolutions == null) return;
		dropdownResolutions.options.Clear();
		foreach (var resolutionA in Screen.resolutions)
		{
			var isHit = false;
			foreach (var resolutionB in resolutions)
			{
				if (resolutionA.width == resolutionB.width &&
				resolutionA.height == resolutionB.height)
				{
					isHit = true;
					break;
				}
			}
			if (!isHit) resolutions.Add(new Resolution()
			{
				width = resolutionA.width,
				height = resolutionA.height
			});
		}
		bool isHit2 = false;
		for (int i = 0; i < resolutions.Count; i++)
		{
			if (option.FullScreenMode == FullScreenMode.Windowed &&
			i == resolutions.Count - 1)
				break;
			var resolution = resolutions[i];
			dropdownResolutions.options.Add(new Dropdown.OptionData(resolution.width + "×" + resolution.height));
			if (option.Resolution.width == resolution.width &&
			option.Resolution.height == resolution.height)
			{
				isHit2 = true;
				dropdownResolutions.value = i;
			}
		}
		if (!isHit2)
		{
			dropdownResolutions.value = resolutions.Count - 1;
			option.Resolution = resolutions[resolutions.Count - 1];
		}
	}

	public void ToggleScreenMode(bool value)
	{
		if (!value) return;
		var toggle = EventSystem.current.currentSelectedGameObject;
		switch (toggle.name)
		{
			case "ChkBoxExclusiveFullScreen":
				option.FullScreenMode = FullScreenMode.ExclusiveFullScreen;
				break;
			case "ChkBoxFullScreenWindow":
				option.FullScreenMode = FullScreenMode.FullScreenWindow;
				break;
			case "ChkBoxWindowed":
				option.FullScreenMode = FullScreenMode.Windowed;
				break;
		}
		updateResolutions();
	}

	public void DropdownResolutionsClick()
	{
		dropdownResolutions.transform.Find("Dropdown List").GetComponent<ScrollRect>().verticalNormalizedPosition =
		1 - (float)dropdownResolutions.value / (resolutions.Count - 1);
		EZR.MemorySound.PlaySound("e_click");
	}

	public void DropdownResolutions(int index)
	{
		option.Resolution = resolutions[index];
	}

	public void ToggleJudge(bool value)
	{
		if (!value) return;
		var toggle = EventSystem.current.currentSelectedGameObject;
		switch (toggle.name)
		{
			case "ChkBoxEasy":
				option.JudgeLevel = 1.2f;
				break;
			case "ChkBoxNormal":
				option.JudgeLevel = 0.8f;
				break;
			case "ChkBoxHard":
				option.JudgeLevel = 0.6f;
				break;
			case "ChkBoxEx":
				option.JudgeLevel = 0.4f;
				break;
		}
	}

	public void TogglePerformance(bool value)
	{
		var toggle = EventSystem.current.currentSelectedGameObject;
		switch (toggle.name)
		{
			case "ChkBoxFrostedGlass":
				option.FrostedGlassEffect = value;
				break;
			case "ChkBoxVSync":
				option.VSync = value;
				break;
			case "ChkBoxShowFPS":
				option.ShowFPS = value;
				break;
			case "ChkBoxLimitFPS":
				option.LimitFPS = value;
				break;
		}
		var performance = transform.Find("GroupSystem/BarPerformance");
		var chkBoxLimitFPS = performance.Find("ChkBoxLimitFPS");
		if (!option.VSync)
		{
			chkBoxLimitFPS.gameObject.SetActive(true);
		}
		else
		{
			chkBoxLimitFPS.gameObject.SetActive(false);
		}
		var sliderLimitFPS = chkBoxLimitFPS.Find("SliderLimitFPS");
		if (option.LimitFPS) sliderLimitFPS.gameObject.SetActive(true);
		else sliderLimitFPS.gameObject.SetActive(false);

		PersistentCanvas.Instance.Text_FPS.enabled = option.ShowFPS;
	}

	public void TogglePanelPosition(bool value)
	{
		if (!value) return;
		var toggle = EventSystem.current.currentSelectedGameObject;
		switch (toggle.name)
		{
			case "ChkBoxLeft":
				option.PanelPosition = EZR.Option.PanelPositionEnum.Left;
				break;
			case "ChkBoxCenter":
				option.PanelPosition = EZR.Option.PanelPositionEnum.Center;
				break;
			case "ChkBoxRight":
				option.PanelPosition = EZR.Option.PanelPositionEnum.Right;
				break;
		}
	}

	public void ToggleTargetLineType(bool value)
	{
		if (!value) return;
		var toggle = EventSystem.current.currentSelectedGameObject;
		switch (toggle.name)
		{
			case "ChkBoxNew":
				option.TargetLineType = EZR.Option.TargetLineTypeEnum.New;
				break;
			case "ChkBoxClassic":
				option.TargetLineType = EZR.Option.TargetLineTypeEnum.Classic;
				break;
		}
	}

	public void ToggleETC(bool value)
	{
		var toggle = EventSystem.current.currentSelectedGameObject;
		switch (toggle.name)
		{
			case "ChkBoxShowFastSlow":
				option.ShowFastSlow = value;
				break;
			case "ChkBoxShowJudgeList":
				option.ShowJudgeList = value;
				break;
			case "ChkBoxShowPercent":
				option.ShowPercent = value;
				break;
			case "ChkBoxUiJudgeLine":
				option.UiJudgeLine = value;
				break;
		}
	}

	public void ToggleBGA(bool value)
	{
		var toggle = EventSystem.current.currentSelectedGameObject;
		switch (toggle.name)
		{
			case "ChkBoxBGA":
				option.ShowBGA = value;
				break;
		}
	}

	public void SliderLimitFPS(float value)
	{
		option.TargetFrameRate = (int)value;
		sliderLimitFPSText.text = option.TargetFrameRate.ToString();
	}

	public void SliderTimming(float value)
	{
		option.TimePrecision = (int)value;
		sliderTimmingText.text = option.TimePrecision.ToString() + "ms";
	}

	public void SliderAlpha(float value)
	{
		var slider = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
		if (slider == null) return;
		Text text_Slider = slider.transform.Find("Text_Slider").GetComponent<Text>();
		switch (slider.name)
		{
			case "GearAlpha":
				option.GearAlpha = (int)slider.value;
				text_Slider.text = string.Format("{0}%", slider.value);
				break;
			case "BGAlpha":
				option.BGAlpha = (int)slider.value;
				text_Slider.text = string.Format("{0}%", slider.value);
				break;
			case "HPAlpha":
				option.HPAlpha = (int)slider.value;
				text_Slider.text = string.Format("{0}%", slider.value);
				break;
			case "JudgeAlpha":
				option.JudgeAlpha = (int)slider.value;
				text_Slider.text = string.Format("{0}%", slider.value);
				break;
			case "ComboAlpha":
				option.ComboAlpha = (int)slider.value;
				text_Slider.text = string.Format("{0}%", slider.value);
				break;
		}
	}

	public void SliderBright(float value)
	{
		var slider = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
		if (slider == null) return;
		Text text_Slider = slider.transform.Find("Text_Slider").GetComponent<Text>();
		switch (slider.name)
		{
			case "BGABright":
				option.BGABright = (int)slider.value;
				text_Slider.text = string.Format("{0}%", slider.value);
				break;
		}
	}

	public void BtnSwitchTag()
	{
		var btn = EventSystem.current.currentSelectedGameObject.GetComponent<EZR.ButtonExtension>();
		if (btn.IsSelected) return;

		transform.Find("GroupSystem").gameObject.SetActive(false);
		transform.Find("GroupSound").gameObject.SetActive(false);
		transform.Find("GroupInput").gameObject.SetActive(false);
		transform.Find("GroupSkin").gameObject.SetActive(false);
		transform.Find("GroupBGA").gameObject.SetActive(false);

		switch (btn.gameObject.name)
		{
			case "BtnSystem":
				transform.Find("GroupSystem").gameObject.SetActive(true);
				break;
			case "BtnSound":
				transform.Find("GroupSound").gameObject.SetActive(true);
				break;
			case "BtnInput":
				transform.Find("GroupInput").gameObject.SetActive(true);
				break;
			case "BtnSkin":
				transform.Find("GroupSkin").gameObject.SetActive(true);
				break;
			case "BtnBGA":
				transform.Find("GroupBGA").gameObject.SetActive(true);
				break;
		}
		updateSystem();
		btn.SetSelected(true);
		EZR.MemorySound.PlaySound("e_click");
	}

	public void OnClickButtonInput()
	{
		var button = EventSystem.current.currentSelectedGameObject;

		var parent = button.transform.parent.parent;
		string[] split = parent.name.Split('_');
		int key = int.Parse(split[1]);
		string buttonName = button.name;
		switch (buttonName)
		{
			case "Button_Key_1":
			case "Button_Key_2":
			case "Button_Key_3":
			case "Button_Key_4":
			case "Button_Key_5":
			case "Button_Key_6":
			case "Button_Key_7":
			case "Button_Key_8":
				{
					string[] sp = button.name.Split('_');
					int keyIndex = int.Parse(sp[2]);
					Text t = button.transform.Find("Text").GetComponent<Text>();
					transform.Find("GroupInput/Popup_PressKey").GetComponent<Popup_PressKey>().Show(t, key - 4, keyIndex - 1, OnKeyPressed);
				}
				break;
		}
	}

	private void OnKeyPressed(int keyType, int keyIndex, int keyCode)
	{
		option.KeyMapping[keyType, keyIndex] = keyCode;
	}

    void updateJudgmentOffset()
    {
        var value = transform.Find("GroupSkin/BarOffset/UpDownOffset/Value").GetComponent<Text>();
        if (option.JudgmentOffset == 0)
        {
            value.text = "0";
        }
        else if (option.JudgmentOffset > 0)
        {
            value.text = "+" + option.JudgmentOffset;
        }
        else if (option.JudgmentOffset < 0)
        {
            value.text = option.JudgmentOffset.ToString();
        }
    }

    Coroutine offsetDelayCoroutine;
    public void BtnOffsetSubtractDown()
    {
        addOffset(-1);
        if (offsetDelayCoroutine != null) StopCoroutine(offsetDelayCoroutine);
        offsetDelayCoroutine = StartCoroutine(offsetDelay(-1));
    }
    public void BtnOffsetAddDown()
    {
        addOffset(1);
        if (offsetDelayCoroutine != null) StopCoroutine(offsetDelayCoroutine);
        offsetDelayCoroutine = StartCoroutine(offsetDelay(1));
    }
    public void BtnOffsetUp()
    {
        StopCoroutine(offsetDelayCoroutine);
        offsetDelayCoroutine = null;
    }
    IEnumerator offsetDelay(int value)
    {
        yield return new WaitForSeconds(0.2f);
        for (; ; )
        {
            yield return new WaitForSeconds(0.075f);
            addOffset(value);
        }
    }

    void addOffset(int value)
    {
        option.JudgmentOffset += value;
        updateJudgmentOffset();
        EZR.MemorySound.PlaySound("e_click");
    }

    public void SliderVolume(float value)
    {
        var slider = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
        if (slider == null) return;
        switch (slider.name)
        {
            case "SliderMasterVolume":
                option.Volume.Master = (int)slider.value;
                break;
            case "SliderGameVolume":
                option.Volume.Game = (int)slider.value;
                break;
            case "SliderMainVolume":
                option.Volume.Main = (int)slider.value;
                break;
            case "SliderBGMVolume":
                option.Volume.BGM = (int)slider.value;
                break;

        }
    }

    public void ToggleSound(bool value)
    {
        var toggle = EventSystem.current.currentSelectedGameObject;
        switch (toggle.name)
        {
            case "ChkBoxLive3D":
                option.Volume.Live3D = value;
                break;
        }
    }

    public void ClickSound()
    {
        EZR.MemorySound.PlaySound("e_click");
    }

    public void Apply()
    {
		EZR.Option.ApplyKeyMapping(option);

		EZR.UserSaveData.SetOption(option);
        EZR.UserSaveData.SaveData();

        EZR.Option.ApplyOption(option);

        Destroy(gameObject);
        EZR.MemorySound.PlaySound("e_click");
    }


    public void Close()
    {
        Destroy(gameObject);
        EZR.MemorySound.PlaySound("e_click");
    }
}
