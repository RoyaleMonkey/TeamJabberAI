using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;

namespace GoldLeader {

	public class GoldLeaderController : BaseSpaceShipController
	{
		SpaceShip enemySpaceship;

		//
		[SerializeField]
		public GoldLeaderMovementSystem movementSystem;

		[SerializeField]
		public GoldLeaderWeaponSystem weaponSystem;
		[SerializeField]
		public GoldLeaderDefenseSystem defenseSystem;

		BehaviorTree behaviorTree = null;

		public override void Initialize(SpaceShip spaceship, GameData data)
		{
			enemySpaceship = data.GetSpaceShipForOwner(0);
			if(enemySpaceship == spaceship)
				enemySpaceship = data.GetSpaceShipForOwner(1);

			//movementSystem = new GoldSquadronWeaponSystem(spaceship, data);

			if (movementSystem == null)
				movementSystem = GetComponent<GoldLeaderMovementSystem>();
			movementSystem.InitializeSystem(spaceship, enemySpaceship, data);

			if (weaponSystem == null)
				weaponSystem = GetComponent<GoldLeaderWeaponSystem>();
			weaponSystem.InitializeSystem(spaceship, enemySpaceship, data);

			if (defenseSystem == null)
				defenseSystem = GetComponent<GoldLeaderDefenseSystem>();
			defenseSystem.InitializeSystem(spaceship, enemySpaceship, data);

			behaviorTree = GetComponent<BehaviorTree>();
		}



		public override InputData UpdateInput(SpaceShip spaceship, GameData data)
		{
			movementSystem.UpdateSystem(data); 
			weaponSystem.UpdateSystem(data);
			defenseSystem.UpdateSystem(data);

			behaviorTree.SetVariableValue("EnemyInSight", weaponSystem.EnemyInSight);
			behaviorTree.SetVariableValue("MineInSight", defenseSystem.MineInSight);

			behaviorTree.SetVariableValue("IsStun", spaceship.IsStun());
			behaviorTree.SetVariableValue("IsHit", spaceship.IsHit());

			behaviorTree.SetVariableValue("EnemyIsStun", enemySpaceship.IsStun());
			behaviorTree.SetVariableValue("EnemyIsHit", enemySpaceship.IsHit());

			behaviorTree.SetVariableValue("DistanceToEnemy", (spaceship.transform.position - enemySpaceship.transform.position).sqrMagnitude);

			int score = 0;
			for (int i = 0; i < data.WayPoints.Count; i++)
			{
				if (data.WayPoints[i].Owner == spaceship.Owner)
					score += 1;
			}
			behaviorTree.SetVariableValue("Waypoints", score);

			InputData input = new InputData(movementSystem.Thrust, movementSystem.Orient, weaponSystem.Shoot, weaponSystem.Mine, defenseSystem.Shockwave);

			weaponSystem.LateUpdateSystem(data);
			defenseSystem.LateUpdateSystem(data);
			return input;
		}

	}


}
