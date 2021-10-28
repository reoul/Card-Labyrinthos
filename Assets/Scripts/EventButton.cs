using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(EventButton))]
public class EventButtonInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("reward_kind"));
        int propertyfield = serializedObject.FindProperty("reward_kind").enumValueIndex;
        switch (propertyfield)
        {
            case (int)REWARD_KIND.ONE:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reward_type1_1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("index1_1"));
                break;
            case (int)REWARD_KIND.TWO:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reward_type1_1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("index1_1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reward_type1_2"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("index1_2"));
                break;
            case (int)REWARD_KIND.RANDOM:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("first_reward_probability"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reward_type1_1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("index1_1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reward_type2"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("index2"));
                break;
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("limitNum"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

public enum REWARD_KIND { ONE, TWO, RANDOM }

[System.Serializable]
public class EventButton : MonoBehaviour
{
    [SerializeField]
    REWARD_KIND reward_kind;
    [SerializeField]
    [Range(0, 100)]
    [Header("첫번째 보상이 걸릴 확률")]
    int first_reward_probability;       //첫번째 보상이 걸릴 확률
    [SerializeField]
    [Header("첫번째 보상")]
    EVENT_REWARD_TYPE reward_type1_1;
    [SerializeField]
    int index1_1;
    [SerializeField]
    EVENT_REWARD_TYPE reward_type1_2;
    [SerializeField]
    int index1_2;
    [SerializeField]
    [Header("두번째 보상")]
    EVENT_REWARD_TYPE reward_type2;
    [SerializeField]
    int index2;

    public int limitNumMin;    //이벤트 버튼 숫자 제한
    public int limitNumMax;    //이벤트 버튼 숫자 제한
    public bool IsAchieve       //조건을 통과하는지 검사하는 프로퍼티
    {
        get
        {
            int sum = CardManager.Inst.HandCardNumSum;
            return sum >= limitNumMin && sum <= limitNumMax;
        }
    }

    public EventData eventData { get { return new EventData(reward_kind, first_reward_probability, reward_type1_1, index1_1, reward_type1_2, index1_2, reward_type2, index2); } }

    bool onEvent = false;   //마우스가 필드 위에 있는지
    void OnMouseUp()
    {
        if (onEvent && IsAchieve && !RewardManager.Inst.activeRewardWindow && !FadeManager.Inst.isActiveFade)
        {
            this.transform.parent.GetComponent<Event>().MouseUp(eventData);
        }
    }
    void OnMouseEnter()
    {
        onEvent = true;
    }
    void OnMouseExit()
    {
        onEvent = false;
    }
}
