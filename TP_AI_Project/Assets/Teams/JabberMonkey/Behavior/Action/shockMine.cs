using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class ShockMine : Action
	{

		private BlackBordScipt blackBord;
		private float lastMagnitude;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}

		public override void OnStart()
		{
			lastMagnitude = 0;
		}

		public override TaskStatus OnUpdate()
		{
			if(!blackBord.closestMine)
				return TaskStatus.Failure;
			float newMagnitude = (blackBord.closestMine.Position - blackBord.myShip.Position).magnitude;
			if ( newMagnitude < 1 || lastMagnitude >= newMagnitude)
            {
				blackBord.shouldShock = true;
				return TaskStatus.Success;
            }
			lastMagnitude = newMagnitude;
			return TaskStatus.Running;
		}
	}
}