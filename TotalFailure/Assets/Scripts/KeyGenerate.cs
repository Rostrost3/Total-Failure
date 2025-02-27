using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyGenerate : MonoBehaviour
{
    public List<Patroler> patrolerList;
    //private List<Vector2> positionPatrolersList = new List<Vector2>();
    private Vector2 positionPatroler;
    public GameObject keyObject;
    private int indexOfEnemyWithKey;


    void Start()
    {
        keyObject.SetActive(false);
        indexOfEnemyWithKey = Random.Range(0, patrolerList.Count);
        patrolerList[indexOfEnemyWithKey].isDropKey = true;
        positionPatroler = patrolerList[indexOfEnemyWithKey].transform.position;
    }


    void Update()
    {
        if (keyObject != null)
        {
            if (patrolerList[indexOfEnemyWithKey] == null)
            {
                patrolerList[indexOfEnemyWithKey].isDropKey = false;
                keyObject.SetActive(true);
                keyObject.transform.position = positionPatroler;
            }
            else
            {
                positionPatroler = patrolerList[indexOfEnemyWithKey].transform.position;
            }
        }
        
    }
}
