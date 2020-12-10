using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace JabberMonkey
{
	public class TargetEnemy : Action
	{

		private Transform myTransform;
		private BlackBordScipt blackBordScipt;

		public override void OnStart()
		{
			blackBordScipt = GetComponent<BlackBordScipt>();
		}

		public override TaskStatus OnUpdate()
		{
			blackBordScipt.targetPosition = blackBordScipt.enemyBackPosition;
			return TaskStatus.Success;
		}
	}
}