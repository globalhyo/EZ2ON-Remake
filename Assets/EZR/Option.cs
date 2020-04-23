using System.Collections.Generic;
using UnityEngine;

namespace EZR
{
    public class Option
    {
        public enum PanelPositionEnum
        {
            Left = -680,
            Center = 0,
            Right = 680
        }

        public enum TargetLineTypeEnum
        {
            Classic,
            New = 30
        }
        public class VolumeClass
        {
            public int Master = 100;
            public int Game = 100;
            public int Main = 100;
            public int BGM = 100;
            public bool Live3D = false;
        }
        public FullScreenMode FullScreenMode = FullScreenMode.FullScreenWindow;
        public Resolution Resolution = Screen.resolutions[Screen.resolutions.Length - 1];
        public SystemLanguage Language = SystemLanguage.Korean;
        public int TimePrecision = 1;
        public bool FrostedGlassEffect = false;
        public bool VSync = true;
		public bool ShowFPS = false;
        public bool LimitFPS = false;
        public int TargetFrameRate = 60;
		public bool ShowBGA = true;
        public PanelPositionEnum PanelPosition = PanelPositionEnum.Center;
        public TargetLineTypeEnum TargetLineType = TargetLineTypeEnum.Classic;
        public int JudgmentOffset = 0;
        public bool ShowFastSlow = false;
		public bool ShowPercent = false;
		public bool UiJudgeLine = false;
		public VolumeClass Volume = new VolumeClass();
		public float JudgeLevel = 0.8f;
		public bool ShowJudgeList = false;

		public int GearAlpha = 100;
		public int BGAlpha = 100;
		public int HPAlpha = 100;
		public int JudgeAlpha = 100;
		public int ComboAlpha = 100;

		public int[,] KeyMapping = new int[,]
		{
			{ (int)KeyCodeVK.D, (int)KeyCodeVK.F, (int)KeyCodeVK.J, (int)KeyCodeVK.K, 0, 0, 0, 0 },
			{ (int)KeyCodeVK.D, (int)KeyCodeVK.F, (int)KeyCodeVK.Space, (int)KeyCodeVK.J, (int)KeyCodeVK.K, 0, 0, 0 },
			{ (int)KeyCodeVK.S, (int)KeyCodeVK.D, (int)KeyCodeVK.F, (int)KeyCodeVK.J, (int)KeyCodeVK.K, (int)KeyCodeVK.L, 0, 0 },
			{ (int)KeyCodeVK.S, (int)KeyCodeVK.D, (int)KeyCodeVK.F, (int)KeyCodeVK.Space, (int)KeyCodeVK.J, (int)KeyCodeVK.K, (int)KeyCodeVK.L, 0 },
			{ (int)KeyCodeVK.LeftShift, (int)KeyCodeVK.A, (int)KeyCodeVK.S, (int)KeyCodeVK.D, (int)KeyCodeVK.L, (int)KeyCodeVK.Semicolon, (int)KeyCodeVK.Quote, (int)KeyCodeVK.RightShift },
			{ (int)KeyCodeVK.F2, (int)KeyCodeVK.F3, (int)KeyCodeVK.F4, 0, 0, 0, 0, 0 }
		};

		public static void ApplyOption(Option option)
        {
            // 设置画面模式
            if (option.VSync) QualitySettings.vSyncCount = 1;
            else QualitySettings.vSyncCount = 0;
            if (option.LimitFPS)
                Application.targetFrameRate = option.TargetFrameRate;
            else
                Application.targetFrameRate = -1;
            if (option.Resolution.width != Screen.currentResolution.width ||
            option.Resolution.height != Screen.currentResolution.height ||
            option.FullScreenMode != Screen.fullScreenMode)
                Screen.SetResolution(option.Resolution.width, option.Resolution.height, option.FullScreenMode);
            // 设置时间粒度
            EZR.Master.TimePrecision = option.TimePrecision;
            // 设置音量
            EZR.MemorySound.MasterVolume = option.Volume.Master / 100f * 0.7f;
            EZR.MemorySound.GameVolume = option.Volume.Game / 100f;
            EZR.MemorySound.MainVolume = option.Volume.Main / 100f;
            EZR.MemorySound.BGMVolume = option.Volume.BGM / 100f;
            if (option.Volume.Live3D)
            {
                var prop = FMOD.PRESET.CONCERTHALL();
                FMODUnity.RuntimeManager.CoreSystem.setReverbProperties(0, ref prop);
            }
            else
            {
                var prop = FMOD.PRESET.OFF();
                FMODUnity.RuntimeManager.CoreSystem.setReverbProperties(0, ref prop);
            }
        }

		public static void ApplyKeyMapping(Option option)
		{
			int xLen = Master.KeyCodeMapping.GetLength(0);
			int yLen = Master.KeyCodeMapping.GetLength(1);

			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					Master.KeyCodeMapping[x, y] = (char)option.KeyMapping[x, y];
				}
			}
		}
    }
}