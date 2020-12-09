using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class MineInFront : Conditional
	{
		public float distanceToCheck;
		public float maxMineOffset;

		BlackBordScipt blackBord = null;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}

		public override TaskStatus OnUpdate()
		{
			GameData data = GameManager.Instance.GetGameData();
			SpaceShip ship = data.SpaceShips[blackBord.shipIndex];
            foreach (Mine item in data.Mines)
            {
				float distance = (ship.Position - item.Position).magnitude;
                if (distance < distanceToCheck)
                {
					Vector2 mineDirection = ship.transform.InverseTransformPoint(item.Position);
					if (mineDirection.x > 0 && Mathf.Abs(mineDirection.y) < maxMineOffset)
						return TaskStatus.Success;
                }
			}

			return TaskStatus.Failure;
		}
	}
}