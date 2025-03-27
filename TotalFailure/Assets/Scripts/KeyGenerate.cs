using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyGenerate : MonoBehaviour
{
    public List<EnemyClass> EnemiesList;
    
    //private List<Vector2> positionPatrolersList = new List<Vector2>();
    private Vector2 positionPatroler;
    public GameObject keyObject;
    private int indexOfEnemyWithKey;


    void Start()
    {
        keyObject.SetActive(false);
        indexOfEnemyWithKey = Random.Range(0, EnemiesList.Count);
        EnemiesList[indexOfEnemyWithKey].isDropKey = true;
        positionPatroler = EnemiesList[indexOfEnemyWithKey].transform.position;
    }


    void Update()
    {
        if (keyObject != null)
        {
            if (EnemiesList[indexOfEnemyWithKey] == null)
            {
                EnemiesList[indexOfEnemyWithKey].isDropKey = false;
                keyObject.SetActive(true);
                keyObject.transform.position = positionPatroler;
            }
            else
            {
                positionPatroler = EnemiesList[indexOfEnemyWithKey].transform.position;
            }
        }
        
    }
}
