using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace GoldLeader
{

    // Je recrée une classe à chaque fois, et c'est pas opti
    public class WaypointData
    {
        public WayPoint WayPoint;
        public Vector3 Position;
        public float DistanceAcceptance;

        public WaypointData(WayPoint point, Vector3 pos, float acceptance)
        {
            WayPoint = point;
            Position = pos;
            DistanceAcceptance = acceptance;
        }

        // Check si le waypoint est le même, si oui, on check position pour savoir si le vaisseau se déplace sur le point, ou si on contourne un asteroid
        public bool IsWayPointValid(WayPoint wayPointToCheck)
        {
            if(wayPointToCheck == WayPoint)
            {
                return WayPoint.transform.position != Position;
            }
            return true;
            
        }
    }






    public class GoldLeaderMovementSystem : MonoBehaviour
    {
        [Header("Heuristique")]
        [SerializeField]
        float angleConsideredBehindShip = 0.4f;


        [Header("Movement")]
        [SerializeField]
        float goodTrajectoryThreshold = 3.0f; // 5
        [SerializeField]
        float goodAngleForPreshot = 10.0f;

        [SerializeField]
        float distanceAcceptance = 0.5f;
        [SerializeField]
        float distanceAcceptanceWhenNotWayPoint = 2.0f;

        [SerializeField]
        float distancePreshot = 5.0f;

        [Header("Fix")]
        [SerializeField] // Si le vaisseau reste en orbite du point autant de temps, c'est qu'il galère
        float TimeMaxInOrbit = 2f;

        SpaceShip ship;
        SpaceShip enemyShip;
        GameData data; // histoire d'être sur d'avoir tout, et pour appeler les task rapidement


        List<WayPoint> waypoints;
        public WaypointData currentWayPoint;
        WaypointData nextWayPoint;

        float timeInOrbit;

        List<WayPoint> pointsBehind = new List<WayPoint>();
        List<WayPoint> pointsFront = new List<WayPoint>();

        int bestIndexBehind = -1;
        int bestIndexFront = -1;

        float bestDistanceBehind = 10000;
        float bestDistanceFront = 10000;





        private float thrust;
        public float Thrust
        {
            get { return thrust; }
        }

        private float orient;
        public float Orient
        {
            get { return orient; }
        }





        public void InitializeSystem(SpaceShip spaceship, SpaceShip enemySpaceship, GameData newData)
        {
            ship = spaceship;
            enemyShip = enemySpaceship;
            data = newData;


            waypoints = data.WayPoints;

            currentWayPoint = new WaypointData(null, ship.transform.position, 0);
            //currentWayPoint = SelectBestWaypoint(spaceship.transform.position);
        }

        public void UpdateSystem(GameData newData)
        {
            // Y'a besoin de faire ça ? On a pas une référence normalement ?
            data = newData;
        }


        // ==========================================================================================================
        #region AlgoPathfindingViteuf

        public WaypointData SelectBestWaypoint(Vector3 start)
        {

            SortWaypoint(start);

            // On sélectionne si on va tout droit ou derrière
            List<WayPoint> finalWaypoints = null;
            int bestIndex = 0;
            /*int score = 0;
            if (bestDistanceBehind < bestDistanceFront - 1)
            {
                score = bonusWeightForPointBehindShip;
            }
            if (pointsBehind.Count + score > pointsFront.Count)*/
            if(pointsFront.Count == 0 || ((bestDistanceBehind + 1) < (bestDistanceFront) && pointsBehind.Count > pointsFront.Count))
            {
                finalWaypoints = pointsBehind;
                bestIndex = bestIndexBehind;
            }
            else
            {
                finalWaypoints = pointsFront;
                bestIndex = bestIndexFront;
            }

            // On preshot au cas où je sais pas
            if(finalWaypoints == null || finalWaypoints.Count == 0)
            {
                int rand = Random.Range(0, waypoints.Count);
                return new WaypointData(waypoints[rand], waypoints[rand].transform.position, distanceAcceptance);
            }

            // On calcul un chemin, en essayant d'éviter les murs
            Vector3 res = Vector3.zero;
            for (int i = 0; i < finalWaypoints.Count; i++)
            {
                if (bestIndex >= finalWaypoints.Count)
                    bestIndex -= finalWaypoints.Count;
                res = CalculatePath(start, finalWaypoints[bestIndex].transform.position);
                if (res != Vector3.negativeInfinity)
                {
                    break;
                }
                bestIndex += 1;
            }


            if (res == Vector3.negativeInfinity)
            {
                int rand = Random.Range(0, waypoints.Count);
                return new WaypointData(waypoints[rand], waypoints[rand].transform.position, distanceAcceptance);
            }

            float finalDistance = distanceAcceptance;
            if (res != finalWaypoints[bestIndex].transform.position)
                finalDistance = distanceAcceptanceWhenNotWayPoint;
            return new WaypointData(finalWaypoints[bestIndex], res, finalDistance); //finalWaypoints[bestIndex];
        }



        // Trie les waypoints dans 2 listes, une devant le vaisseau, une autre derrière
        private void SortWaypoint(Vector3 start)
        {
            pointsBehind.Clear();
            pointsFront.Clear();
            bestIndexBehind = -1;
            bestIndexFront = -1;
            bestDistanceBehind = 10000;
            bestDistanceFront = 10000;

            for (int i = 0; i < waypoints.Count; i++)
            {
                if (waypoints[i].Owner != ship.Owner && currentWayPoint.IsWayPointValid(waypoints[i]))
                {
                    Vector3 direction = waypoints[i].transform.position - start;
                    if (Vector3.Dot(ship.transform.right, direction) < angleConsideredBehindShip)
                    {
                        // the waypoint is behind the ship, donc c'est chiant
                        pointsBehind.Add(waypoints[i]);

                        //======================================================================
                        // Calcul de distance (plus opti après la décision du devant derrière ?)
                        float distance = direction.sqrMagnitude;
                        if (distance < bestDistanceBehind)
                        {
                            bestDistanceBehind = distance;
                            bestIndexBehind = pointsBehind.Count - 1;
                        }
                        //======================================================================
                    }
                    else
                    {
                        // the waypoint is devant, donc c'est good
                        pointsFront.Add(waypoints[i]);

                        //======================================================================
                        // Calcul de distance (plus opti après la décision du devant derrière ?)
                        float distance = direction.sqrMagnitude;
                        if (distance < bestDistanceFront)
                        {
                            bestDistanceFront = distance;
                            bestIndexFront = pointsFront.Count - 1;
                        }
                        //======================================================================
                    }
                }
            }
        }


        // Check surtout si y'a un asteroid ou un mur
        public Vector3 CalculatePath(Vector3 start, Vector3 target)
        {
            int layer = 1 << 12;
            layer |= 1 << 15;

            RaycastHit2D hit = Physics2D.Raycast(start, target - start, (target - start).magnitude, layer);
            Debug.DrawRay(start, target - start, Color.green, 1);
            if (hit) 
            {
                Vector3 d1;
                Vector3 d2;
                Vector3 dFinal;
                // Y'a un asteroid
                Asteroid asteroid = hit.transform.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    // On prend la taille et la position de l'asteroid et on prend un point perpendiculaire à Asteoir -> vaisseau
                    d1 = asteroid.transform.position - start;
                    d2 = Vector2.Perpendicular(d1.normalized) * (asteroid.Radius + (ship.Radius * 3));
                    dFinal = d1 + d2;
                }
                else
                {
                    d1 = hit.transform.position;
                    d2 = Vector3.zero;
                    dFinal = target;
                }

                float angleLeft = 0;
                float angleRight = 0;

                // On check à gauche
                hit = Physics2D.Raycast(start, dFinal, dFinal.magnitude, layer);
                Debug.DrawRay(start, dFinal, Color.blue, 1);
                angleLeft = Vector3.Dot(ship.transform.right, dFinal);
                if (hit)
                {
                    angleLeft = -1.0f;
                }

                // On check à droite
                dFinal = d1 - d2;
                hit = Physics2D.Raycast(start, dFinal, dFinal.magnitude, layer);
                Debug.DrawRay(start, dFinal, Color.blue, 1);
                angleRight = Vector3.Dot(ship.transform.right, dFinal);
                if (hit)
                {
                    angleRight = -1.0f;
                }

                // On compare celui qui a l'angle le plus favorable
                if(angleLeft > angleRight)
                {
                    dFinal = d1 + d2;
                }
                else if (angleLeft < angleRight)
                {
                    dFinal = d1 - d2;
                }
                else
                {
                    return Vector3.negativeInfinity;
                }
                return start + dFinal;
            }
            return target;
        }

        #endregion





        public void FindWaypoint()
        {
            timeInOrbit = 0;
            if (nextWayPoint == null)
            {
                nextWayPoint = SelectBestWaypoint(currentWayPoint.Position);
            }
            currentWayPoint = nextWayPoint;
            nextWayPoint = null;
            orient = RotateToTarget(ship.transform.position, currentWayPoint.Position);
            thrust = 1f;
        }

        public void FindWaypointEnemy(bool canIntersect, Vector2 intersection)
        {
            SortWaypoint(ship.transform.position);
            Vector3 direction = enemyShip.transform.position - ship.transform.position;
            if (Vector3.Dot(ship.transform.right, direction) < angleConsideredBehindShip)
            {
                // L'ennemi est derrière le joueur
                // Si il y a moins de 2 points devant le joueur, on prend la liste de derrière
            }
            else
            {
                // L'ennemi est devant lets go
            }

            // si la liste final est null ou 0, on va a la position de l'ennemi
        }



        public bool MoveToPoint()
        {
            float distance = (currentWayPoint.Position - ship.transform.position).sqrMagnitude;

            if (distance < distancePreshot)
            {
                timeInOrbit += Time.deltaTime;
                if (timeInOrbit >= TimeMaxInOrbit) // On a un détecte que le vaisseau stagne, on recalcule une trajectoie
                {
                    distance = 0;
                }
            }

            if (distance < currentWayPoint.DistanceAcceptance) // On a atteint le point
            {
                return true;
            }

            if ((ship.Velocity.magnitude >= (ship.SpeedMax * 0.6f)) && (Vector2.Angle(ship.Velocity, currentWayPoint.Position - ship.transform.position) < goodAngleForPreshot) && (distance < distancePreshot)) // On prend de l'avance
            {
                if (nextWayPoint == null)
                {
                    nextWayPoint = SelectBestWaypoint(currentWayPoint.Position);
                }
                orient = RotateToTarget(currentWayPoint.Position, nextWayPoint.Position - new Vector3(ship.Velocity.x, ship.Velocity.y, 0));
                thrust = 0;
            }
            else 
            {
                orient = RotateToTarget(ship.transform.position, currentWayPoint.Position);
                thrust = 1f;
            }
            return false;
        }



        public bool MoveToEnemy(bool canIntersect, Vector2 intersection)
        {
            float distance = (currentWayPoint.Position - ship.transform.position).sqrMagnitude;

            if (distance < distancePreshot)
            {
                timeInOrbit += Time.deltaTime;
                if (timeInOrbit >= TimeMaxInOrbit) // On a un détecte que le vaisseau stagne, on recalcule une trajectoie
                {
                    distance = 0;
                }
            }

            if (distance < currentWayPoint.DistanceAcceptance) // On a atteint le point
            {
                return true;
            }

            if ((ship.Velocity.magnitude >= (ship.SpeedMax * 0.5f)) && (Vector2.Angle(ship.Velocity, currentWayPoint.Position - ship.transform.position) < goodAngleForPreshot))
            {
                if (canIntersect == true)
                {
                    orient = RotateToTarget(currentWayPoint.Position, intersection);
                }
                else
                {
                    orient = RotateToTarget(currentWayPoint.Position, enemyShip.transform.position);
                }
                thrust = 0;
            }
            else
            {
                orient = RotateToTarget(ship.transform.position, currentWayPoint.Position);
                thrust = 1f;
            }
            return false;
        }



        public void StopMove()
        {
            thrust = 0;
        }





        public float RotateToTarget(Vector3 start, Vector3 target)
        {
            float angle = Vector2.SignedAngle(ship.transform.right, target - start);
            if(Mathf.Abs(angle) >= goodTrajectoryThreshold)
            {
                return ship.Orientation + (135 * Mathf.Sign(angle));
            }
            return ship.Orientation;
        }
    }
}
