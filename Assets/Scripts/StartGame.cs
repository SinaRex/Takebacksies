using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject foreGround;
    public GameObject backGround;


    void Start() {
        Time.timeScale = 1f;
    }

    void Update() {
        Time.timeScale = 1f;
        if (Input.anyKey) {
            ChangeScene();
        }
        if (Input.GetButton("Jump1") || Input.GetButton("Jump2"))
        {
            ChangeScene();
        }

    }

    public void ChangeScene()
    {
        StartCoroutine(CharacterSelector());
    }

   private IEnumerator CharacterSelector()
    {
        backGround.GetComponent<Animator>().SetTrigger("backStart");
        yield return new WaitForSeconds(0.2f);
        foreGround.GetComponent<Animator>().SetTrigger("ForeStart");
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MapSelectScene");
    }
}
