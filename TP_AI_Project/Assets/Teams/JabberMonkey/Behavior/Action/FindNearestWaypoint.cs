using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

namespace JabberMonkey
{
    public class FindNearestWaypoint : Action
    {
		private List<Vector3> waypointList;
		private BlackBordScipt blackboard;
		public override void OnStart()
		{
			blackboard = GetComponent<BlackBordScipt>();
			foreach(WayPoint p in GameManager.Instance.GetGameData().WayPoints)
            {
				if(p.Owner != blackboard.shipIndex)
					waypointList.Add(p.Position);
            }
			if(waypointList.Count > 1)
				waypointList.Sort((x, y) => Vector3.Distance(x, blackboard.myShip.transform.position).CompareTo(Vector3.Distance(y, blackboard.myShip.transform.position)));

		}

		public override TaskStatus OnUpdate()
		{
				return TaskStatus.Success;
		}
	}
}