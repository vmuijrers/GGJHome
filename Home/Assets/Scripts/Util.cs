using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
	public static T Choose<T> (params T[] input)
	{
		return input[Random.Range(0, input.Length)];
	}

	public static float SquareDistance (Vector3 lhs, Vector3 rhs)
	{
		return (rhs - lhs).sqrMagnitude;
	}
}