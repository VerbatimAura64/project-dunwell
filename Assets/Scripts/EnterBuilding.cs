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
    private vThirdPersonController cc;

    void OnTriggerEnter(Collider PC)
    {
        Time.timeScale = 0;
        choiceScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cc = player.GetComponent<vThirdPersonController>();
    }

    public void Confirm()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        StartCoroutine(gm.LoadNewScene());
        player.GetComponent<vThirdPersonInput>().tpCamera.GetComponent<vThirdPersonCamera>().lockCamera = true;
        cc.isSprinting = false;
        cc.input = Vector2.zero;
        cc.lockMovement = true;
        choiceScreen.SetActive(false);
        //SceneManager.LoadSceneAsync(level);
        Time.timeScale = 1;
    }

    public void EndDemo()
    {
        gm.EndGame();
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(2);
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
