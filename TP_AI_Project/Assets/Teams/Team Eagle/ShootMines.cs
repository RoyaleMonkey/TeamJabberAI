using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Eagle
{
	[TaskCategory("AI_Eagle")]
	public class ShootMines : Action
	{
		public override void OnStart()
		{

		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}
}