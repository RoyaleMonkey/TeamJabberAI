using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class TimerValue : Conditional
	{
		public SharedFloat checkValue;
		public SharedBool isSuperior = false;
		BlackBordScipt blackBord = null;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}

		public override TaskStatus OnUpdate()
		{
			if(isSuperior.Value)
            {
				if (checkValue.Value > GameManager.Instance.GetGameData().timeLeft)
					return TaskStatus.Success;
				else
					return TaskStatus.Failure;
            }
			else
            {
				if (checkValue.Value < GameManager.Instance.GetGameData().timeLeft)
					return TaskStatus.Success;
				else
					return TaskStatus.Failure;
            }
		}
	}
}