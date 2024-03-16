using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class InterruptSelector : Selector {
        protected override ProcessState OnUpdate() {
            int previous = current;
            base.OnStart();
            var status = base.OnUpdate();
            if (previous != current) {
                if (children[previous].state == ProcessState.Running) {
                    children[previous].Abort();
                }
            }

            return status;
        }
    }
}