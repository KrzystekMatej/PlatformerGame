using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    [System.Serializable]
    public abstract class Node {
        [HideInInspector] public ProcessState state = ProcessState.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid = System.Guid.NewGuid().ToString();
        [HideInInspector] public Vector2 position;
        [HideInInspector] public Context context;
        [HideInInspector] public Blackboard blackboard;
        [TextArea] public string description;
        [Tooltip("When enabled, the nodes OnDrawGizmos will be invoked")] public bool drawGizmos = true;
#if UNITY_EDITOR
        public bool logStart = false;
        public bool logState = false;
        public bool logStop = false;
#endif

        public virtual void OnInit() {
            // Nothing to do here
        }

        public ProcessState Update() {

            if (!started) {
                OnStart();
                started = true;
#if UNITY_EDITOR
                if (logStart) Log(LogType.Log, "Start");
#endif
            }

            state = OnUpdate();
#if UNITY_EDITOR
            if (logState) Log(LogType.Log, state);
#endif

            if (state != ProcessState.Running) {
                OnStop();
                started = false;
#if UNITY_EDITOR
                if (logStop) Log(LogType.Log, "Stop");
#endif
            }

            return state;
        }

        public void Abort() {
            BehaviourTree.Traverse(this, (node) => {
                node.started = false;
                node.state = ProcessState.Running;
                node.OnStop();
#if UNITY_EDITOR
                if (node.logStop) Log(LogType.Log, "Stop");
#endif
            });
        }

        public virtual void OnDrawGizmos(AgentManager agent) { }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract ProcessState OnUpdate();

        protected virtual void Log(LogType type, object message) {
            message = $"[{GetType()}]{message}";
            switch (type)
            {
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
            }
        }
    }

    public enum ProcessState
    {
        Running,
        Failure,
        Success
    }
}