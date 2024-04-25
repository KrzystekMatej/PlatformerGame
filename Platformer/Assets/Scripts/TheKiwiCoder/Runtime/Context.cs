using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder
{

    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context
    {
        // Add other game specific systems here
        public AIInputController InputController { get; private set; }
        public AgentManager Agent { get; private set; }
        public SteeringController Steering { get; private set; }

        public static Context CreateFromGameObject(GameObject gameObject)
        {
            Context context = new Context();
            context.InputController = gameObject.GetComponentInParent<AIInputController>();
            context.Agent = context.InputController.GetComponentInChildren<AgentManager>();
            context.Steering = context.InputController.GetComponentInChildren<SteeringController>();
            return context;
        }
    }
}