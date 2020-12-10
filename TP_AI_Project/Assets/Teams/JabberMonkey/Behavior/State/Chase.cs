﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JabberMonkey
{
    public class Chase : StateMachineBehaviour
    {
        public BlackBordScipt blackBord;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!blackBord)
                blackBord = animator.GetComponent<BlackBordScipt>();

            blackBord.changeBehaviour(1);
        }
    }
}
