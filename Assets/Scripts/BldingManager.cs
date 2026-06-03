using Invector.CharacterController;
using TMPro;
using UnityEngine;

public class BldingManager : MonoBehaviour
{
    public bool endingB;
    public bool bluffed;
    public GameObject player;
    private vThirdPersonController cc;
    public GM gm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gm.isConversation = true;
            player = other.gameObject;
            cc = player.GetComponent<vThirdPersonController>();
            cc.isSprinting = false;
            cc.input = Vector2.zero;
            cc.lockMovement = true;
            player.GetComponentInChildren<vThirdPersonCamera>().enabled = false;
            //player.GetComponent<vThirdPersonInput>().enabled = false;
            if (!gm.dialogue.activeInHierarchy)
                gm.dialogue.SetActive(true);
            gm.typeWriter._readyForNewText = true;
            gm.typeWriter.PrepareForNewText(gm.dialogue);
            gm._inkStory.ChoosePathString("bldManagerConv");
            gm.dialogue.transform.GetChild(1).GetComponent<TMP_Text>().text = gm._inkStory.Continue();
            Debug.Log("Player detected");
            
        }
    }

    public void Restore()
    {
        gm.isConversation = false;
        cc.lockMovement = false;
        player.GetComponentInChildren<vThirdPersonCamera>().enabled = true;
    }
}

