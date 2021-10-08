using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class MapData
{
    public EVENT_TYPE event_type;
    public MONSTER_TYPE monster_type;
    public int count;
    public bool isClear;
}

public enum EVENT_TYPE { EVENT1, EVENT2, EVENT3 };
public enum MONSTER_TYPE { MONSTER1, MONSTER2, MONSTER3 };

[CustomEditor(typeof(MapManager))]
public class TestInspector : Editor
{
    SerializedProperty abc;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("isTrue"));
        if (serializedObject.FindProperty("isTrue").boolValue)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("a"));

        serializedObject.ApplyModifiedProperties();
    }
}

public class MapManager : MonoBehaviour
{
    public static MapManager Inst { get; private set; }

    [SerializeField]
    bool isTrue;
    [SerializeField]
    int a;
    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void IconMouseUp(MapData mapData)
    {
        FadeManager.Inst.FadeOut();
    }
}
