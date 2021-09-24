using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    [SerializeField] private GameObject winGame;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag == "player" && winGame != null && !winGame.gameObject.activeSelf)
        {
            StartCoroutine(WinGameSunny());
        }
    }

    IEnumerator WinGameSunny()
    {
        yield return new WaitForSeconds(0.5f);
        winGame.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
