using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

namespace JabberMonkey
{
    public class FindNearestWaypoint : Action
    {
		public float maxDistance;
		public AnimationCurve enemyWaypointPriority;
		public bool useCurve = false;
		private List<WayPoint> waypointList = new List<WayPoint>();
		private BlackBordScipt blackboard;
		public override void OnStart()
		{
			blackboard = GetComponent<BlackBordScipt>();
		}

		public override TaskStatus OnUpdate()
		{
			waypointList.Clear();
			foreach (WayPoint p in GameManager.Instance.GetGameData().WayPoints)
			{
				if (p.Owner != blackboard.shipIndex)
				{
					waypointList.Add(p);
				}
			}
			if (waypointList.Count > 0)
			{
				waypointList.Sort((x, y) => Vector3.Distance(x.Position, blackboard.myShip.transform.position).CompareTo(Vector3.Distance(y.Position, blackboard.myShip.transform.position)));
				if (useCurve)
					waypointList.Sort((x, y) => SortWaypointsWithCurve(x, y));
				if (Vector3.Distance(waypointList[0].Position, blackboard.myShip.transform.position) < maxDistance)
                {
					blackboard.targetPosition = waypointList[0].Position;
					return TaskStatus.Success;
				}
			}
			return TaskStatus.Running;
		}

		public int SortWaypointsWithCurve(WayPoint a, WayPoint b)
        {
			float DistanceA = Vector3.Distance(a.Position, blackboard.myShip.transform.position);
			float DistanceB = Vector3.Distance(b.Position, blackboard.myShip.transform.position);
			if(a.Owner == 1 - blackboard.shipIndex)
            {
				DistanceA *= enemyWaypointPriority.Evaluate(DistanceA);
            }
			if (b.Owner == 1 - blackboard.shipIndex)
			{
				DistanceB *= enemyWaypointPriority.Evaluate(DistanceB);
			}

			return DistanceA.CompareTo(DistanceB);
        }
	}
}