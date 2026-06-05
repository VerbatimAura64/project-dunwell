using Invector.CharacterController;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Clue : MonoBehaviour
{
    [SerializeField]
    private GM gm;
    public bool relevant;
    public bool discovered;
    public bool isClueCard;
    public GameObject clueCardObj;
    public string clueName;
    public string description;
    public Sprite clueSprite;

    public GameObject clueObj;
    public string inkKnotTitle;
    public int[] inkChoice;

    #region ClueCard Variables
    [Header("Clue Card Variables")]
    public TextMeshProUGUI cardClueName;
    //public Image cardClueImage;
    //Texture2D clueTexture;
    #endregion

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
        
        gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
        
        if(isClueCard)
        {
            clueCardObj = this.gameObject;
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
                //GameObject.Find("Dunwell").GetComponent<vThirdPersonInput>().clueTriggered = false;
                
            }
        }
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
