using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class EnergyInfValue : Conditional
	{
		public int energyValue;

		private BlackBordScipt blackBord;

	public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}

		public override TaskStatus OnUpdate()
		{
			if (blackBord.myShip.Energy < energyValue)
				return TaskStatus.Success;
			else
				return TaskStatus.Failure;
		}
	} }