using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Monkey.Tasks
{
	public class MoveToTarget : Action
	{

		public SharedString targetName;
		private GameObject target;
		public SharedFloat arriveDistance = 0.2f;
		private Transform playerTransform;
		private float currentDistance;
		private float targetOrientation;
		public override void OnStart()
		{
			target = GameObject.Find(targetName.Value);
			//targetOrientation = GameManager.Instance.GetGameData().SpaceShips[1].transform.rotation.z;
		}

		public override TaskStatus OnUpdate()
		{
			playerTransform = GameManager.Instance.GetGameData().SpaceShips[1].transform;
			currentDistance = (playerTransform.position - target.transform.position).magnitude;

			Vector3 diference = target.transform.position - playerTransform.position;
			float sign = (target.transform.position.y < playerTransform.position.y) ? -1.0f : 1.0f;
			targetOrientation =  Vector2.Angle(Vector2.right, diference) * sign;

			//targetOrientation = Vector3.Angle(playerTransform.right,target.transform.position - playerTransform.position);

			Debug.Log("Angle : " + targetOrientation);
			Debug.Log("Forward : " + playerTransform.right);
			GetComponent<ExampleTeam.ExampleController>().activeInputData = new InputData(currentDistance, targetOrientation, false, false, false);
			if(currentDistance < arriveDistance.Value)
				return TaskStatus.Success;

			return TaskStatus.Running;
		}
	}
}
