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
		private List<Vector3> waypointList = new List<Vector3>();
		private BlackBordScipt blackboard;
		public override void OnStart()
		{
			waypointList.Clear();
			blackboard = GetComponent<BlackBordScipt>();
			foreach(WayPoint p in GameManager.Instance.GetGameData().WayPoints)
            {
				if(p.Owner != blackboard.shipIndex)
					waypointList.Add(p.Position);
            }
			if(waypointList.Count > 0)
            {
				waypointList.Sort((x, y) => Vector3.Distance(x, blackboard.myShip.transform.position).CompareTo(Vector3.Distance(y, blackboard.myShip.transform.position)));

				if(Vector3.Distance(waypointList[0], blackboard.myShip.transform.position) < maxDistance)
					blackboard.targetPosition = waypointList[0];
			}

		}

		public override TaskStatus OnUpdate()
		{
				return TaskStatus.Success;
		}
	}
}