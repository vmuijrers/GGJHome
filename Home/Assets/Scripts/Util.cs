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

    public static T GetRandomEnumerator<T>() {
        System.Array values = System.Enum.GetValues(typeof(T));

        System.Random random = new System.Random();
        T randomBar = (T)values.GetValue(Random.Range(0, values.Length));
        return randomBar;
    }
}