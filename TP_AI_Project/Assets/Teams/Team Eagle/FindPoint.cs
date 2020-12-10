using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using DoNotModify;
using UnityEngine;

namespace Eagle
{
    [TaskCategory("AI_Eagle")]
    public class FindPoint : Action
    {
        BehaviorTree _behaviorTree;

        public override void OnStart()
        {
            _behaviorTree = GetComponent<BehaviorTree>();
        }

        public override TaskStatus OnUpdate()
        {
            GameData data = (_behaviorTree.GetVariable("GameData") as SharedGameData).Value;
            int _owner = (_behaviorTree.GetVariable("Owner") as SharedInt).Value;


            float tmpDist = 1000f;
            int indexA = 0;

            for (int i = 0; i < data.WayPoints.Count; i++)
            {
                if (tmpDist > Vector2.Distance(data.WayPoints[i].Position, data.SpaceShips[_owner].Position) && data.WayPoints[i].Owner != _owner)
                {
                    tmpDist = Vector2.Distance(data.WayPoints[i].Position, data.SpaceShips[_owner].Position);
                    indexA = i;
                }
            }

            _behaviorTree.SetVariableValue("FocusPointA", data.WayPoints[indexA]);
            int indexB = 0;

            for (int i = 0; i < data.WayPoints.Count; i++)
            {
                if (tmpDist > Vector2.Distance(data.WayPoints[i].Position, data.WayPoints[indexA].Position) && data.WayPoints[i].Owner != _owner)
                {
                    tmpDist = Vector2.Distance(data.WayPoints[i].Position, data.SpaceShips[_owner].Position);
                    indexB = i;
                }
            }

            _behaviorTree.SetVariableValue("FocusPointB", data.WayPoints[indexB]);
            //_behaviorTree.FindTask<IsWayPointTrigger>().wayPoint[0] = data.WayPoints[indexA];

            //Debug.Log(Vector2.Angle(data.SpaceShips[_owner].Position, focusPosition));

            //Debug.DrawLine(focusPosition, spaceShipPosition, Color.white, 2f);

            return TaskStatus.Success;
        }
    }
}