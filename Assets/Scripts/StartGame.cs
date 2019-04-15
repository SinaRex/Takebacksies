using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject foreGround;
    public GameObject backGround;

    private void Start()
    {
        Time.timeScale = 1;
    }

    void Update() {
        if (Input.anyKey) {
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
