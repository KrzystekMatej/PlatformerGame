using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    [System.Serializable]
    public class BooleanKey : BlackboardKey<bool> {

    }

    [System.Serializable]
    public class IntKey : BlackboardKey<int> {

    }

    [System.Serializable]
    public class FloatKey : BlackboardKey<float> {

    }

    [System.Serializable]
    public class DoubleKey : BlackboardKey<double> {

    }

    [System.Serializable]
    public class StringKey : BlackboardKey<string> {

    }

    [System.Serializable]
    public class Vector2Key : BlackboardKey<Vector2> {

    }

    [System.Serializable]
    public class Vector3Key : BlackboardKey<Vector3> {

    }

    [System.Serializable]
    public class Vector4Key : BlackboardKey<Vector4> {

    }

    [System.Serializable]
    public class Vector2IntKey : BlackboardKey<Vector2Int> {

    }

    [System.Serializable]
    public class Vector3IntKey : BlackboardKey<Vector3Int> {

    }

    [System.Serializable]
    public class GradientKey : BlackboardKey<Gradient> {

    }

    [System.Serializable]
    public class ColorKey : BlackboardKey<Color> {

    }

    [System.Serializable]
    public class LayerKey : BlackboardKey<int> {

    }

    [System.Serializable]
    public class LayerMaskKey : BlackboardKey<LayerMask> {

    }

    [System.Serializable]
    public class TagKey : BlackboardKey<string> {

    }

    [System.Serializable]
    public class CurveKey : BlackboardKey<AnimationCurve> {

    }

    [System.Serializable]
    public class BoundsKey : BlackboardKey<Bounds> {

    }

    [System.Serializable]
    public class BoundsIntKey : BlackboardKey<BoundsInt> {

    }

    [System.Serializable]
    public class GameObjectKey : BlackboardKey<GameObject> {

    }

    [System.Serializable]
    public class MaterialKey : BlackboardKey<Material> {

    }

    [System.Serializable]
    public class RigidBodyKey : BlackboardKey<Rigidbody> {

    }

    [System.Serializable]
    public class RigidBody2DKey : BlackboardKey<Rigidbody2D>
    {

    }

    [System.Serializable]
    public class ColliderKey : BlackboardKey<Collider> {

    }

    [System.Serializable]
    public class Collider2DKey : BlackboardKey<Collider2D>
    {

    }

    [System.Serializable]
    public class WeaponKey : BlackboardKey<Weapon>
    {

    }
    [System.Serializable]
    public class IntListKey : BlackboardKey<List<int>>
    {

    }

    [System.Serializable]
    public class RaycastHit2DKey : BlackboardKey<RaycastHit2D>
    {

    }

    [System.Serializable]
    public class RaycastHit2DListKey : BlackboardKey<List<RaycastHit2D>>
    {

    }

    [System.Serializable]
    public class SeekTargeterKey : BlackboardKey<SeekTargeter>
    {

    }

    [System.Serializable]
    public class PathTargeterKey : BlackboardKey<PathTargeter>
    {

    }

    [System.Serializable]
    public class SteeringPipelineKey : BlackboardKey<SteeringPipeline>
    {

    }

    [System.Serializable]
    public class ProcessStateKey : BlackboardKey<ProcessState>
    {

    }
}
