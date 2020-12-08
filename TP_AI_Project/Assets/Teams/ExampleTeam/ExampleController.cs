using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace ExampleTeam {

	public class ExampleController : BaseSpaceShipController
	{

		public InputData activeInputData;

		public override void Initialize(SpaceShip spaceship, GameData data)
		{
		}

		public override InputData UpdateInput(SpaceShip spaceship, GameData data)
		{
			return activeInputData;
		}

	}


}
