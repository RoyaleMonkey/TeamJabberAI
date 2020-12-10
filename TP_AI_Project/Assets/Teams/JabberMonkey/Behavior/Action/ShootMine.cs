using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace JabberMonkey 
{
	[TaskCategory("JabberAI")]
	public class ShootMine : Action
	{
		public float angleMaxOffset;
		
		BlackBordScipt blackBord = null;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}
		public override void OnStart()
		{
		}

		public override TaskStatus OnUpdate()
		{
			if (!blackBord.closestMine)
				return TaskStatus.Failure;

			Vector2 diference = blackBord.closestMine.Position - blackBord.myShip.Position;
			float sign = (blackBord.closestMine.Position.y < blackBord.myShip.Position.y) ? -1.0f : 1.0f;
			float orientation = Vector2.Angle(Vector2.right, diference) * sign;

			blackBord.trust = 0;
			blackBord.targetAngle = orientation;
			if (orientation < 0)
				orientation += 360;
			if(Mathf.Abs(blackBord.myShip.Orientation - orientation)<angleMaxOffset)
            {
				blackBord.shouldShoot = true;
				return TaskStatus.Success;
            }
			return TaskStatus.Running;
		}
	} 
}