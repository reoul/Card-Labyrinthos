using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(EventButton))]
public class EventButtonInspector : Editor
{
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("reward_kind"));
        int propertyfield = this.serializedObject.FindProperty("reward_kind").enumValueIndex;
        switch (propertyfield)
        {
            case (int)REWARD_KIND.ONE:
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("reward_type1_1"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("index1_1"));
                break;
            case (int)REWARD_KIND.TWO:
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("reward_type1_1"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("index1_1"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("reward_type1_2"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("index1_2"));
                break;
            case (int)REWARD_KIND.RANDOM:
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("first_reward_probability"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("reward_type1_1"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("index1_1"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("reward_type2"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("index2"));
                break;
        }
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("limitNumMin"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("limitNumMax"));

        this.serializedObject.ApplyModifiedProperties();
    }
}
#endif

public enum REWARD_KIND { ONE, TWO, RANDOM }

[Serializable]
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
            return sum >= this.limitNumMin && sum <= this.limitNumMax;
        }
    }

    public EventData eventData { get { return new EventData(this.reward_kind, this.first_reward_probability, this.reward_type1_1, this.index1_1, this.reward_type1_2, this.index1_2, this.reward_type2, this.index2); } }

    bool onEvent;   //마우스가 필드 위에 있는지
    void OnMouseUp()
    {
        if (this.onEvent && this.IsAchieve && !RewardManager.Inst.activeRewardWindow && !FadeManager.Inst.isActiveFade && CardManager.Inst.MyHandCards.Count >= 3)
        {
            this.transform.parent.GetComponent<Event>().MouseUp(this.eventData);
        }
    }
    void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        this.onEvent = true;
    }
    void OnMouseExit()
    {
        this.onEvent = false;
    }
}
