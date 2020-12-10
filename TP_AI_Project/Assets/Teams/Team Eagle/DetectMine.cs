using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using System.Collections.Generic;

namespace Eagle
{
	public struct Triangle
	{
		public Vector2 p0;
		public Vector2 p1;
		public Vector2 p2;
	}

	public class DetectMine : Action
	{
		float range;
		float angle;

		private List<Mine> mines = new List<Mine>();

		BehaviorTree _behaviorTree;
		public override void OnStart()
		{
			_behaviorTree = GetComponent<BehaviorTree>();
			range = (_behaviorTree.GetVariable("MineFocusRange") as SharedFloat).Value;
			angle = (_behaviorTree.GetVariable("MineFocusAngle") as SharedFloat).Value;
		}

		private bool PointInTriangle(Vector2 p, Triangle tri)
		{
			var dX = p.x - tri.p2.x;
			var dY = p.y - tri.p2.y;
			var dX21 = tri.p2.x - tri.p1.x;
			var dY12 = tri.p1.y - tri.p2.y;
			var D = dY12 * (tri.p0.x - tri.p2.x) + dX21 * (tri.p0.y - tri.p2.y);
			var s = dY12 * dX + dX21 * dY;
			var t = (tri.p2.y - tri.p0.y) * dX + (tri.p0.x - tri.p2.x) * dY;
			if (D < 0) return s <= 0 && t <= 0 && s + t >= D;
			return s >= 0 && t >= 0 && s + t <= D;
		}

		public override TaskStatus OnUpdate()
		{
			GameData data = (_behaviorTree.GetVariable("GameData") as SharedGameData).Value;
			int _owner = (_behaviorTree.GetVariable("Owner") as SharedInt).Value;

			Triangle triangle = new Triangle();
			triangle.p0 = data.SpaceShips[_owner].Position;

			Vector2 orientR = new Vector2(Mathf.Cos((data.SpaceShips[_owner].Orientation + angle / 2) * Mathf.Deg2Rad), Mathf.Sin((data.SpaceShips[_owner].Orientation + angle / 2) * Mathf.Deg2Rad));
			Vector2 orientL = new Vector2(Mathf.Cos((data.SpaceShips[_owner].Orientation - angle / 2) * Mathf.Deg2Rad), Mathf.Sin((data.SpaceShips[_owner].Orientation - angle / 2) * Mathf.Deg2Rad));

			triangle.p1 = triangle.p0 + orientR.normalized* range;
			triangle.p2 = triangle.p0 + orientL.normalized* range;
			//Debug.DrawLine(triangle.p0, triangle.p1, Color.green);
			//Debug.DrawLine(triangle.p0, triangle.p2, Color.red);

            for (int i = mines.Count; i-->0;)
            {
                if (!mines[i])
                {
					mines.Remove(mines[i]);
                }
            }

			bool isMineShooted = false;

            for (int i = 0; i < data.Mines.Count; i++)
            {
				if(PointInTriangle(data.Mines[i].Position, triangle))
				{
					isMineShooted = false;

					for (int j = 0; j < mines.Count; j++)
                    {
                        if (mines[j] == data.Mines[i])
						{
							isMineShooted = true;
							break;
                        }
					}
                    if (!isMineShooted)
					{
						mines.Add(data.Mines[i]);
						//Debug.Log("shoot bomber");
						_behaviorTree.SetVariableValue("TriggerShoot", true);

						return TaskStatus.Success;
					}
				}
            }
	
		return TaskStatus.Success;
		}
	}
}