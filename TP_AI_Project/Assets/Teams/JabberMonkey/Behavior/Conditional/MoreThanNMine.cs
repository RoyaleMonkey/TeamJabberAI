using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class MoreThanNMine : Conditional
	{
		public int nombreMine;

		private BlackBordScipt blackBord;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}
		public override TaskStatus OnUpdate()
		{
			int nbMine = 0;
            foreach (Mine item in GameManager.Instance.GetGameData().Mines)
            {
				float distance = (item.Position - blackBord.closestMine.Position).magnitude;
				if (distance < 2)
				{
					nbMine++;
					if(nbMine>nombreMine)
						return TaskStatus.Success;
				}
            }
			return TaskStatus.Failure;
		}
	}
}