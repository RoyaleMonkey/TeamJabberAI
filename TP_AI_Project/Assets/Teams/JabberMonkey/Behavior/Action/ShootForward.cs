using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class ShootForward : Action
	{
		public float cooldown = 0.05f;

		private bool canShoot = true;
		BlackBordScipt blackBord = null;

		public override void OnAwake()
		{
			blackBord = GetComponent<BlackBordScipt>();
		}

		public override void OnStart()
		{
		}

		public override TaskStatus OnUpdate()
		{
			if (!canShoot)
				return TaskStatus.Failure;

			blackBord.shouldShoot = true;
			StartCoroutine(ShootRoutine());
			return TaskStatus.Success;
		}

		private IEnumerator ShootRoutine()
		{
			canShoot = false;
			yield return new WaitForSeconds(cooldown);
			canShoot = true;
		}
	}
}