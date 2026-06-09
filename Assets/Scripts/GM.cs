//using Ink.Parsed;
using Ink.Runtime;
using Ink.UnityIntegration;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GM : MonoBehaviour
{
    public TextAsset inkAsset;
    public Story inkStory;
    //public TextMeshProUGUI dialogueText;
    public GameObject caseBoard;
    public bool isClipPlaying;
    public AudioSource auPlayer;
    public AudioClip clipPlaying;
    public GameObject clueCardObj;
    public GameObject dialogue;
    public TypeWriterEffect typeWriter;
    public GameObject arrow;
    public GameObject manager;
    public bool managerAlerted;
    public bool isConversation;
    public bool datapadChoice;
    public GameObject choiceButtonPrefab;
    public Transform choicesContainer;
    public Clue clueScript;
    public List<AudioClip> queueList;
    public List<GameObject> cluesFound;
    public List<GameObject> clueCards;
    public Story _inkStory;
    
    private List<GameObject> choiceButtons = new List<GameObject>();
    //public Queue<AudioClip> toBePlayed;
    //public List<bool> choices;
    //public int goodChoice, badChoice;
    //public TextAsset inkAsset;
    [System.Serializable]
    public class ClueInfo
    {
        public string name;
        public string description;
        public bool discovered;
        public GameObject clueObj;
        public string inkKnotTitle;
        public int[] inkChoice;
    }
    
    [System.Serializable]
    public class ClueList
    {
        public ClueInfo[] clues;
        
    }

    public ClueList clueList = new();
    // Start is called before the first frame update
    void Awake()
    {
        caseBoard.SetActive(false);
        _inkStory = new Story(inkAsset.text);
        InkPlayerWindow window = InkPlayerWindow.GetWindow(true);
        if (window != null) { InkPlayerWindow.Attach(_inkStory); }

        if(SceneManager.GetActiveScene().name == "Interior")
        {
            _inkStory.ChoosePathString("intMonologue");
            _inkStory.Continue();
        }
        Queue<AudioClip> queueList = new();
        
        //auPlayer = GetComponent<AudioSource>();
        //_inkStory.variablesState["good_count"] = goodChoice;
        //_inkStory.variablesState["bad_count"] = badChoice;
        //dialogue.GetComponent<TMP_Text>().text = "";
        _inkStory.Continue();
        dialogue.transform.GetChild(1).GetComponent<TMP_Text>().text = _inkStory.currentText;
    }

    // Update is called once per frame
    void Update()
    {
        IsClipOn();
        //DiscoverClue();
        ManagerAlerted();
        if (Input.GetKeyDown(KeyCode.Return))
            if(arrow.activeInHierarchy)
                ContinueStory();
        //if(Input.GetKeyDown(KeyCode.Return))
          //  PlayNext();
        //DiscoverClue();
        //while (_inkStory.canContinue)
        {
            //Debug.Log
        }
        //_inkStory.variablesState["good_count"] = goodChoice;
        //_inkStory.variablesState["bad_count"] = badChoice;
        //ToBePlayed();

        //toBePlayed.Enqueue(queueList.First());
    }

    public void TriggerClueKnot(string knotTitle)
    {
        
        _inkStory.ChoosePathString(knotTitle);
        AdvanceDialogue();
        //ContinueStory();
    }

    public void AdvanceDialogue()
    {
        if (_inkStory.canContinue)
        {
            string line = _inkStory.Continue().Trim();

            // Skip blank lines automatically
            if (string.IsNullOrWhiteSpace(line))
            {
                AdvanceDialogue();
                return;
            }
            typeWriter._readyForNewText = true;
            typeWriter.PrepareForNewText(dialogue);
            dialogue.transform.GetChild(1).GetComponent<TMP_Text>().text = line;
            //_inkStory.Continue();
            dialogue.SetActive(true);

            List<string> tags = _inkStory.currentTags;
            foreach (string tag in tags)
            {
                if (tag.StartsWith("SCENE_"))
                {
                    string sceneName = tag.Substring(6); // Extract the scene name after "SCENE_"
                                                         // Load the scene using SceneManager.LoadScene(sceneName);
                    Debug.Log("Scene change triggered: " + sceneName);
                    AdvanceDialogue(); //UI will produce a blank line, so we need to advance the dialogue again to skip it
                }
                if (tag.StartsWith("CONVO_"))
                {
                    AdvanceDialogue();
                    dialogue.SetActive(false);
                    // Load the scene using SceneManager.LoadScene(sceneName);
                    manager.GetComponent<BldingManager>().Restore();
                    Debug.Log("Conversation is Done ");
                    //ui will produce a blank line, so we need to advance the dialogue again to skip it
                    continue;
                }
                if (tag.StartsWith("CLUE_"))
                {
                    string clueName = tag.Substring(0); // Extract the clue number after "CLUE_"
                                                        // Load the scene using SceneManager.LoadScene(sceneName);
                    Debug.Log("Clue found triggered: " + clueName);

                }
            }
        }
        else
        {
            dialogue.SetActive(false);
        }

    }

    public void ContinueStory()
    {
        if (_inkStory.canContinue)
        {
            typeWriter._readyForNewText = true;
            typeWriter.PrepareForNewText(dialogue);
            dialogue.transform.GetChild(1).GetComponent<TMP_Text>().text = _inkStory.Continue();
            dialogue.SetActive(true);
            List<string> tags = _inkStory.currentTags;

            foreach (string tag in tags)
            {
                /*if (tag.StartsWith("audio:"))
                {
                    string clipName = tag.Substring(6); // Extract the clip name after "audio:"
                    AudioClip clipToPlay = Resources.Load<AudioClip>(clipName); // Load the audio clip from Resources folder
                    if (clipToPlay != null)
                    {
                        queueList.Add(clipToPlay); // Add the clip to the queue
                        Debug.Log("Added clip to queue: " + clipName);
                    }
                    else
                    {
                        Debug.LogWarning("Audio clip not found: " + clipName);
                    }
                }*/
                if (tag.StartsWith("SCENE_"))
                {
                    string sceneName = tag.Substring(6); // Extract the scene name after "SCENE_"
                    // Load the scene using SceneManager.LoadScene(sceneName);
                    Debug.Log("Scene change triggered: " + sceneName);
                    AdvanceDialogue();//ui will produce a blank line, so we need to advance the dialogue again to skip it
                    continue;
                }
                if (tag.StartsWith("CONVO_"))
                {
                    AdvanceDialogue();
                    dialogue.SetActive(false);
                    // Load the scene using SceneManager.LoadScene(sceneName);
                    manager.GetComponent<BldingManager>().Restore();
                    Debug.Log("Conversation is Done ");
                    //ui will produce a blank line, so we need to advance the dialogue again to skip it
                    continue;
                }
                if (tag.StartsWith("CLUE_"))
                {
                    string clueName = tag.Substring(0); // Extract the clue number after "CLUE_"
                    // Load the scene using SceneManager.LoadScene(sceneName);
                    Debug.Log("Clue found triggered: " + clueName);
                    continue;
                }
            }


        } else if (_inkStory.currentChoices.Count > 0)
        {

            dialogue.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            arrow.SetActive(false);
            if (isConversation || datapadChoice)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            //choices = new List<bool>();
            for (int i = 0; i < _inkStory.currentChoices.Count; i++)
            {
                if(isConversation || datapadChoice)
                    DisplayChoices();
                else
                    dialogue.SetActive(false);
                //Choice choice = _inkStory.currentChoices[i];
                //choices.Add(false);
                //Debug.Log("Choice " + (i+1) + ": " + choice.text);
            }
        
        } else
        {
            dialogue.SetActive(false);
        }
    }

    void DisplayChoices()
    {
        HideChoices();

        for (int i = 0; i < _inkStory.currentChoices.Count; i++)
        {

            Choice choice = _inkStory.currentChoices[i];
            GameObject choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
            choiceButton.GetComponentInChildren<TMP_Text>().text = choice.text;
            int choiceIndex = i; // Capture the current index for the lambda
            choiceButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => MakeChoice(choiceIndex));
            choiceButtons.Add(choiceButton);
        }

        choicesContainer.gameObject.SetActive(true);
    }
    
    void HideChoices()
    {
        foreach (GameObject choiceButton in choiceButtons)
        {
            Destroy(choiceButton);
        }
        choiceButtons.Clear();
        choicesContainer.gameObject.SetActive(false);
    
    }

    void IsClipOn()
    {
        /*if (auPlayer.isPlaying)
        {
            isClipPlaying = true;

        }
        else
        {
            isClipPlaying = false;
            PlayNext();
            //clipPlaying = null;
        }*/
    }

    public void PlayClip()
    {

        //auPlayer.Play();

        /* for(int i =0; i < choices.Length; i++)
         {
             if (choices[i].GetComponent<AudioSource>().isPlaying)
             {
                 clipPlaying = choices[i].GetComponent<AudioSource>().clip;
                 toBePlayed.Enqueue(queueList.First());
                 //queueList.Add(toBePlayed.First());
                 isClipPlaying = true;
                 Debug.Log(choices[i].GetComponent<AudioSource>().clip.name);

             }
             else
             {
                 isClipPlaying = false;
             }
         }*/

    }

    public void PlayNext()
    {
        clipPlaying = queueList.First();
        //auPlayer.resource = clipPlaying;
        //auPlayer.Play();
        queueList.Remove(clipPlaying);
        //.Peek();
        //toBePlayed.Dequeue();

    }

    public void MakeChoice(int choice)
    {
        _inkStory.ChooseChoiceIndex(choice);
        Cursor.visible = false;
        HideChoices();
        AdvanceDialogue();

    }

    public void DiscoverClue()
    {
        typeWriter._readyForNewText = true;
        typeWriter.PrepareForNewText(dialogue);
        clueList.clues = new ClueInfo[cluesFound.Count];
        //clueCards
        for (int i = 0; i < cluesFound.Count; i++)
        {
            //clueList.clues = new ClueInfo[cluesFound.Count];
            clueScript = cluesFound[i].GetComponent<Clue>();
            clueList.clues[i] = new ClueInfo
            {
                name = clueScript.clueName,
                //description = dialogue.GetComponent<TextMeshProUGUI>().text,
                discovered = clueScript.discovered,
                clueObj = cluesFound[i],
                inkKnotTitle = clueScript.inkKnotTitle,
                inkChoice = clueScript.inkChoice
            };
           
            //_inkStory.ChoosePathString(clueList.clues.Last<ClueInfo>().inkKnotTitle);

            //clueList.clues[i] = clueInfo;
        }
        //if(clueList.clues.Length > 0)
        TriggerClueKnot(clueList.clues.Last<ClueInfo>().inkKnotTitle);
        //_inkStory.
        //
        //_inkStory.Continue();
        /*if (clueList.clues.Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.Return)) { 
                if(clueList.clues.Last<ClueInfo>().inkChoice != null && clueList.clues.Last<ClueInfo>().inkChoice.Length > 0)
                {
                    MakeChoice(clueList.clues.Last<ClueInfo>().inkChoice[0]);
                }
                _inkStory.ChoosePathString(clueList.clues.Last<ClueInfo>().inkKnotTitle);
             //_inkStory.variablesState["clueInspected"] = true;
             
            }
        }*/

        //.Last<Clue>().inkKnotTitle);
       //if(Input.GetKeyDown(KeyCode.Return))
            //_inkStory.ChoosePathString(clueList.clues.Last<ClueInfo>().inkKnotTitle);
        // _inkStory.variablesState["clueInspected"] = clueInspected;
    }

    public Quaternion RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Keep only the horizontal direction
        if (direction.sqrMagnitude > 0.01f) // Avoid zero-length direction
        {
            return Quaternion.LookRotation(direction);
        }
        else
        {
            return transform.rotation; // No change in rotation if target is very close
        }
    }

    public RectTransform AddCardspace(GameObject newCard)
    {
        //GameObject newCard = Instantiate(clueCardObj, clueCardObj.transform);
        //clueCards.Add(newCard);
        newCard.transform.position = new Vector3(newCard.transform.position.x + 5f, newCard.transform.position.y, newCard.transform.position.z);
        return newCard.transform as RectTransform;
    }

    void ManagerAlerted()
    {
        if(managerAlerted)
        {
            manager.SetActive(true);
            //_inkStory.ChoosePathString("bldManagerConv");
            //AdvanceDialogue();
        }
    }
}

