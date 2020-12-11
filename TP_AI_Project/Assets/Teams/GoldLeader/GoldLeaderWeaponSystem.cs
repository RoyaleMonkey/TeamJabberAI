using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace GoldLeader
{
    public class GoldLeaderWeaponSystem : MonoBehaviour
    {
        [Header("Shoot")]
        [SerializeField]
        float distanceForShoot = 6.0f;
        [SerializeField]
        float timeCooldownShoot = 0.2f;
        [SerializeField]
        float ShootTimeTolerance = 0.2f;

        [Header("Mine")]
        [SerializeField]
        float timeCooldownMine = 2f;

        GameData data;
        SpaceShip ship;
        SpaceShip enemyShip;

        float cooldownMine = 0;
        float cooldownShoot = 0;

        bool _debugCanShootIntersect = false;
        Vector2 _debugIntersection = Vector2.zero;
        float _debugTimeDiff = 0;


        private bool mine;
        public bool Mine
        {
            get { return mine; }
        }

        private bool shoot;
        public bool Shoot
        {
            get { return shoot; }
        }

        private bool enemyInSight;
        public bool EnemyInSight
        {
            get { return enemyInSight; }
        }


        public Vector2 ShootIntersection
        {
            get { return _debugIntersection; }
        }
        public bool CanShootIntersect
        {
            get { return _debugCanShootIntersect; }
        }


        public void InitializeSystem(SpaceShip playerShip, SpaceShip enemySpaceship, GameData newData)
        {
            ship = playerShip;
            enemyShip = enemySpaceship;
            data = newData;
        }

        public void UpdateSystem(GameData newData)
        {
            // Y'a besoin de faire ça ? On a pas une référence normalement ?
            data = newData;
            CheckEnemyInSight();

        }

        public void LateUpdateSystem(GameData newData)
        {
            if (cooldownShoot > 0) cooldownShoot -= Time.deltaTime;
            if (cooldownMine > 0) cooldownMine -= Time.deltaTime;

            if (shoot == true)
            {
                shoot = false;
                cooldownShoot = timeCooldownShoot;
            }
            if (mine == true)
            {
                mine = false;
                cooldownMine = timeCooldownMine;
            }
        }

        private void CheckEnemyInSight()
        {
            if (cooldownShoot > 0)
            {
                enemyInSight = false; 
                return;
            }
            if((ship.transform.position - enemyShip.transform.position).sqrMagnitude > distanceForShoot)
            {
                enemyInSight = false;
                return;
            }
            if (CanHit(data, ShootTimeTolerance) == true)
            {
                // Raycast entre le player et le point
                int layerMask = 1 << 15;
                layerMask |= 1 << 12;
                Vector2 shipPos = new Vector2(ship.transform.position.x, ship.transform.position.y);
                RaycastHit2D hit = Physics2D.Raycast(shipPos, _debugIntersection - shipPos, (_debugIntersection - shipPos).magnitude, layerMask);
                if(!hit)
                {
                    Debug.Log("Go, go go");
                    enemyInSight = true;
                    return;
                }
            }
            enemyInSight = false;
        }

        public bool CanHit(GameData gameData, float timeTolerance)
        {
            _debugCanShootIntersect = false;

            float shootAngle = Mathf.Deg2Rad * ship.Orientation;
            Vector2 shootDir = new Vector2(Mathf.Cos(shootAngle), Mathf.Sin(shootAngle));

            Vector2 intersection;
            bool canIntersect = MathUtils.ComputeIntersection(ship.Position, shootDir, enemyShip.Position, enemyShip.Velocity, out intersection);
            if (!canIntersect)
            {
                return false;
            }
            Vector2 aiToI = intersection - ship.Position;
            Vector2 enemyToI = intersection - enemyShip.Position;
            if (Vector2.Dot(aiToI, shootDir) <= 0)
                return false;

            float bulletTimeToI = aiToI.magnitude / Bullet.Speed;
            float enemyTimeToI = enemyToI.magnitude / enemyShip.Velocity.magnitude;
            enemyTimeToI *= Vector2.Dot(enemyToI, enemyShip.Velocity) > 0 ? 1 : -1;

            _debugCanShootIntersect = canIntersect;
            _debugIntersection = intersection;

            float timeDiff = bulletTimeToI - enemyTimeToI;
            _debugTimeDiff = timeDiff;
            return Mathf.Abs(timeDiff) < timeTolerance;
        }

        private void OnDrawGizmos()
        {
            if (_debugCanShootIntersect)
            {
                Gizmos.DrawLine(ship.Position, _debugIntersection);
                Gizmos.DrawLine(enemyShip.Position, _debugIntersection);
                Gizmos.DrawSphere(_debugIntersection, Mathf.Clamp(Mathf.Abs(_debugTimeDiff), 0.5f, 0));
            }
        }


        public void FireShoot(float energyToSave)
        {
            if (cooldownShoot > 0) return;
            if (ship.Energy >= (energyToSave))
                shoot = true;
        }

        public void LandMine(float energyToSave, float delay)
        {
            StartCoroutine(LandMineCoroutine(energyToSave, delay));
        }
        private IEnumerator LandMineCoroutine(float delay, float energyToSave)
        {
            yield return new WaitForSeconds(delay);
            if (cooldownMine <= 0)
            {
                if (ship.Energy >= (ship.MineEnergyCost + energyToSave))
                    mine = true;
            }
        }

    }
}
