using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Invector.CharacterController;

[RequireComponent(typeof(BoxCollider))]
public class EnterBuilding : MonoBehaviour
{
    [HideInInspector] public bool pause = false;
    public GameObject choiceScreen;
    public GameObject player;
    public GM gm;
    private Collider PC;
    public string level;
    public int loadLevel;

    void OnTriggerEnter(Collider PC)
    {
        Time.timeScale = 0;
        choiceScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Confirm()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        StartCoroutine(gm.LoadNewScene());
        player.GetComponent<vThirdPersonController>().lockMovement = true;
        //SceneManager.LoadSceneAsync(level);
        Time.timeScale = 1;
    }

    public void EndDemo()
    {
        gm.EndGame();
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
    }

    public void Deny()
    {
        choiceScreen.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PC = player.GetComponent<Collider>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GM>();
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);
    }
}
