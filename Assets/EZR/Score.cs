﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZR
{
    public class Score
    {
        public float RawScore = 0;
        public int Kool = 0;
        public int Cool = 0;
        public int Good = 0;
        public int Miss = 0;
        public int Fail = 0;
        public int MaxCombo = 0;
        public int TotalNote = 0;
        public bool IsClear = false;

        public enum Bonus
        {
            NoBonus = 0,
            AllCombo = 10000,
            AllCool = 30000,
            AllKool = 50000
        }

        public static string GetBounsFullName(Bonus bonus)
        {
            switch (bonus)
            {
                case Bonus.AllKool: return "All kool";
                case Bonus.AllCool: return "All cool";
                case Bonus.AllCombo: return "All combo";
                default: return "No bonus";
            }
        }

        public enum Grade
        {
            F,
            D,
            C,
            B,
            A,
            APlus
        }

        // 分数公式
        public void AddScore(JudgmentType judgment, int combo)
        {
            switch (judgment)
            {
                case JudgmentType.Kool:
                    Kool++;
                    RawScore += 170 + 17 * Mathf.Log(combo, 2);
                    break;
                case JudgmentType.Cool:
                    Cool++;
                    RawScore += 100 + 10 * Mathf.Log(combo, 2);
                    break;
                case JudgmentType.Good:
                    Good++;
                    RawScore += 40 + 4 * Mathf.Log(combo, 2);
                    break;
                case JudgmentType.Miss:
                    Miss++;
                    break;
                case JudgmentType.Fail:
                    Fail++;
                    break;
            }
        }

        public int GetScore()
        {
            return (int)Mathf.Round(RawScore + (int)GetBonus());
        }

        public Grade GetGrade()
        {
            var rate = (Kool + Cool) / (float)TotalNote;
            if (rate >= 0.98f) return Grade.APlus;
            else if (rate < 0.98f && rate >= 0.95f) return Grade.A;
            else if (rate < 0.95f && rate >= 0.88f) return Grade.B;
            else if (rate < 0.88f && rate >= 0.82f) return Grade.C;
            else if (rate < 0.82f && rate >= 0.75f) return Grade.D;
            else return Grade.F;
        }

        public Bonus GetBonus()
        {
            if (Kool == TotalNote) return Bonus.AllKool;
            if (Kool + Cool == TotalNote) return Bonus.AllCool;
            if (MaxCombo == TotalNote) return Bonus.AllKool;
            return Bonus.NoBonus;
        }

        public void Reset()
        {
            RawScore = 0;
            Kool = 0;
            Cool = 0;
            Good = 0;
            Miss = 0;
            Fail = 0;
            MaxCombo = 0;
            IsClear = false;
        }
    }
}
