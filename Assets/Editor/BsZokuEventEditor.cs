using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BsZokuEventEditor : Editor
{
    public SerializedProperty
     type_Prop,
     waves_Prop;


    void OnEnable()
    {
        // Setup the SerializedProperties
        type_Prop = serializedObject.FindProperty("type");
        waves_Prop = serializedObject.FindProperty("waves");
    
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(type_Prop);

        BsZokuEvent.BsZokuEventType st = (BsZokuEvent.BsZokuEventType)type_Prop.enumValueIndex;

        switch (st)
        {
            case BsZokuEvent.BsZokuEventType.BlackScreen:
                EditorGUILayout.PropertyField(waves_Prop, new GUIContent("waves"));
                break;

            case BsZokuEvent.BsZokuEventType.Conversation:
           
                break;

            case BsZokuEvent.BsZokuEventType.Waves:

                break;

        }


        serializedObject.ApplyModifiedProperties();
    }
}
