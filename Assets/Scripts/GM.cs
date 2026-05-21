//using Ink.Parsed;
using Ink.Runtime;
using Ink.UnityIntegration;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    //public bool dialogueIsPlaying;
    public Clue clueScript;
    public List<AudioClip> queueList;
    public List<GameObject> cluesFound;
    public List<GameObject> clueCards;
    public Story _inkStory;
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
        dialogue.GetComponent<TMP_Text>().text = _inkStory.currentText;
    }

    // Update is called once per frame
    void Update()
    {
        IsClipOn();
        //DiscoverClue();
        if(Input.GetKeyDown(KeyCode.Return))
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
            dialogue.GetComponent<TMP_Text>().text = _inkStory.Continue();
            dialogue.SetActive(true);

            List<string> tags = _inkStory.currentTags;
            foreach (string tag in tags)
            {
                if (tag.StartsWith("SCENE_"))
                {
                    string sceneName = tag.Substring(6); // Extract the scene name after "SCENE_"
                                                         // Load the scene using SceneManager.LoadScene(sceneName);
                    Debug.Log("Scene change triggered: " + sceneName);

                }
                if (tag.StartsWith("CLUE_"))
                {
                    string clueName = tag.Substring(0); // Extract the clue number after "CLUE_"
                                                        // Load the scene using SceneManager.LoadScene(sceneName);
                    Debug.Log("Clue found triggered: " + clueName);

                }
            }
        }

    }

    public void ContinueStory()
    {
        if (_inkStory.canContinue)
        {
            dialogue.GetComponent<TMP_Text>().text = _inkStory.Continue();
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

            if(_inkStory.currentChoices.Count > 0)
            {
                //choices = new List<bool>();
                for (int i = 0; i < _inkStory.currentChoices.Count; i++)
                {
                    //Choice choice = _inkStory.currentChoices[i];
                    //choices.Add(false);
                    //Debug.Log("Choice " + (i+1) + ": " + choice.text);
                }
            }
        }
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

    //public void EnterDialogueMode(TextAsset inkAsset)
    //{

    //    }

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

    public void MakeChoice(string choice)
    {
        if (!string.IsNullOrEmpty(choice))
        {
            //goodChoice++;
            //_inkStory.variablesState["good_count"] = goodChoice;
            //_inkStory.ChooseChoiceIndex(0);
            //Debug.Log(_inkStory.currentChoices);//ChoosePathString(choice);
            ContinueStory();
        }
        else
        {
            //badChoice++;
            //_inkStory.variablesState["bad_count"] = badChoice;
            //_inkStory.ChooseChoiceIndex(1);
            ContinueStory();
        }

    }

    public void DiscoverClue()
    {

        clueList.clues = new ClueInfo[cluesFound.Count];
        //clueCards
        for (int i = 0; i < cluesFound.Count; i++)
        {
            clueScript = cluesFound[i].GetComponent<Clue>();
            clueList.clues[i] = new ClueInfo
            {
                name = cluesFound[i].GetComponent<Clue>().clueName,
                description = cluesFound[i].GetComponent<Clue>().description,
                discovered = cluesFound[i].GetComponent<Clue>().discovered,
                clueObj = cluesFound[i],
                inkKnotTitle = cluesFound[i].GetComponent<Clue>().inkKnotTitle,
                inkChoice = cluesFound[i].GetComponent<Clue>().inkChoice

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


}

/*
[System.Serializable]
    public class Clue
    {
        public string name;
        public string description;
        public bool discovered;
        public GameObject clueObj;
        public string inkKnotTitle;
    }

    [System.Serializable]
    public class ClueList
    {
        public Clue[] clues;
    }


    private void Awake()
    {
        inkStory = new Story(inkAsset.text);
        InkPlayerWindow window = InkPlayerWindow.GetWindow(true);
        if (window != null) { InkPlayerWindow.Attach(inkStory); }

    }

    public void ContinueStory()
    {
        if (inkStory.canContinue)
        {
            dialogueText.GetComponent<TextMeshProUGUI>().text = inkStory.Continue();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}*/

