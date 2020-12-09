using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace ExampleTeam {

	public class ExampleController : BaseSpaceShipController
	{
		private Blackboard _blackboard;
		public InputData activeInputData;


		public override void Initialize(SpaceShip spaceship, GameData data)
		{
			_blackboard = GetComponent<Blackboard>();
			_blackboard.Initialize(spaceship, data);
		}

		public override InputData UpdateInput(SpaceShip spaceship, GameData data)
		{
			return activeInputData;
		}

	}


}
