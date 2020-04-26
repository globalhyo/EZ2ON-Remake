namespace EZR
{
    public static class GameDifficult
    {
		private const float JudgeEZ = 1.2f;
		private const float JudgeNM = 0.8f;
		private const float JudgeHD = 0.6f;
		private const float JudgeEX = 0.4f;

		public enum Difficult
        {
            EZ,
            NM,
            HD,
            SHD,
            DJMAX_EZ,
            DJMAX_NM,
            DJMAX_HD,
            DJMAX_MX,
            DJMAX_SC
        }

		public enum JudgeDifficult
		{
			EZ,
			NM,
			HD,
			EX
		}

		public static float GetJudge(this JudgeDifficult difficult)
		{
			switch (difficult)
			{
				default:
					return JudgeEZ;
				case JudgeDifficult.NM:
					return JudgeNM;
				case JudgeDifficult.HD:
					return JudgeHD;
				case JudgeDifficult.EX:
					return JudgeEX;
			}
		}

        public static string GetString(Difficult difficult)
        {
            switch (difficult)
            {
                case Difficult.EZ:
                    return "-ez";
                case Difficult.NM:
                    return "";
                case Difficult.HD:
                    return "-hd";
                case Difficult.SHD:
                    return "-shd";
                case Difficult.DJMAX_EZ:
                    return "_ez";
                case Difficult.DJMAX_NM:
                    return "_nm";
                case Difficult.DJMAX_HD:
                    return "_hd";
                case Difficult.DJMAX_MX:
                    return "_mx";
                case Difficult.DJMAX_SC:
                    return "_sc";
                default:
                    return null;
            }
        }

        public static string GetFullName(Difficult difficult)
        {
            switch (difficult)
            {
                case Difficult.EZ:
                    return "Easy mix";
                case Difficult.NM:
                    return "Normal mix";
                case Difficult.HD:
                    return "Hard mix";
                case Difficult.SHD:
                    return "SuperHard mix";
                case Difficult.DJMAX_EZ:
                    return "Easy mode";
                case Difficult.DJMAX_NM:
                    return "Normal mode";
                case Difficult.DJMAX_HD:
                    return "Hard mode";
                case Difficult.DJMAX_MX:
                    return "Maximum mode";
                case Difficult.DJMAX_SC:
                    return "SuperCrazy mode";
                default:
                    return null;
            }
        }
    }
}