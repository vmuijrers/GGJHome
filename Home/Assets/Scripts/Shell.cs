using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Shell : MonoBehaviour
{
	int inhabitants = 0;
	List<PlayerIndex> playerIndeces = new List<PlayerIndex>();

	public void CrabEnters (PlayerIndex playerIndex)
	{
		inhabitants++;
		playerIndeces.Add(playerIndex);
	}
}