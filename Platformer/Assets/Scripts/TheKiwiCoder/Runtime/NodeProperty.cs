using UnityEngine;

namespace TheKiwiCoder {

    [System.Serializable]
    public class NodeProperty {
        [SerializeReference]
        protected BlackboardKey reference; 
    }

    [System.Serializable]
    public class NodeProperty<T> : NodeProperty {

        public T defaultValue = default;

        public T Value {
            set {
                if (reference != null) {
                    reference.SetValue(value);
                } else {
                    defaultValue = value;
                }
            }
            get {
                if (reference != null) {
                    return (T)reference.GetValue();
                } else {
                    return defaultValue;
                }
            }
        }

        public bool IsBlackboardKey()
        {
            return reference != null;
        }
    }
}