using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chests : MonoBehaviour
{
    public List<TrapChest> chests;
    public GameObject keyObject;
    private int indexOfChestWithKey;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        keyObject.SetActive(false);
        indexOfChestWithKey = Random.Range(0, chests.Count);
        chests[indexOfChestWithKey].isDropKey = true;

        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(playerTransform.position, chests[indexOfChestWithKey].transform.position) <= chests[indexOfChestWithKey].activationDistance &&
            Input.GetKeyDown(KeyCode.E) && !chests[indexOfChestWithKey].isActivated && !TrapChest.isEnemyAlive)
        {
            chests[indexOfChestWithKey].isActivated = true;
            keyObject.SetActive(true);
            keyObject.transform.position = chests[indexOfChestWithKey].transform.position;
        }
    }
}
