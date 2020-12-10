using BehaviorDesigner.Runtime;
using DoNotModify;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JabberMonkey
{
    public class BlackBordScipt : MonoBehaviour
    {
        [HideInInspector]
        public float enemyHitTimer = 0;
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

        private Animator _anim;
        private BehaviorTree[] behaviorTrees;
        private int currentTree = -1;
        private int lastEnemyHitCount = 0;

        // Start is called before the first frame update
        void Awake()
        {
            behaviorTrees = GetComponents<BehaviorTree>();
            GameData gameData = GameManager.Instance.GetGameData();
            _anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            GameData gameData = GameManager.Instance.GetGameData();

            enemyBackPosition = gameData.SpaceShips[1-shipIndex].transform.position + gameData.SpaceShips[0].transform.right * -1;

            UpdateScoreData();
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

        public void changeBehaviour(int index)
        {
            foreach (BehaviorTree item in behaviorTrees)
            {
                item.enabled = false;
            }
            behaviorTrees[index].enabled = true;
            currentTree = index;
        }

        public void UpdateScoreData()
        {
            _anim.SetInteger("PlayerAdvantage", myShip.Score - GameManager.Instance.GetGameData().SpaceShips[1 - shipIndex].Score);
        }

        private void CheckEnemyHit(GameData gameData)
        {
            if (gameData.SpaceShips[1 - shipIndex].HitCount == lastEnemyHitCount)
                enemyHitTimer += Time.deltaTime;
            else
            {
                lastEnemyHitCount = gameData.SpaceShips[1 - shipIndex].HitCount;
                enemyHitTimer = 0;
            }
        }
    }
}
