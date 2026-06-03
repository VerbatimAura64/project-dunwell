using TMPro;
using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(TMP_Text))]
public class TypeWriterEffect : MonoBehaviour
{
    public TMP_Text _textBox;

    [Header("Test String")]
    [SerializeField] private string testText;

    public int currentVisibleCharacterIndex;
    private Coroutine typewriterCoroutine;
    public bool _readyForNewText = true;
    public GameObject nextArrow;
    private WaitForSeconds simpleDelay;
    private WaitForSeconds interpunctuationDelay;

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 30;
    [SerializeField] private float interpunctuationDelay_ = 0.5f;


    public bool currentlySkipping { get; set; }
    private WaitForSeconds skippingDelay;
    [Header("Skip Options")]
    [SerializeField] private bool quickSkip;
    [SerializeField][Min(1)] private int skipSpeedUp = 5;

    private WaitForSeconds _textboxFullEventDelay;
    [SerializeField][Range(.01f, 0.5f)] private float sendDoneDelay = .25f;

    public static event Action CompleteTextRevealed;
    public static event Action<char> CharacterRevealed;



    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
        nextArrow.SetActive(false);
        simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        interpunctuationDelay = new WaitForSeconds(interpunctuationDelay_);

        skippingDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedUp));

        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }

    private void Start()
    {
        _textBox.maxVisibleCharacters = 0;
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
    }

    private void OnEnable()
    {
        _textBox.maxVisibleCharacters = 0;
    }

    /*private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
    }*/



    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
            {
                Skip();
            }
        }
    }


    void Skip()
    {
        if (currentlySkipping)
            return;
        currentlySkipping = true;

        if (!quickSkip)
        {
            StartCoroutine(routine: SkipSpeedupReset());
            return;
        }

        StopCoroutine(typewriterCoroutine);
        _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
        _readyForNewText = true;
        CompleteTextRevealed?.Invoke();
    }

    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
        currentlySkipping = false;
    }


    public void PrepareForNewText(UnityEngine.Object obj)
    {
        if (!_readyForNewText)
        {
            return;
        }

        _readyForNewText = false;

        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);

        }


        _textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;

        typewriterCoroutine = StartCoroutine(routine: TypeWriter());
    }

    public IEnumerator TypeWriter()
    {
        TMP_TextInfo textInfo = _textBox.textInfo;
        while (currentVisibleCharacterIndex < textInfo.characterCount + 1)
        {

            var lastCharacterIndex = textInfo.characterCount - 1;

            /*if (currentVisibleCharacterIndex == lastCharacterIndex)
            {
                _textBox.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;
                CompleteTextRevealed?.Invoke();
                _readyForNewText = true;
                yield break;
            }*/

            char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;

            _textBox.maxVisibleCharacters++;

            if (!currentlySkipping && (character == '?' || character == '.' || character == ','
                || character == ':' || character == ';'
                || character == '!' || character == '-'))
            {

                yield return interpunctuationDelay;
            }

            else
            {
                yield return currentlySkipping ? skippingDelay : simpleDelay;
            }
            currentVisibleCharacterIndex++;
            
            if(currentVisibleCharacterIndex >= lastCharacterIndex)
            {
                nextArrow.SetActive(true);
            } else
            {
                nextArrow.SetActive(false);
            }
        }
        
        
    }
}
