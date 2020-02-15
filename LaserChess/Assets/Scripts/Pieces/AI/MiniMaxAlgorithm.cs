using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxAlgorithm
{
	static int Minimax(int depth, int nodeIndex, bool isMax, int[] scores, int h)
	{
		if (depth == h)
			return scores[nodeIndex];

		if (isMax)
		{
			return Math.Max(Minimax(depth + 1, nodeIndex * 2, false, scores, h),
					Minimax(depth + 1, nodeIndex * 2 + 1, false, scores, h));
		}
		else
		{
			return Math.Min(Minimax(depth + 1, nodeIndex * 2, true, scores, h),
				Minimax(depth + 1, nodeIndex * 2 + 1, true, scores, h));
		}
	}

	static int log2(int n)
	{
		return (n == 1) ? 0 : 1 + log2(n / 2);
	}

} 



