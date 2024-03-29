using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class RandomFailure : ActionNode {

        [Range(0,1)]
        [Tooltip("Percentage chance of failure")] public float chanceOfFailure = 0.5f;

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            float value = Random.value;
            if (value > chanceOfFailure) {
                return ProcessState.Failure;
            }
            return ProcessState.Success;
        }
    }
}