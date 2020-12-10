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
			target = blackboard.targetPosition;

			playerTransform = GameManager.Instance.GetGameData().SpaceShips[blackboard.shipIndex].transform;
			speed = GameManager.Instance.GetGameData().SpaceShips[blackboard.shipIndex].Velocity;
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
			hit = Physics2D.Raycast(playerTransform.position, blackboard.myShip.Velocity, 1.5f,(1<<12));
			Debug.DrawLine(playerTransform.position, blackboard.myShip.Position + blackboard.myShip.Velocity, Color.green);

			if(hit)
            {
				RaycastHit2D hitbetweenPlayerAndTarget = Physics2D.Raycast(playerTransform.position, blackboard.targetPosition - playerTransform.position, Vector3.Distance(blackboard.targetPosition, playerTransform.position), (1 << 12));

				if (hitbetweenPlayerAndTarget)
                {
					float signhit = (hit.normal.y < 0) ? -1.0f : 1.0f;
					float normalAngle = Vector2.Angle(Vector2.right, hit.normal) * signhit;
					Vector2 targetDirectionFromHit = new Vector2(blackboard.targetPosition.x, blackboard.targetPosition.y) - hit.point;
					float hitTargetDirAngle = Vector2.Angle(targetDirectionFromHit, Vector2.right);

					Debug.Log("Hit Target Angle  : " + hitTargetDirAngle);
					Debug.Log("AngleDifference : " + Mathf.DeltaAngle(hitTargetDirAngle, normalAngle + 90));
					targetOrientation = (Mathf.DeltaAngle(hitTargetDirAngle, normalAngle + 90) < 90) ? normalAngle + 90f : normalAngle - 90f;
				}

				thrust = 0.5f;
            }
            

			blackboard.trust = thrust;
			blackboard.targetAngle = targetOrientation;
			if (currentDistance < arriveDistance.Value)
				return TaskStatus.Success;

			return TaskStatus.Success;
		}

	}
}
