using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject[] Texts;
    public float TimePerText = 2f;
    public int Index = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Loading());
        }
    }


    private IEnumerator Loading()
    {
        for (int i = 0; i < Texts.Length; i++)
        {
            Texts[i].SetActive(false);
            Index++;
            if (Index < Texts.Length)
            {
                Texts[Index].SetActive(true);
            }
            

            yield return new WaitForSecondsRealtime(TimePerText);
        }

        SceneManager.LoadScene(1);
    }
}
