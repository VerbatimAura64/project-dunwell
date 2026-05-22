using UnityEngine;
using TMPro;
using Ink;

public class Terminal : MonoBehaviour
{
    public bool storageTerminal;
    public GM gm;
    public Door door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
        if(storageTerminal)
            door = GameObject.Find("APT4").GetComponent<Door>();
    }

    public void StartTerminal()
    {
        if (storageTerminal)
        {

            RemoteUnlock();
        }
    }

    void RemoteUnlock()
    {
        if (door != null)
        {
            door.locked = false;
            gm._inkStory.ChoosePathString("findTerminal");
            gm.dialogue.GetComponent<TextMeshProUGUI>().text = gm._inkStory.Continue();
            //door.OpenDoor();
        }
    }

    void TerminalHackGame()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
