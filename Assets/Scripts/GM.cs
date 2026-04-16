using UnityEngine;
using Ink.Runtime;
using Ink.UnityIntegration;
using TMPro;

public class GM : MonoBehaviour
{
    public TextAsset inkAsset;
    public Story inkStory;
    public TextMeshProUGUI dialogueText;
    
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
}
