using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaHeartActivator : MonoBehaviour
{
    public GameObject heart;
    public GameObject miniBoss;
    private Vector2 positionMiniBoss;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        heart.SetActive(false);
        isActive = true;
        positionMiniBoss = miniBoss.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive && miniBoss == null)
        {
            heart.SetActive(true);
            heart.transform.position = positionMiniBoss;
            isActive = false;
        }
        else if(isActive && miniBoss != null)
        {
            positionMiniBoss = miniBoss.transform.position;
        }
    }
}
