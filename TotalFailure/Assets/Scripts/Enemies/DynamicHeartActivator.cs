using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PatrolerData
{
    public Patroler patroler;
    public bool isDropHeart;
    public Vector2 lastPosition;

    public PatrolerData(Patroler p, bool drop)
    {
        patroler = p;
        patroler.isDropHeart = drop;
        isDropHeart = drop;
        lastPosition = p.transform.position;
    }
}

public class DynamicHeartActivator : MonoBehaviour
{
    public GameObject heartObject;
    private List<PatrolerData> patrolerDataList = new List<PatrolerData>();
    private float chance;

    void Start()
    {
        chance = Random.Range(0f, 0.5f);

        var allPatrolers = FindObjectsOfType<Patroler>();
        foreach (var p in allPatrolers)
        {
            bool drop = Random.value < chance;
            patrolerDataList.Add(new PatrolerData(p, drop));
        }

        heartObject.SetActive(false);
    }

    void Update()
    {
        // Добавление новых патрулёров
        var currentPatrolers = FindObjectsOfType<Patroler>();
        foreach (var p in currentPatrolers)
        {
            if (!patrolerDataList.Any(d => d.patroler == p))
            {
                bool drop = Random.value < chance;
                patrolerDataList.Add(new PatrolerData(p, drop));
            }
        }

        // Проверка на смерть
        foreach (var data in patrolerDataList)
        {
            if (data.patroler == null && data.isDropHeart)
            {
                data.isDropHeart = false;
                heartObject.SetActive(true);
                heartObject.transform.position = data.lastPosition;
            }
            else if (data.patroler != null)
            {
                data.lastPosition = data.patroler.transform.position;
            }
        }
    }
}