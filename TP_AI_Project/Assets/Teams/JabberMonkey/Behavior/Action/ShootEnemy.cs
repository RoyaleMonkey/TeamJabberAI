using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using System.Collections;

namespace JabberMonkey
{
	[TaskCategory("JabberAI")]
	public class ShootEnemy : Action
	{
		public float ShootTimeTolerance = 0.05f;
		public float cooldown = 0.2f;

		private bool canShoot = true;

		BlackBordScipt blackBord;

		bool _debugCanShootIntersect = false;
		Vector2 _debugIntersection = Vector2.zero;
		float _debugTimeDiff = 0;

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

			_debugCanShootIntersect = false;

			GameData gameData = GameManager.Instance.GetGameData();

			SpaceShip aiShip = gameData.SpaceShips[blackBord.shipIndex];
			SpaceShip enemyShip = gameData.SpaceShips[1 - blackBord.shipIndex];

			float shootAngle = Mathf.Deg2Rad * aiShip.Orientation;
			Vector2 shootDir = new Vector2(Mathf.Cos(shootAngle), Mathf.Sin(shootAngle));

			Vector2 intersection;
			bool canIntersect = MathUtils.ComputeIntersection(aiShip.Position, shootDir, enemyShip.Position, enemyShip.Velocity, out intersection);
			if (!canIntersect)
			{
				blackBord.trust = 0;
				blackBord.targetAngle = Vector2.SignedAngle(Vector2.right,enemyShip.Position - aiShip.Position);
				return TaskStatus.Running;
			}
			Vector2 aiToI = intersection - aiShip.Position;
			Vector2 enemyToI = intersection - enemyShip.Position;
			if (Vector2.Dot(aiToI, shootDir) <= 0)
			{
				blackBord.trust = 0;
				blackBord.targetAngle = Vector2.SignedAngle(Vector2.right, enemyShip.Position + enemyShip.Velocity - aiShip.Position);
				return TaskStatus.Running;
			}

			float bulletTimeToI = aiToI.magnitude / Bullet.Speed;
			float enemyTimeToI = enemyToI.magnitude / enemyShip.Velocity.magnitude;
			enemyTimeToI *= Vector2.Dot(enemyToI, enemyShip.Position) > 0 ? 1 : -1;

			_debugCanShootIntersect = canIntersect;
			_debugIntersection = intersection;

			float timeDiff = bulletTimeToI - enemyTimeToI;
			_debugTimeDiff = timeDiff;
			if (Mathf.Abs(timeDiff) < ShootTimeTolerance)
            {
				blackBord.shouldShoot = true;
				StartCoroutine(ShootRoutine());
				return TaskStatus.Success;
			}
			else
			{
				blackBord.trust = 0;
				blackBord.targetAngle = Vector2.SignedAngle(Vector2.right, enemyShip.Position+ enemyShip.Velocity - aiShip.Position);
				return TaskStatus.Running;
			}
		}
		public override void OnDrawGizmos()
		{
			if (_debugCanShootIntersect)
			{
				GameData gameData = GameManager.Instance.GetGameData();
				SpaceShip aiShip = gameData.SpaceShips[blackBord.shipIndex];
				SpaceShip enemyShip = gameData.SpaceShips[1 - blackBord.shipIndex];
				Gizmos.DrawLine(aiShip.Position, _debugIntersection);
				Gizmos.DrawLine(enemyShip.Position, _debugIntersection);
				Gizmos.DrawSphere(_debugIntersection, Mathf.Clamp(Mathf.Abs(_debugTimeDiff), 0.5f, 0));
			}
		}

		private IEnumerator ShootRoutine()
		{
			canShoot = false;
			yield return new WaitForSeconds(cooldown);
			canShoot = true;
		}
	}
}