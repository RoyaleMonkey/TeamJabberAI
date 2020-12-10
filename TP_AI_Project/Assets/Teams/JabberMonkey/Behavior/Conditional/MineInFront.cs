using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class MineInFront : Conditional
	{
		public float distanceToCheck = 2;

		BlackBordScipt blackBord = null;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}

		public override TaskStatus OnUpdate()
		{
			SpaceShip ship = GameManager.Instance.GetGameData().SpaceShips[blackBord.shipIndex];

			RaycastHit2D hit2D = Physics2D.Raycast(ship.Position, ship.Velocity, distanceToCheck, 1 << 13);
			if (hit2D)
			{
				blackBord.closestMine = hit2D.rigidbody.GetComponent<Mine>();
				return TaskStatus.Success;
			}

			return TaskStatus.Failure;
		}
	}
}