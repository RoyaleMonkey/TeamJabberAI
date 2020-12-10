using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Eagle
{
	public class RotateToWayPoint : Action
	{
		//public SharedString nextStepe;

		BehaviorTree _behaviorTree;
		public override void OnStart()
		{
			_behaviorTree = GetComponent<BehaviorTree>();
		}

		public override TaskStatus OnUpdate()
		{
			int _owner = (_behaviorTree.GetVariable("Owner") as SharedInt).Value;
			GameData data = (_behaviorTree.GetVariable("GameData") as SharedGameData).Value;


			Vector2 focusWaipoint = (_behaviorTree.GetVariable("FocusPointA") as SharedWayPoint).Value.Position;

            if (Vector2.Distance(data.SpaceShips[_owner].Position, focusWaipoint)< 1)
            {
				focusWaipoint = (_behaviorTree.GetVariable("FocusPointB") as SharedWayPoint).Value.Position;
			}


			Vector2 direction = Vector2.Perpendicular(focusWaipoint - (data.SpaceShips[_owner].Position));
			//Debug.DrawLine(focusWaipoint, focusWaipoint - direction * data.SpaceShips[_owner].Velocity, Color.blue);

			focusWaipoint -= data.SpaceShips[_owner].Velocity.normalized;
			//float angle = Mathf.Atan2(focusWaipoint.y, focusWaipoint.x) * Mathf.Rad2Deg;
			//Vector2 focusPos = direction.normalized * Mathf.Cos((-data.SpaceShips[_owner].Velocity).magnitude * angle);


			Vector2 tmp = focusWaipoint - data.SpaceShips[_owner].Position;
			_behaviorTree.SetVariableValue("targetOrient", Mathf.Atan2(tmp.y, tmp.x) * Mathf.Rad2Deg);


			if (((data.SpaceShips[_owner].Orientation - Mathf.Atan2(tmp.y, tmp.x) * Mathf.Rad2Deg) >90))
			{
				_behaviorTree.SetVariableValue("Speed", .5f);
            }
            else
			{
				_behaviorTree.SetVariableValue("Speed", 1f);
			}

			return TaskStatus.Success;
		}
	}
}