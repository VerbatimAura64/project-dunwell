using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnterBuilding : MonoBehaviour
{
    [HideInInspector] public bool pause = false;
    public GameObject choiceScreen;
    public GameObject player;
    private Collider PC;
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
        SceneManager.LoadSceneAsync(loadLevel);
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
    }

    // Update is called once per frame
}
