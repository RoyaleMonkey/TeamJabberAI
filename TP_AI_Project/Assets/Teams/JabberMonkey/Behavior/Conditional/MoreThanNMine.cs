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
		public bool isChasing;

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
				if (!isChasing && !item.IsActive)
					continue;

				float distance = (item.Position - blackBord.closestMine.Position).magnitude;
				if (distance < 1.7)
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