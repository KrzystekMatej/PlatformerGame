using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    [System.Serializable]
    public class Wait : ActionNode {

        [Tooltip("Amount of time to wait before returning success")] public float duration = 1;
        private float startTime;

        protected override void OnStart() {
            startTime = Time.time;
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            
            float timeRemaining = Time.time - startTime;
            if (timeRemaining > duration) {
                return ProcessState.Success;
            }
            return ProcessState.Running;
        }
    }
}
