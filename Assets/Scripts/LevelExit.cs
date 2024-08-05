using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    public string titleScreen;

    public float waitToEndLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.ending = true;
            //Debug.Log("SKRTTTT");
            StartCoroutine(EndLevelCo());
        }
    }

    private IEnumerator EndLevelCo()
    {
        yield return new WaitForSeconds(waitToEndLevel);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(titleScreen);
    }
}
