using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace JabberMonkey
{
	public class FollowPlayer : Action
	{

		private Transform myTransform;
		private BlackBordScipt blackBordScipt;

		public override void OnStart()
		{
			blackBordScipt = GetComponent<BlackBordScipt>();
			myTransform = blackBordScipt.myShip.transform;
		}

		public override TaskStatus OnUpdate()
		{
			float currentDistance = (myTransform.position - blackBordScipt.enemyBackPosition).magnitude;

			Vector3 diference = blackBordScipt.enemyBackPosition - myTransform.position;
			float sign = (blackBordScipt.enemyBackPosition.y < myTransform.position.y) ? -1.0f : 1.0f;
			float targetOrientation = Vector2.Angle(Vector2.right, diference) * sign;

			blackBordScipt.trust = currentDistance / 2;
			blackBordScipt.targetAngle = targetOrientation;

			return TaskStatus.Running;
		}
	}
}