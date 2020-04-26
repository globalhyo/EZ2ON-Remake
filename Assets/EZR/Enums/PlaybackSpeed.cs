/*
  ============================================
	Author	: jinnin0105
	Time 	: 2020-04-26 오후 12:47:13
  ============================================
*/

namespace EZR
{
	public static class PlaybackSpeed
	{
		public const float NONE = 1f;
		public const float NC_HD = 1.25f;
		public const float NC_SC = 1.5f;

		public const string NONE_Name = "NONE";
		public const string NCHD_Name = "NC HD";
		public const string NCSC_Name = "NC SC";

		public static float GetSpeed(this PlaybackType playbackType)
		{
			switch (playbackType)
			{
				default:
					return NONE;
				case PlaybackType.NC_HD:
					return NC_HD;
				case PlaybackType.NC_SC:
					return NC_SC;
			}
		}
	}

	public enum PlaybackType
	{
		NONE,
		NC_HD,
		NC_SC
	}
}
