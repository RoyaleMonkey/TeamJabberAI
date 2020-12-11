using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace JabberMonkey
{
	public class TargetEnemy : Action
	{

		public float checkDistance;
		public float forwardMultiplier;

		private Transform myTransform;
		private BlackBordScipt blackBordScipt;

		public override void OnStart()
		{
			blackBordScipt = GetComponent<BlackBordScipt>();
		}

		public override TaskStatus OnUpdate()
		{
            foreach (WayPoint item in GameManager.Instance.GetGameData().WayPoints)
            {
				if(item.Owner != blackBordScipt.shipIndex)
                {
					Vector2 forward = blackBordScipt.myShip.transform.right;
					float Distance = ((blackBordScipt.myShip.Position + forward * forwardMultiplier) - item.Position).magnitude;
					if (Distance < checkDistance)
					{
						blackBordScipt.targetPosition = item.Position;
						return TaskStatus.Success;
					}

				}
            }
			blackBordScipt.targetPosition = blackBordScipt.enemyBackPosition;
			return TaskStatus.Success;
		}
	}
}