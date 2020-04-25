using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

namespace EZR
{
    public static class UserSaveData
    {
        public static string Version => "1.4";
        public static string MinVer => "1.4";
        public static JObject UserData = new JObject(new JProperty("version", Version));
        static string aesKey = "LodasEZ2OFF2020";
        static string saveName = "userdata.save";

        static byte[] getKeyBytes
        {
            get
            {
                var key = new byte[32];

                for (int i = 0; i < key.Length; i++)
                {
                    key[i] = (byte)aesKey[i % aesKey.Length];
                }

                return key;
            }
        }

        static byte[] getIvBytes
        {
            get
            {
                var iv = new byte[16];

                for (int i = 0; i < iv.Length; i++)
                {
                    iv[i] = (byte)aesKey[i % aesKey.Length];
                }

                return iv;
            }
        }

        public static void SaveData()
        {
            using (var aes = Aes.Create())
            {
                aes.Key = getKeyBytes;
                aes.IV = getIvBytes;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            UserData["version"] = Version;
                            swEncrypt.Write(UserData.ToString(Formatting.None));
                        }
                        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, saveName), msEncrypt.ToArray());
                    }
                }
            }
            Debug.Log("Save user data...");
        }

        public static void LoadSave()
        {
            var fullPath = Path.Combine(Application.persistentDataPath, saveName);
            if (!File.Exists(fullPath)) return;
            byte[] cipherText = File.ReadAllBytes(fullPath);
            using (var aes = Aes.Create())
            {
                aes.Key = getKeyBytes;
                aes.IV = getIvBytes;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        try
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                var plaintext = srDecrypt.ReadToEnd();
                                var userData = JObject.Parse(plaintext);
                                if (!string.IsNullOrEmpty(((string)userData["version"])) &&
                                EZR.Utils.Version2Decmal((string)userData["version"]) >= EZR.Utils.Version2Decmal(MinVer))
                                    UserData = userData;
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        public static int GetScore(string name, GameType type, GameMode.Mode mode, GameDifficult.Difficult diff)
        {
            JObject jobj;
            if (!UserData.ContainsKey("myBestScore")) return 0;
            jobj = (JObject)UserData["myBestScore"];
            if (!jobj.ContainsKey(type.ToString())) return 0;
            jobj = (JObject)jobj[type.ToString()];
            if (!jobj.ContainsKey(name)) return 0;
            jobj = (JObject)jobj[name];
            if (!jobj.ContainsKey(mode.ToString())) return 0;
            jobj = (JObject)jobj[mode.ToString()];
            if (!jobj.ContainsKey(diff.ToString())) return 0;
            return (int)jobj[diff.ToString()];
        }
        public static bool SetScore(int score, string name, GameType type, GameMode.Mode mode, GameDifficult.Difficult diff)
        {
            if (score == 0) return false;
            JObject jobj;
            if (!UserData.ContainsKey("myBestScore"))
                UserData["myBestScore"] = new JObject();
            jobj = (JObject)UserData["myBestScore"];
            if (!jobj.ContainsKey(type.ToString()))
                jobj[type.ToString()] = new JObject();
            jobj = (JObject)jobj[type.ToString()];
            if (!jobj.ContainsKey(name))
                jobj[name] = new JObject();
            jobj = (JObject)jobj[name];
            if (!jobj.ContainsKey(mode.ToString()))
                jobj[mode.ToString()] = new JObject();
            jobj = (JObject)jobj[mode.ToString()];
            if (!jobj.ContainsKey(diff.ToString()))
            {
                jobj[diff.ToString()] = score;
                return true;
            }
            var bestScore = (int)jobj[diff.ToString()];
            if (score > bestScore)
            {
                jobj[diff.ToString()] = score;
                return true;
            }
            else return false;
        }

        public static Option GetOption()
        {
			Option option = new Option();
			if (!UserData.ContainsKey("setting")) return option;

            option.FullScreenMode = Utils.ParseEnum<FullScreenMode>((string)UserData["setting"]["fullScreenMode"] ?? option.FullScreenMode.ToString());
            option.Resolution = new Resolution()
            {
                width = (int)(UserData["setting"]["resolution"]["width"] ?? option.Resolution.width),
                height = (int)(UserData["setting"]["resolution"]["height"] ?? option.Resolution.height)
            };
            option.Language = Utils.ParseEnum<SystemLanguage>((string)UserData["setting"]["language"] ?? option.Language.ToString());
            option.TimePrecision = (int)(UserData["setting"]["timePrecision"] ?? option.TimePrecision);
            option.FrostedGlassEffect = (bool)(UserData["setting"]["frostedGlassEffect"] ?? option.FrostedGlassEffect);
            option.VSync = (bool)(UserData["setting"]["vSync"] ?? option.VSync);
			option.LimitFPS = (bool)(UserData["setting"]["limitFPS"] ?? option.LimitFPS);
            option.TargetFrameRate = (int)(UserData["setting"]["targetFrameRate"] ?? option.TargetFrameRate);
            option.PanelPosition = Utils.ParseEnum<Option.PanelPositionEnum>((string)(UserData["setting"]["panelPosition"] ?? option.PanelPosition.ToString()));
            option.TargetLineType = Utils.ParseEnum<Option.TargetLineTypeEnum>((string)(UserData["setting"]["targetLineType"] ?? option.TargetLineType.ToString()));
            option.JudgmentOffset = (int)(UserData["setting"]["judgmentOffset"] ?? option.JudgmentOffset);
            option.ShowFastSlow = (bool)(UserData["setting"]["showFastSlow"] ?? option.ShowFastSlow);
            option.Volume.Master = Mathf.Clamp((int)(UserData["setting"]["volume"]["master"] ?? option.Volume.Master), 0, 100);
            option.Volume.Game = Mathf.Clamp((int)(UserData["setting"]["volume"]["game"] ?? option.Volume.Game), 0, 100);
            option.Volume.Main = Mathf.Clamp((int)(UserData["setting"]["volume"]["main"] ?? option.Volume.Main), 0, 100);
            option.Volume.BGM = Mathf.Clamp((int)(UserData["setting"]["volume"]["bgm"] ?? option.Volume.BGM), 0, 100);
            option.Volume.Live3D = (bool)(UserData["setting"]["volume"]["live3D"] ?? option.Volume.Live3D);

			option.ShowFPS = (bool)(UserData["setting"]["ShowFPS"] ?? option.ShowFPS);
			option.ShowBGA = (bool)(UserData["setting"]["ShowBGA"] ?? option.ShowBGA);
			option.JudgeLevel = (float)(UserData["setting"]["JudgeLevel"] ?? option.JudgeLevel);
			option.ShowJudgeList = (bool)(UserData["setting"]["ShowJudgeList"] ?? option.ShowJudgeList);
			option.ShowPercent = (bool)(UserData["setting"]["ShowPercent"] ?? option.ShowPercent);
			option.UiJudgeLine = (bool)(UserData["setting"]["UiJudgeLine"] ?? option.UiJudgeLine);

			option.GearAlpha = (int)(UserData["setting"]["GearAlpha"] ?? option.GearAlpha);
			option.BGAlpha = (int)(UserData["setting"]["BGAlpha"] ?? option.BGAlpha);
			option.HPAlpha = (int)(UserData["setting"]["HPAlpha"] ?? option.HPAlpha);
			option.JudgeAlpha = (int)(UserData["setting"]["JudgeAlpha"] ?? option.JudgeAlpha);
			option.ComboAlpha = (int)(UserData["setting"]["ComboAlpha"] ?? option.ComboAlpha);
			option.BGABright = (int)(UserData["setting"]["BGABright"] ?? option.BGABright);

			if (UserData["setting"]["KeyConfig"] == null) return option;

			int xLen = option.KeyMapping.GetLength(0), yLen = option.KeyMapping.GetLength(1);
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					option.KeyMapping[x, y] = (int)(UserData["setting"]["KeyConfig"][string.Format("{0}_{1}", x, y)] ?? option.KeyMapping[x, y]);
				}
			}

			return option;
        }

		public static void SetOption(Option option)
		{
			JObject jobj;
			if (!UserData.ContainsKey("setting"))
				UserData["setting"] = new JObject();
			jobj = (JObject)UserData["setting"];
			jobj["fullScreenMode"] = option.FullScreenMode.ToString();
			if (!jobj.ContainsKey("resolution"))
				jobj["resolution"] = new JObject();
			jobj["resolution"]["width"] = option.Resolution.width;
			jobj["resolution"]["height"] = option.Resolution.height;
			jobj["language"] = option.Language.ToString();
			jobj["timePrecision"] = option.TimePrecision;
			jobj["frostedGlassEffect"] = option.FrostedGlassEffect;
			jobj["vSync"] = option.VSync;
			jobj["limitFPS"] = option.LimitFPS;
			jobj["targetFrameRate"] = option.TargetFrameRate;
			jobj["panelPosition"] = option.PanelPosition.ToString();
			jobj["targetLineType"] = option.TargetLineType.ToString();
			jobj["judgmentOffset"] = option.JudgmentOffset;
			jobj["showFastSlow"] = option.ShowFastSlow;
			if (!jobj.ContainsKey("volume"))
				jobj["volume"] = new JObject();
			jobj["volume"]["master"] = option.Volume.Master;
			jobj["volume"]["game"] = option.Volume.Game;
			jobj["volume"]["main"] = option.Volume.Main;
			jobj["volume"]["bgm"] = option.Volume.BGM;
			jobj["volume"]["live3D"] = option.Volume.Live3D;

			jobj["ShowFPS"] = option.ShowFPS;
			jobj["ShowBGA"] = option.ShowBGA;
			jobj["JudgeLevel"] = option.JudgeLevel;
			jobj["ShowJudgeList"] = option.ShowJudgeList;
			jobj["ShowPercent"] = option.ShowPercent;
			jobj["UiJudgeLine"] = option.UiJudgeLine;

			jobj["GearAlpha"] = option.GearAlpha;
			jobj["BGAlpha"] = option.BGAlpha;
			jobj["HPAlpha"] = option.HPAlpha;
			jobj["JudgeAlpha"] = option.JudgeAlpha;
			jobj["ComboAlpha"] = option.ComboAlpha;
			jobj["BGABright"] = option.BGABright;

			if (jobj.ContainsKey("KeyConfig") == false)
				jobj["KeyConfig"] = new JObject();
			int xLen = option.KeyMapping.GetLength(0), yLen = option.KeyMapping.GetLength(1);
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					jobj["KeyConfig"][string.Format("{0}_{1}", x, y)] = option.KeyMapping[x, y];
				}
			}
		}

        public static Dictionary<string, string> GetInventory()
        {
            var inventory = new Dictionary<string, string>()
            {
                ["panelResource"] = "R14",
                ["noteResource"] = "Note_04"
            };
            if (!UserData.ContainsKey("inventory")) return inventory;
            inventory["panelResource"] = (string)UserData["inventory"]["panelResource"] ?? inventory["panelResource"];
            inventory["noteResource"] = (string)UserData["inventory"]["noteResource"] ?? inventory["noteResource"];
            return inventory;
        }

        public static void SetInventory()
        {
            JObject jobj;
            if (!UserData.ContainsKey("inventory"))
                UserData["inventory"] = new JObject();
            jobj = (JObject)UserData["inventory"];
            jobj["panelResource"] = EZR.DisplayLoop.PanelResource;
            jobj["noteResource"] = EZR.DisplayLoop.NoteResource;
        }
    }
}