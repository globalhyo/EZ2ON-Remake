/*
  ============================================
	Author	: KJH
	Time 	: 2020-04-22 오후 4:21:43
  ============================================
*/

using System;

public static partial class Utils
{
	public static T[] Randomize<T>(this T[] items, int start = -1, int end = -1)
	{
		Random rand = new Random();

		if (start < 0)
			start = 0;

		if (end < 0)
			end = items.Length;

		for (int i = start; i < end; i++)
		{
			int j = rand.Next(i, end);
			T temp = items[i];
			items[i] = items[j];
			items[j] = temp;
		}

		return items;
	}
}