﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.IO;

public class SingleResultUI : MonoBehaviour
{
    public Sprite GradeA;
    public Sprite GradeB;
    public Sprite GradeC;
    public Sprite GradeD;
    public Sprite GradeF;
    public Sprite BonusAC;
    public Sprite BonusACC;
    public Sprite BonusAK;
    public Sprite ClearFailed;
    public Sprite ClearSuccess;
    public Color ColorAC = Color.white;
    public Color ColorACC = Color.white;
    public Color ColorAK = Color.white;
    public Color EzColor = Color.white;
    public Color NmColor = Color.white;
    public Color HdColor = Color.white;
    public Color ShdColor = Color.white;
    public Color[] LinesColor = new Color[EZR.PlayManager.MaxLines - 3];

    void Start()
    {
        var info = EZR.SongsList.List[EZR.SongsList.currentIndex];
        transform.Find("SongName").GetComponent<Text>().text = info.displayName.ToUpper();
        var diffText = transform.Find("Difficulty").GetComponent<Text>();
        diffText.text = EZR.GameDifficulty.GetFullName(EZR.PlayManager.GameDifficult).ToUpper();
        switch (EZR.PlayManager.GameDifficult)
        {
            case EZR.GameDifficulty.Difficulty.EZ:
                diffText.color = EzColor;
                break;
            case EZR.GameDifficulty.Difficulty.NM:
                diffText.color = NmColor;
                break;
            case EZR.GameDifficulty.Difficulty.HD:
                diffText.color = HdColor;
                break;
            case EZR.GameDifficulty.Difficulty.SHD:
                diffText.color = ShdColor;
                break;
            case EZR.GameDifficulty.Difficulty.DJMAX_NM:
                diffText.color = NmColor;
                break;
            case EZR.GameDifficulty.Difficulty.DJMAX_HD:
                diffText.color = HdColor;
                break;
            case EZR.GameDifficulty.Difficulty.DJMAX_MX:
                diffText.color = ShdColor;
                break;
        }
        var keyMode = transform.Find("KeyMode").GetComponent<Text>();
        var numLines = EZR.GameMode.GetNumLines(EZR.PlayManager.GameMode);
        keyMode.text = numLines.ToString();
        keyMode.color = LinesColor[numLines - 4];
        transform.Find("ResultGroup/TotalNote").GetComponent<Text>().text = EZR.PlayManager.Score.TotalNote.ToString();
        transform.Find("ResultGroup/Combo").GetComponent<Text>().text = EZR.PlayManager.Score.MaxCombo.ToString();
        transform.Find("ResultGroup/Kool").GetComponent<Text>().text = EZR.PlayManager.Score.Kool.ToString();
        transform.Find("ResultGroup/Cool").GetComponent<Text>().text = EZR.PlayManager.Score.Cool.ToString();
        transform.Find("ResultGroup/Good").GetComponent<Text>().text = EZR.PlayManager.Score.Good.ToString();
        transform.Find("ResultGroup/Miss").GetComponent<Text>().text = EZR.PlayManager.Score.Miss.ToString();
        transform.Find("ResultGroup/Fail").GetComponent<Text>().text = EZR.PlayManager.Score.Fail.ToString();
        var bonus = EZR.PlayManager.Score.GetBonus();
        var bonusText = transform.Find("ResultGroup/Bonus").GetComponent<Text>();
        var bonusImage = transform.Find("Bonus").GetComponent<Image>();
        bonusText.text = EZR.Score.GetBounsFullName(bonus);
        switch (bonus)
        {
            case EZR.Score.Bonus.AllKool:
                bonusText.color = ColorAK;
                bonusImage.overrideSprite = BonusAK;
                break;
            case EZR.Score.Bonus.AllCool:
                bonusText.color = ColorACC;
                bonusImage.overrideSprite = BonusACC;
                break;
            case EZR.Score.Bonus.AllCombo:
                bonusText.color = ColorAC;
                bonusImage.overrideSprite = BonusAC;
                break;
            default:
                bonusImage.enabled = false;
                break;
        }
        transform.Find("ResultGroup/Bonus/Add").GetComponent<Text>().text = "+" + (int)bonus;
        var score = EZR.PlayManager.Score.GetScore();
        transform.Find("ResultGroup/Score").GetComponent<Text>().text = score.ToString();
        transform.Find("ResultGroup/Exp").gameObject.SetActive(false);
        // TODO
        // transform.Find("ResultGroup/Exp").GetComponent<Text>().text =
        var bestScore = EZR.UserSaveData.GetScore(EZR.PlayManager.SongName, EZR.PlayManager.GameType, EZR.PlayManager.GameMode, EZR.PlayManager.GameDifficult);
        var isNewRecord = EZR.UserSaveData.SetScore(score, EZR.PlayManager.SongName, EZR.PlayManager.GameType, EZR.PlayManager.GameMode, EZR.PlayManager.GameDifficult);
        EZR.UserSaveData.SaveData();
        var bestScoreText = transform.Find("MyBestScore").GetComponent<Text>();
        if (isNewRecord)
            bestScoreText.text = score.ToString();
        else
        {
            bestScoreText.text = bestScore.ToString();
            transform.Find("NewRecord").gameObject.SetActive(false);
        }
        var gradeImage = transform.Find("Grade").GetComponent<Image>();
        var grade = EZR.PlayManager.Score.GetGrade();
        switch (grade)
        {
            case EZR.Score.Grade.A:
                gradeImage.overrideSprite = GradeA;
                break;
            case EZR.Score.Grade.B:
                gradeImage.overrideSprite = GradeB;
                break;
            case EZR.Score.Grade.C:
                gradeImage.overrideSprite = GradeC;
                break;
            case EZR.Score.Grade.D:
                gradeImage.overrideSprite = GradeD;
                break;
            case EZR.Score.Grade.F:
                gradeImage.overrideSprite = GradeF;
                break;
        }
        var clear = transform.Find("Clear").GetComponent<Image>();
        if (EZR.PlayManager.Score.IsClear)
            clear.overrideSprite = ClearSuccess;
        else
            clear.overrideSprite = ClearFailed;
        string fileName = "";
        switch (EZR.PlayManager.GameType)
        {
            case EZR.GameType.EZ2ON:
                fileName = "big_" + EZR.PlayManager.SongName + EZR.GameDifficulty.GetString(EZR.PlayManager.GameDifficult) + ".png";
                break;
            case EZR.GameType.EZ2DJ:
                fileName = EZR.PlayManager.SongName + EZR.GameDifficulty.GetString(EZR.PlayManager.GameDifficult) + ".bmp";
                break;
            case EZR.GameType.DJMAX:
                fileName = "song_pic_f_" + EZR.PlayManager.SongName + "_" + ((int)EZR.PlayManager.GameDifficult - 3).ToString().PadLeft(2, '0') + ".png";
                break;
        }

        var buffer = EZR.ZipLoader.LoadFile(Path.Combine(EZR.Master.GameResourcesFolder, EZR.PlayManager.GameType.ToString(), "Songs", EZR.PlayManager.SongName + ".zip"), fileName);
        if (buffer != null)
        {
            transform.Find("Disc").GetComponent<RawImage>().texture = EZR.ImageLoader.Load(buffer, fileName);
        }
    }
}