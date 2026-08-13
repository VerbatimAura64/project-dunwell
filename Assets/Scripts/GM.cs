//using Ink.Parsed;
using Ink.Runtime;
//using Ink.UnityIntegration;
using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GM : MonoBehaviour
{
    private static GM instance;
    private static Canvas screenInstance;
    public TextAsset inkAsset;
    public Story inkStory;
    public Canvas canvas;
    public GameObject objective;
    public GameObject instruct;
    public GameObject pauseScreen;
    public Camera jailCam;
    public Camera mainCam;
    public GameObject caseBoard;
    public bool isClipPlaying;
    public AudioSource auPlayer;
    public AudioClip clipPlaying;
    public GameObject clueCardObj;
    public GameObject dialogue;
    public TypeWriterEffect typeWriter;
    public GameObject arrow;
    public GameObject manager;
    public GameObject fadeScreen;
    public GameObject endMenu;
    public bool managerAlerted;
    public bool isConversation;
    public bool datapadChoice;
    public bool gameOver;
    public bool gameEnding;
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

        StartCoroutine(RevealScene());
        caseBoard.SetActive(false);
        _inkStory = new Story(inkAsset.text);
        //      InkPlayerWindow window = InkPlayerWindow.GetWindow(true);
        //    if (window != null) { InkPlayerWindow.Attach(_inkStory); }
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        if (screenInstance != null && screenInstance != canvas)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        screenInstance = canvas;
        //DontDestroyOnLoad(this);
        //DontDestroyOnLoad(canvas);

        if (SceneManager.GetActiveScene().name == "Interior")
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
        if (Input.GetKeyDown(KeyCode.Tab))
            if (arrow.activeInHierarchy)
                ContinueStory();
        IsDemoOver();
        EndGameB();
    }

    private IEnumerator Instructions()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(InstructFade(1f, 0f, 2f));

    }

    public IEnumerator InstructFade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        //Color tC = instruct.GetComponentInChildren<TextMeshProUGUI>().color;
        Color c = instruct.GetComponent<Image>().color;
        Color c2 = instruct.GetComponentInChildren<TextMeshProUGUI>().color;
        c.a = startAlpha;
        c2.a = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);
            //tC.a = Mathf.Lerp(startAlpha, endAlpha, t);
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            c2.a = Mathf.Lerp(startAlpha, endAlpha, t);
            instruct.GetComponent<Image>().color = c;
            instruct.GetComponentInChildren<TextMeshProUGUI>().color = c2;
            yield return null;
        }
        //tC.a = endAlpha;
        c.a = endAlpha;
        c2.a = endAlpha;
        //instruct.GetComponentInChildren<TextMeshProUGUI>().color = tC;
        instruct.GetComponent<Image>().color = c;
        instruct.GetComponentInChildren<TextMeshProUGUI>().color = c2;
        instruct.SetActive(false);
    }

    void IsDemoOver()
    {
        gameOver = (bool)_inkStory.variablesState["gameOver"];

    }

    public void EndGameB()
    {
        if ((bool)_inkStory.variablesState["managerCaught"] || ((bool)_inkStory.variablesState["attemptToBluff"] && !(bool)_inkStory.variablesState["bluffed"]))
        {
            Debug.LogWarning(_inkStory.variablesState["managerCaught"]);
            Debug.LogWarning(_inkStory.variablesState["attemptToBluff"]);
            Debug.LogWarning(_inkStory.variablesState["bluffed"]);
            if (gameOver)
            {
                StartCoroutine(FadeAndShowMenu());
            }
        }
    }

    public void EndGame()
    {
        if (gameOver)
            StartCoroutine(FadeAndShowMenu());
    }

    public void StartGame()
    {
        StartCoroutine(LoadNewScene());
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    private IEnumerator EndingBTransition()
    {
        yield return StartCoroutine(FadeScreen(0f, 1f, 2f));
        mainCam.enabled = false;
        jailCam.enabled = true;
        yield return StartCoroutine(FadeScreen(1f, 0f, 2f));
    }
    public IEnumerator LoadNewScene()
    {
        fadeScreen.SetActive(true);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        yield return StartCoroutine(FadeScreen(0f, 1f, 4f));
        yield return SceneManager.LoadSceneAsync(sceneIndex + 1);
        yield return new WaitForSecondsRealtime(5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex + 1);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Scene is loaded but not activated — if you reach here without crashing,
        // loading isn't the problem, activation is
        Debug.Log("Scene loaded, activating...");
        asyncLoad.allowSceneActivation = true;
        //yield return StartCoroutine(FadeScreen(1f, 0f, 2f));
        //fadeScreen.SetActive(false);
    }
    public IEnumerator RevealScene()
    {
        yield return StartCoroutine(FadeScreen(1f, 0f, 3f));
        fadeScreen.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(Instructions());
        }
    }
    private IEnumerator FadeAndShowMenu()
    {
        fadeScreen.SetActive(true);
        float duration = 2f;
        float elapsed = 0f;
        Color c = fadeScreen.GetComponent<Image>().color;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            c.a = Mathf.Clamp01(elapsed / duration);
            fadeScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(3.5f);

        endMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    public IEnumerator FadeScreen(float startAlpha, float endAlpha, float duration)
    {
        fadeScreen.SetActive(true);
        //float duration = 2f;
        float elapsed = 0f;
        float fadeDuration = 3f;
        Color c = fadeScreen.GetComponent<Image>().color;
        c.a = startAlpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeDuration;
            t = t * t * (3f - 2f * t);

            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        c.a = endAlpha;
        fadeScreen.GetComponent<Image>().color = c;
    }
    public void TriggerClueKnot(string knotTitle)
    {

        _inkStory.ChoosePathString(knotTitle);
        AdvanceDialogue();
        //ContinueStory();
    }

    public void TriggerConnectionKnot(string knotTitle)
    {

        _inkStory.ChoosePathString(knotTitle);
        AdvanceDialogue();
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
                    //manager.GetComponent<BldingManager>().BluffChance();
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
                if (tag.StartsWith("ENDING_"))
                {
                    string ending = tag.Substring(0); // Extract the clue number after "ENDING_"
                    // Load the scene using SceneManager.LoadScene(sceneName);
                    Debug.LogError(ending);
                    gameEnding = true;
                    if (ending.Equals("ENDING_B"))
                    {
                        //Fade logic here
                        AdvanceDialogue();
                        StartCoroutine(EndingBTransition());
                        objective.SetActive(false);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<vThirdPersonController>().lockMovement = true;
                        //GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<vThirdPersonCamera>().lockCamera = true;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<vThirdPersonInput>().prompt.SetActive(false);
                    }

                    //Fade logic here
                    //
                    //

                    //disable input logic
                    //
                    //
                    //Debug.Log("Clue found triggered: " + clueName);
                    //Trigger Ending sequence
                    continue;
                }
            }


        }
        else if (_inkStory.currentChoices.Count > 0)
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
                if (isConversation || datapadChoice)
                    DisplayChoices();
                else
                    dialogue.SetActive(false);
                //Choice choice = _inkStory.currentChoices[i];
                //choices.Add(false);
                //Debug.Log("Choice " + (i+1) + ": " + choice.text);
            }

        }
        else
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
        if (datapadChoice)
        {
            gameEnding = true;
            GameObject.Find("Datapads").GetComponent<BoxCollider>().enabled = false;
            GameObject.Find("Dexter's Login").GetComponent<BoxCollider>().enabled = false;
        }
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
        if (manager != null)
        {
            if (managerAlerted)
            {
                manager.SetActive(true);
                //_inkStory.ChoosePathString("bldManagerConv");
                //AdvanceDialogue();
            }
            else if ((bool)_inkStory.variablesState["foundStorageTerminal"])
            {
                manager.SetActive(true);

            }
            else
            {
                manager.SetActive(false);
            }
        }
    }
}

