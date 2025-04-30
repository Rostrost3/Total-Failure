using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonWaveAttack : MonoBehaviour
{
    public float attackDelay = 15f;
    public GameObject fireWave;
    public Transform spawnPoint;
    private bool isAttacking;

    public CharonMovement charon;

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(WaveAttack());
        }
    }

    IEnumerator WaveAttack()
    {
        //animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay);

        GameObject wave = Instantiate(fireWave, spawnPoint.position, Quaternion.identity);
        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        wave.GetComponent<CharonFireWave>().SetDirection(dir);

        isAttacking = false;
    }
}
