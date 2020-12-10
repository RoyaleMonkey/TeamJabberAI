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

			Vector3 start = ship.Position + ship.Velocity.normalized * 0.6f;

			RaycastHit2D hit2D = Physics2D.Raycast(start+ship.transform.up * 0.2f, ship.Velocity, distanceToCheck, 1 << 13);
			Debug.DrawLine(start + ship.transform.up * 0.2f, ship.Position + ship.Velocity);
            if (!hit2D)
            {
				hit2D = Physics2D.Raycast(start + ship.transform.up * -0.2f, ship.Velocity, distanceToCheck, 1 << 13);
				Debug.DrawLine(start + ship.transform.up * -0.2f, ship.Position + ship.Velocity);
			}
			if (hit2D)
			{
				Mine mine = hit2D.rigidbody.GetComponent<Mine>();
				blackBord.closestMine = mine;
				return TaskStatus.Success;
            }

			return TaskStatus.Failure;
		}
	}
}