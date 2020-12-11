using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class DropMine : Action
	{

		BlackBordScipt blackBord = null;
        public override void OnAwake()
        {
			blackBord = GetComponent<BlackBordScipt>();
		}

        public override void OnStart()
		{
				blackBord.shouldMine = true;
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}
}