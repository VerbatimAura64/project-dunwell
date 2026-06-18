using Invector.CharacterController;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Clue : MonoBehaviour
{
    [SerializeField]
    private GM gm;
    [SerializeField]
    private GameObject player;
    public bool relevant;
    public bool discovered;
    public bool isClueCard;
    public bool isInteractable;
    //[SerializeField]
    public Vector3 ogPos;
    public Quaternion ogDirection;
    public GameObject clueCardObj;
    public string clueName;
    public string description;
    public Sprite clueSprite;
    [SerializeField]
    private bool _selected;

    public GameObject clueObj;
    public string inkKnotTitle;
    public int[] inkChoice;

    #region ClueCard Variables
    [Header("Clue Card Variables")]
    public TextMeshProUGUI cardClueName;
    //public Image cardClueImage;
    //Texture2D clueTexture;
    #endregion

    public void SetSelected(bool selected)
    {
        _selected = selected;
        GetComponent<Image>().color = selected ? Color.red : Color.black;

    }

    public bool IsSelected() => _selected;

    private void Update()
    {
        //clueTexture = AssetPreview.GetAssetPreview(clueObj);
        //Debug.Log(clueTexture);
        WallReveal();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        this.gameObject.name = clueName;
        ogPos = this.transform.position;
        ogDirection = this.transform.rotation;
        gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
        player = GameObject.Find("Dunwell");
        
        if(isClueCard)
        {
            clueCardObj = this.gameObject;
            gm.clueCards.Add(clueCardObj);
            //clueSprite = Resources.Load<Sprite>(clueName);
            //clueTexture = AssetPreview.GetAssetPreview(clueObj);
            
            if (discovered && clueObj == null)
            {
                clueObj = GameObject.Find(clueName);
                clueCardObj = GameObject.Find(clueName);
                cardClueName = GameObject.Find("ClueName").GetComponent<TextMeshProUGUI>();
                cardClueName.name = clueCardObj.name;
                cardClueName.text = clueName;
                //cardClueImage = GameObject.Find("ClueImage").GetComponent<Image>();
                //cardClueImage.sprite = clueSprite;
                //cardClueImage.name = clueCardObj.name;
                //if (clueTexture != null)
                //{
                 //   Rect rect = new Rect(0, 0, clueTexture.width, clueTexture.height);
                   // Vector2 pivot = new Vector2(0.5f, 0.5f);
                    //Sprite newSprite = Sprite.Create(clueTexture, rect, pivot);
                    
                  //  cardClueImage.sprite = newSprite;
                //clueSprite = Sprite.Create(clueTexture, new Rect(0, 0, clueTexture.width, clueTexture.height), new Vector2(0.5f, 0.5f));
                //}
                
            }
        }
        else
        {
            clueObj = this.gameObject;
            if (discovered && clueCardObj == null)
            {
                //clueCardObj = GameObject.Find(clueName);
                //cardClueName = GameObject.Find("ClueName").GetComponent<TextMeshProUGUI>();
                //cardClueName.name = clueCardObj.name;
                //cardClueName.text = clueName;
                //clueCardObj.GetComponent<Clue>().cardClueName = cardClueName;
                //clueCardObj.GetComponent<Clue>().cardClueName.text = clueName;
                //clueCardObj = GameObject.FindWithTag("ClueCard");
            } 
        }
        
                

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Activate choices");
            player.GetComponent<vThirdPersonInput>().focused = false;
            player.GetComponent<vThirdPersonController>().lockMovement = false;
            player.GetComponent<vThirdPersonInput>().tpCamera.GetComponent<vThirdPersonCamera>().inspectCam.enabled = false;
            player.GetComponent<vThirdPersonInput>().tpCamera.GetComponent<vThirdPersonCamera>()._camera.enabled = true;
        }
    }

    void WallReveal()
    {
        if(clueObj.name == "Wall Screen")
        {
            if ((int)gm._inkStory.variablesState["good_count"] >= 5)
            {
               
                this.clueObj.GetComponent<Clue>().enabled = false;
                this.clueObj.GetComponent<BoxCollider>().enabled = false;
                //this.clueObj.tag = "Terminal";
                GameObject.Find("WallPanelClue").GetComponent<MeshRenderer>().enabled = true;
                GameObject.Find("WallPanelClue").GetComponent<SphereCollider>().enabled = true;
                GameObject.Find("Locked Door").GetComponent<Transform>().rotation = Quaternion.Euler(0F, 90f, 0f);
                //GameObject.Find("Locked Door").GetComponent<Transform>().Rotate(0F,-90f, 0f);
                gm.managerAlerted = false;
                //GameObject.Find("Dunwell").GetComponent<vThirdPersonInput>().clueTriggered = false;
                
            }
        }

        if (clueObj.name == "Dexter's Login")
        {
            //if ((int)gm._inkStory.variablesState["good_count"] >= 5)
            if ((bool)gm._inkStory.variablesState["foundMorrowTrace"] == true)
            {

                //this.clueObj.GetComponent<Clue>().enabled = false;
                //this.clueObj.GetComponent<BoxCollider>().enabled = false;
                //this.clueObj.tag = "Terminal";
                //GameObject.Find("Datapads").GetComponent<MeshRenderer>().enabled = true;
                //player.GetComponent<vThirdPersonInput>().prompt.GetComponent<TMP_Text>().text = "Hi";
                GameObject.Find("Datapads").GetComponent<BoxCollider>().enabled = true;
                //GameObject.Find("Dunwell").GetComponent<vThirdPersonInput>().clueTriggered = false;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.name.Equals("Datapads"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //Debug.Log("Activate choices");
                
                gm.datapadChoice = true;
                gm.ContinueStory();

                gm.dialogue.SetActive(true);
            }
        }

        /*if (other.gameObject.CompareTag("BldManager"))
        {
            Debug.Log("Activate Ending B");
            other.GetComponent<NavMeshAgent>().isStopped = true;
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.GetComponent<BoxCollider>().size = new Vector3(25, 2, 30);
            //Transition to Jail Scene or view
            gm._inkStory.variablesState["managerCaught"] = true;
            //Debug.LogError(gm._inkStory.variablesState["managerCaught"]);

            
        }*/
    }

    private void Start()
    {
        /*Texture2D clueTexture = AssetPreview.GetAssetPreview(clueObj);
        if(clueTexture != null)
        {
            Rect rect = new Rect(0, 0, clueTexture.width, clueTexture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite newSprite = Sprite.Create(clueTexture, rect, pivot);
            //clueSprite = Sprite.Create(clueTexture, new Rect(0, 0, clueTexture.width, clueTexture.height), new Vector2(0.5f, 0.5f));
        }*/
    }


    /* private void OnTriggerEnter(Collider other)
     {
         if (other.gameObject.CompareTag("Player"))
         {
             GameObject Player = other.gameObject;
             if (Player.GetComponent<vThirdPersonInput>().focused)
             {
                 this.discovered = true;
             }

         }
     }*/

    // Update is called once per frame

}
