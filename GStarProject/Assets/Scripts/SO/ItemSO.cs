using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Designated,
    NonDesignated
}

[System.Serializable]
public class Item
{
    public string name; //ī�� �̸�
    [Range(0,3)]
    public int cost;    //�ش� ī�� ���
    public CardType type;
    public Sprite sprite;
    [Header("����")]
    public int attack;  //�ش����ŭ ������ �������� ����
    public int attack_count;    //���� Ƚ��
    public bool allememy;   //��ü ������ �������� ��

    [Header("���")]
    public int shield;  //�ش����ŭ �������� ex)���� 3�ε� ���ݷ��� 4�̸� �� ĳ���Ϳ��� 1��ŭ�� ������ �鰨

    [Header("��ƿ")]
    public bool copy;   //���ϴ� ī�带 ���� ������ ������ �����´�
    public bool copy_throw;   //���ϴ� ī�带 ���� ������ ���� ī�� ���̿� �д�
    public int get_cost;    //�ش� �� ��ŭ �ڽ�Ʈ�� ��´�
    public int less_damage; //���� ���ݷ¿� �ش� ����ŭ ����
    public int draw_card;   //�ش� ����ŭ ī�带 ��ο� �ؿ´�
    public int force;   //�ش� ����ŭ ���ݷ��� �ö󰣴�.
    public int vulnerable;  //�ش� ����ŭ ����� �Ǵ�. ���: �������� ������ 50�� �� ���� �޴´�
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public Item[] items;
}
