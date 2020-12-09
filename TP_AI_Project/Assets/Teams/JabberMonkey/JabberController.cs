using DoNotModify;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JabberMonkey
{
    public class JabberController : BaseSpaceShipController
    {
		BlackBordScipt blackBord;
		public override void Initialize(SpaceShip spaceship, GameData data)
		{
			blackBord = GetComponent<BlackBordScipt>();
			blackBord.Initialize(spaceship);
		}

		public override InputData UpdateInput(SpaceShip spaceship, GameData data)
		{
			return blackBord.GetInputData();
		}
	}
}
