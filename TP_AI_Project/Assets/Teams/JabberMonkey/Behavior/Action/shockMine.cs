using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class ShockMine : Action
	{

		private BlackBordScipt blackBord;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}

		public override void OnStart()
		{

		}

		public override TaskStatus OnUpdate()
		{
			if((blackBord.closestMine.Position - blackBord.myShip.Position).magnitude <0.75)
            {
				blackBord.shouldShock = true;
				return TaskStatus.Success;
            }				

			return TaskStatus.Running;
		}
	}
}