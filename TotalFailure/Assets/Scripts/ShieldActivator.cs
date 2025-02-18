using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldActivator : MonoBehaviour
{
    public List<Patroler> patrolers; // ������ ������
    private int index; // ������ ���������� �����
    private Vector2 positionPatroler;
    public GameObject shieldObject; // ������ ���� (� �������� ��������� ������ PatrolerShield)
    private bool stop;

    void Start()
    {
        index = Random.Range(0, patrolers.Count); // �������� ���������� �����
        shieldObject.SetActive(false); // �������� �������� ���
        stop = false;
    }

    void Update()
    {
        if (!stop)
        {
            // ���������, ���� �� ����
            if (patrolers[index] == null)
            {
                stop = true;
                shieldObject.SetActive(true); // ������ ��� �������
                shieldObject.transform.position = positionPatroler; // ��������� ��� �� ����� �����
            }
            else
            {
                positionPatroler.x = patrolers[index].transform.position.x + 1;
                positionPatroler.y = patrolers[index].transform.position.y;
            }
        }
    }
}
