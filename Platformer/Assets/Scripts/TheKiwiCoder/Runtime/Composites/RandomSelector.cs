using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheKiwiCoder {
    [System.Serializable]
    public class RandomSelector : CompositeNode {
        protected int current;

        protected override void OnStart() {
            current = Random.Range(0, children.Count);
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            var child = children[current];
            return child.Update();
        }
    }
}