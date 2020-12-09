using BehaviorDesigner.Runtime;
using DoNotModify;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JabberMonkey
{
    public class BlackBordScipt : MonoBehaviour
    {

        public bool shouldShootClosestMine;
        public float shootOffest;

        [HideInInspector]
        public Vector3 enemyBackPosition;
        [HideInInspector]
        public Mine closestMine;
        [HideInInspector]
        public float trust;
        [HideInInspector]
        public float targetAngle;
        [HideInInspector]
        public bool shouldShoot;
        [HideInInspector]
        public bool shouldMine;
        [HideInInspector]
        public bool shouldShock;
        [HideInInspector]
        public int shipIndex;
        [HideInInspector]
        public SpaceShip myShip;

        public Vector3 targetPosition;

        private BehaviorTree behaviorTree;

        // Start is called before the first frame update
        void Awake()
        {
            behaviorTree = GetComponent<BehaviorTree>();
            GameData gameData = GameManager.Instance.GetGameData();
        }

        // Update is called once per frame
        void Update()
        {
            GameData gameData = GameManager.Instance.GetGameData();

            FindClosestMine(gameData);

            enemyBackPosition = gameData.SpaceShips[1-shipIndex].transform.position + gameData.SpaceShips[0].transform.right * -1;
        }

        void FindClosestMine(GameData gameData)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Mine item in gameData.Mines)
            {
                float distance = (myShip.Position - item.Position).magnitude;
                if (distance > closestDistance)
                {
                    closestDistance = distance;
                    closestMine = item;
                }
            }
        }

        public void Initialize(SpaceShip spaceShip)
        {
            shipIndex = spaceShip.Owner;
            myShip = spaceShip;
        }

        public InputData GetInputData()
        {
            InputData input = new InputData(trust, targetAngle, shouldShoot, shouldMine, shouldShock);
            shouldShoot = false;
            shouldMine = false;
            shouldShock = false;
            return input;
        }
    }
}
