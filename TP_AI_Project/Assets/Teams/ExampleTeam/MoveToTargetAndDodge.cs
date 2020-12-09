using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace JabberMonkey
{
	public class MoveToTargetAndDodge : Action
	{
		public SharedString targetName;
		private Vector3 target;
		public SharedFloat arriveDistance = 0.2f;
		private Transform playerTransform;
		private float currentDistance;
		private float targetOrientation;
		private float thrust;
		private Vector2 speed;
		private BlackBordScipt blackboard;
		public override void OnStart()
		{
			blackboard = GetComponent<BlackBordScipt>();
			target = blackboard.targetPosition;
			//targetOrientation = GameManager.Instance.GetGameData().SpaceShips[1].transform.rotation.z;
		}

		public override TaskStatus OnUpdate()
		{
			playerTransform = GameManager.Instance.GetGameData().SpaceShips[1].transform;
			speed = GameManager.Instance.GetGameData().SpaceShips[1].Velocity;
			currentDistance = (playerTransform.position - target).magnitude;

			//Basic Move
			Vector3 diference = target - playerTransform.position;
			float sign = (target.y < playerTransform.position.y) ? -1.0f : 1.0f;
			targetOrientation = Vector2.Angle(Vector2.right, diference) * sign;
			thrust = currentDistance;

			float velocityAngle = Vector2.SignedAngle(diference, blackboard.myShip.Velocity);
			Debug.Log("velocity angle: "+ velocityAngle+" target angle: "+ targetOrientation);

			targetOrientation -= Mathf.Lerp(velocityAngle, 0, currentDistance/2 );

			//Dodge Asteroids
			RaycastHit2D hit;
			hit = Physics2D.Raycast(playerTransform.position, playerTransform.right, 1.5f,(1<<12));
			Debug.DrawLine(playerTransform.position, playerTransform.position + playerTransform.right, Color.green);

			if(hit)
            {
				float signhit = (hit.normal.y < 0) ? -1.0f : 1.0f;
				float normalAngle = Vector2.Angle(Vector2.right,hit.normal) * signhit;
				Debug.Log(hit.collider.transform.parent.gameObject.name);
				targetOrientation = normalAngle;
				thrust = 0.5f;
            }
            

			blackboard.trust = thrust;
			blackboard.targetAngle = targetOrientation;
			if (currentDistance < arriveDistance.Value)
				return TaskStatus.Success;

			return TaskStatus.Running;
		}

	}
}
