using Ink.Runtime;
using Ink.UnityIntegration;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GM : MonoBehaviour
{
    public TextAsset inkAsset;
    public Story inkStory;
    public TextMeshProUGUI dialogueText;

    public bool isClipPlaying;
    public AudioSource auPlayer;
    public AudioClip clipPlaying;
    public List<AudioClip> queueList;
    //public Queue<AudioClip> toBePlayed;
    //public List<bool> choices;
    public List<GameObject> cluesFound;
    //public int goodChoice, badChoice;
    //public TextAsset inkAsset;
    public Story _inkStory;
    public GameObject dialogue;
    public bool dialogueIsPlaying;

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

    public ClueList clueList = new();
    // Start is called before the first frame update
    void Awake()
    {
        _inkStory = new Story(inkAsset.text);
        InkPlayerWindow window = InkPlayerWindow.GetWindow(true);
        if (window != null) { InkPlayerWindow.Attach(_inkStory); }
        Queue<AudioClip> queueList = new();
        auPlayer = GetComponent<AudioSource>();
        //_inkStory.variablesState["good_count"] = goodChoice;
        //_inkStory.variablesState["bad_count"] = badChoice;
        //dialogue.GetComponent<TMP_Text>().text = "";
        _inkStory.Continue();
    }

    // Update is called once per frame
    void Update()
    {
        IsClipOn();
        //while (_inkStory.canContinue)
        {
            //Debug.Log
        }
        //_inkStory.variablesState["good_count"] = goodChoice;
        //_inkStory.variablesState["bad_count"] = badChoice;
        //ToBePlayed();

        //toBePlayed.Enqueue(queueList.First());
    }

    public void ContinueStory()
    {
        if (_inkStory.canContinue)
        {


            dialogue.GetComponent<TMP_Text>().text = _inkStory.Continue();
            dialogue.SetActive(true);
        }
    }

    void IsClipOn()
    {
        if (auPlayer.isPlaying)
        {
            isClipPlaying = true;

        }
        else
        {
            isClipPlaying = false;
            PlayNext();
            //clipPlaying = null;
        }
    }

    //public void EnterDialogueMode(TextAsset inkAsset)
    //{

    //    }

    public void PlayClip()
    {

        auPlayer.Play();

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
        auPlayer.resource = clipPlaying;
        auPlayer.Play();
        queueList.Remove(clipPlaying);
        //.Peek();
        //toBePlayed.Dequeue();

    }

    public void MakeChoice(bool choice)
    {
        if (choice)
        {
            //goodChoice++;
            //_inkStory.variablesState["good_count"] = goodChoice;
            //_inkStory.ChooseChoiceIndex(0);
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
        // _inkStory.variablesState["clueInspected"] = clueInspected;
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

