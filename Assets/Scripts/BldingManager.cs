using Invector.CharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BldingManager : MonoBehaviour
{
    public bool endingB;
    public bool bluffed;
    public GameObject player;
    public GameObject newDestination;
    private vThirdPersonController cc;
    public GM gm;
    public Animator animController;
    public NavMeshAgent m_Agent;
    public float distance;
    public float catchDistance;
    public bool playerCaught;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GM>();
        animController = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(m_Agent.transform.position, player.transform.position);
        //Debug.Log(animController.deltaPosition);
        if(!playerCaught) {
            
            if (distance <= catchDistance) 
            {
                m_Agent.isStopped = true;
                gm.gameEnding = true;
            } 
            else if (gm.managerAlerted || (bool)gm._inkStory.variablesState["foundStorageTerminal"])
            {
                //m_Agent.velocity = (animController.deltaPosition / Time.deltaTime);
                m_Agent.isStopped = false;
                m_Agent.destination = player.transform.position;
            }
        }

        if (m_Agent.velocity.magnitude != 0f)
        {
            
            //m_Agent.speed = (animController.deltaPosition / Time.deltaTime).magnitude;
            animController.SetFloat("InputVertical", .5f);
        }
        else
        {
            animController.SetFloat("InputVertical", m_Agent.velocity.magnitude);
        }
    }

    void BluffChance()
    {
        if (Random.value < .5)
        {
            Debug.LogError("Failed to bluff!");
            bluffed = false;
        } else
        {
            Debug.LogError("BLUFFED!");
            bluffed = true;
            gm._inkStory.variablesState["bluffed"] = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!playerCaught)
            {
                BluffChance();
                if (Cursor.visible)
                    Cursor.visible = false;
                Vector3 playerDirection = -(player.transform.position - m_Agent.transform.position);
                Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
                float angle = Quaternion.Angle(targetRotation, transform.rotation);
                if (player.GetComponent<vThirdPersonInput>().focused)
                    player.GetComponent<vThirdPersonInput>().focused = false;
                    
                if (player.GetComponent<vThirdPersonInput>().caseFocused)
                    player.GetComponent<vThirdPersonInput>().caseFocused = false;
                gm.dialogue.transform.localPosition = new Vector3(0f, -415f, 0);
                gm.caseBoard.SetActive(false);
                player.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, angle);

                gm.isConversation = true;
                //player = other.gameObject;
                cc = player.GetComponent<vThirdPersonController>();
                cc.isSprinting = false;
                cc.input = Vector2.zero;
                cc.lockMovement = true;
                player.GetComponentInChildren<vThirdPersonCamera>().enabled = false;
                if (!gm.dialogue.activeInHierarchy)
                    gm.dialogue.SetActive(true);
                gm.typeWriter._readyForNewText = true;
                gm.typeWriter.PrepareForNewText(gm.dialogue);
                gm._inkStory.ChoosePathString("bldManagerConv");
                gm.dialogue.transform.GetChild(1).GetComponent<TMP_Text>().text = gm._inkStory.Continue();
                Debug.Log("Player detected");
                playerCaught = true;
            }

        }

        if (other.gameObject.CompareTag("Apt4"))
        {
            if (player.GetComponent<vThirdPersonInput>().inApt)
            {
                Debug.Log("Activate Ending B");
                gm._inkStory.variablesState["managerCaught"] = true;
                GetComponent<SphereCollider>().radius = 27f;
                GetComponent<NavMeshAgent>().isStopped = true;
                GetComponent<NavMeshAgent>().enabled = false;

            }

        }
    }

    public void Restore()
    {
        gm.isConversation = false;
        cc.lockMovement = false;
        player.GetComponentInChildren<vThirdPersonCamera>().enabled = true;

        m_Agent.destination = newDestination.transform.position;
        distance = Vector3.Distance(m_Agent.transform.position, newDestination.transform.position);
    }
}

