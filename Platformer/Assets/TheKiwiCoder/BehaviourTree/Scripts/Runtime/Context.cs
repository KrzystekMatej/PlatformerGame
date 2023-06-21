using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder {

    // The context is a shared object every node has access to.
    // Commonly used components and subsystems should be stored here
    // It will be somewhat specific to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context {
        // Add other game specific systems here
        public Agent agent;
        public AIInputController inputController;
        public AIVision vision;
        public AISteering steering;

        public static Context CreateFromGameObject(GameObject gameObject) {
            // Fetch all commonly used components
            Context context = new Context();
            // Add whatever else you need here...
            context.agent = gameObject.GetComponentInChildren<Agent>();
            context.inputController = gameObject.GetComponentInChildren<AIInputController>();
            context.vision = gameObject.GetComponentInChildren<AIVision>();
            context.steering = gameObject.GetComponentInChildren<AISteering>();

            return context;
        }
    }
}