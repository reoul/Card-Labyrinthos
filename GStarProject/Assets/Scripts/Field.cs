using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Field))]
public class FieldInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("field_type"));
        if (serializedObject.FindProperty("field_type").enumValueIndex == (int)FIELD_TYPE.BATTLE)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("monster_type"));
        else if (serializedObject.FindProperty("field_type").enumValueIndex == (int)FIELD_TYPE.EVENT)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("event_type"));
        serializedObject.ApplyModifiedProperties();
    }
}
public class Field : MonoBehaviour
{
    [SerializeField]
    FIELD_TYPE field_type;
    [SerializeField]
    EVENT_TYPE event_type;
    [SerializeField]
    MONSTER_TYPE monster_type;

    public FieldData fielddata { get { return new FieldData(field_type, event_type, monster_type); } }

    void OnMouseUp()
    {
        //FieldData a = new FieldData();
        MapManager.Inst.IconMouseUp(fielddata);
    }
}
