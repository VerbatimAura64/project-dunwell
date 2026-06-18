using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    public RectTransform connectionContainer;
    [SerializeField]private Clue _firstSelected = null;
    private List<CardConnection> activeConnections = new List<CardConnection>();
    private GM GM;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GM>();
    }

    private void Update()
    {
        foreach (CardConnection connection in activeConnections)
        {
            UpdateLine(connection);
        }
    }

    private void UpdateLine(CardConnection connection)
    {
        Vector2 localA = connectionContainer.InverseTransformPoint(connection.cardA.GetComponent<RectTransform>().position);
        Vector2 localB = connectionContainer.InverseTransformPoint(connection.cardB.GetComponent<RectTransform>().position);

        Vector2 midpoint = (localA + localB) / 2f;
        float distance = Vector2.Distance(localA, localB);
        float angle = Mathf.Atan2(localB.y - localA.y, localB.x - localA.x) * Mathf.Rad2Deg;

        connection.lineRect.anchoredPosition = midpoint;
        connection.lineRect.sizeDelta = new Vector2(distance, 3f);
        connection.lineRect.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnCardClicked(Clue clueCard)
    {
        if (_firstSelected == null)
        {
            _firstSelected = clueCard;
            clueCard.SetSelected(true);
        }
        else if (_firstSelected == clueCard)
        {
            _firstSelected.SetSelected(false);
            _firstSelected = null;
        }
        else
        {
            TryConnect(_firstSelected, clueCard);
            _firstSelected.SetSelected(false);
            _firstSelected = null;
        }

    }
    private HashSet<string> _madeConnections = new HashSet<string>();

    private void TryConnect(Clue a, Clue b)
    {
        a.name = a.name.Replace(" ", "");
        b.name = b.name.Replace(" ", "");
        Debug.Log(a.name +"_"+ b.name);
        string connectionKey = GetConnectionKey(a.name, b.name);

        if (_madeConnections.Contains(connectionKey)) { return; };//RectTransform lineRect = CreateLineObject(); }
        

        if (validConnections.ContainsKey(connectionKey))
        {
            _madeConnections.Add(connectionKey);
            if (!GM.dialogue.activeInHierarchy) GM.dialogue.SetActive(true);
            GM.TriggerConnectionKnot(validConnections[connectionKey]);
            //Debug.Log(connectionKey);
            RectTransform lineRect = CreateLineObject();
            activeConnections.Add(new CardConnection(a,b,lineRect));
            //DrawConnection(a, b);
        }

    }

    private RectTransform CreateLineObject()
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        lineObj.transform.SetParent(connectionContainer, false);
        Image line = lineObj.AddComponent<Image>();
        line.color = Color.black;

        return lineObj.GetComponent<RectTransform>();
    }

    private string GetConnectionKey(string clueA, string clueB)
    {
        
        string[] sorted = new string[] { clueA, clueB };
        System.Array.Sort(sorted);
        return sorted[0] + "_" + sorted[1];
    }

    private Dictionary<string, string> validConnections = new Dictionary<string, string>()
    {
        {"Body_Gun", "connectionBodyGun" },
        {"FineDoc_NeighborDatapad", "connectionNeighborFine" },
        {"FineDoc_WallScreen", "connectionScreenFine" },
        {"Desk_Gun", "connectionDeskGun"},
        {"DexterDatapad_NeighborDatapad","connectionDatapads"},
        {"Desk_Dexter'sDrive","connectionDexDrive"}
    };

    private void DrawConnection(Clue a, Clue b)
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        lineObj.transform.SetParent(connectionContainer, false);



        Image line = lineObj.AddComponent<Image>();
        line.color = Color.black;

        RectTransform lineRect = lineObj.GetComponent<RectTransform>();

        Vector2 posA = a.GetComponent<RectTransform>().anchoredPosition;
        RectTransform rectA = a.GetComponent<RectTransform>();
        RectTransform rectB = b.GetComponent<RectTransform>();

        // World position reflects the card's true on-screen location,
        // whether placed by spline, drag, or layout
        Vector2 localA = connectionContainer.InverseTransformPoint(rectA.position);
        Vector2 localB = connectionContainer.InverseTransformPoint(rectB.position);

        Vector2 midpoint = (localA + localB) / 2f;
        float distance = Vector2.Distance(localA, localB);
        float angle = Mathf.Atan2(localB.y - localA.y, localB.x - localA.x) * Mathf.Rad2Deg;



        //Vector2 posB = b.GetComponent<RectTransform>().anchoredPosition;

        //Vector2 midpoint = -(posA + posB) /4f ;
        //float distance = Vector2.Distance(posA,posB);
        //float angle = Mathf.Atan2(posB.y - posA.y, posB.x - posA.x) * Mathf.Rad2Deg;

        lineRect.anchoredPosition = midpoint;
        lineRect.sizeDelta = new Vector2(distance, 3f);
        lineRect.rotation = Quaternion.Euler(0, 0, angle);

        /*
        line.positionCount = 2;
        line.startWidth = 2f;
        line.endWidth = 2f;
        //line.material = connectionLineMaterial;

        line.SetPosition(0, a.transform.position);
        line.SetPosition(1, b.transform.position);
    */}

    public void ClearConnections()
    {
        foreach(Transform child in connectionContainer)
        {
            Destroy(child.gameObject);
        }
    }

    

   
}

[System.Serializable]
public class CardConnection
{
    public Clue cardA;
    public Clue cardB;
    public RectTransform lineRect;

    public CardConnection (Clue a, Clue b, RectTransform line)
    {
        cardA = a;
        cardB = b;
        lineRect = line;
    }

    private List<CardConnection> _activeConnections = new List<CardConnection>();

}