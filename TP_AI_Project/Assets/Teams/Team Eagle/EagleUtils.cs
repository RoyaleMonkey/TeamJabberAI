using DoNotModify;
using BehaviorDesigner.Runtime;

namespace Eagle
{
    [System.Serializable]
    public class SharedGameData : SharedVariable<GameData>
    {
        public static implicit operator SharedGameData(GameData value) { return new SharedGameData { Value = value }; }
    }

    [System.Serializable]
    public class SharedWayPoint : SharedVariable<WayPoint>
    {
        public static implicit operator SharedWayPoint(WayPoint value) { return new SharedWayPoint { Value = value }; }
    }
}