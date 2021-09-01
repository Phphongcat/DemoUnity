using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemControl : MonoBehaviour
{
    private Animator amin;
    [SerializeField] private float timeTodead;

    void Start()
    {
        amin = gameObject.GetComponent<Animator>();
    }

    IEnumerator PlayAnimationDead()
    {
        amin.SetBool("dead", true);
        yield return new WaitForSeconds(timeTodead);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "player")
        {
            StartCoroutine(PlayAnimationDead());
        }
    }
}
