using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class ShootForward : Action
	{

		BlackBordScipt blackBord = null;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}

		public override void OnStart()
		{
			blackBord.shouldShoot = true;
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}
}