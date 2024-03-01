using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ScriptableObjectDependencyInjection
{
    [CustomPropertyDrawer(typeof(BoolReference), true)]
    public class BoolReferenceDrawer : ScriptableReferenceDrawer { }

    [CustomPropertyDrawer(typeof(FloatReference), true)]
    public class FloatReferenceDrawer : ScriptableReferenceDrawer { }

    [CustomPropertyDrawer(typeof(StringReference), true)]
    public class StringReferenceDrawer : ScriptableReferenceDrawer { }

    [CustomPropertyDrawer(typeof(IntegerReference), true)]
    public class IntegerReferenceDrawer : ScriptableReferenceDrawer { }

    [CustomPropertyDrawer(typeof(GameObjectReference), true)]
    public class GameObjectReferenceDrawer : ScriptableReferenceDrawer { }
}