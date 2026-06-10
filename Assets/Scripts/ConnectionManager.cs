using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    public RectTransform connectionContainer;
    [SerializeField]private Clue _firstSelected = null;
    private GM GM;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GM>();
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
        Debug.Log(a.name +"_"+ b.name);
        string connectionKey = GetConnectionKey(a.name, b.name);

        if (_madeConnections.Contains(connectionKey)) return;

        if (validConnections.ContainsKey(connectionKey))
        {
            _madeConnections.Add(connectionKey);
            //GM.TriggerClueKnot(validConnections[connectionKey]);
            Debug.Log("Connection Made");
            DrawConnection(a, b);
        }

    }

    private string GetConnectionKey(string clueA, string clueB)
    {
        string[] sorted = new string[] { clueA, clueB };
        System.Array.Sort(sorted);
        return sorted[0] + "_" + sorted[1];
    }

    private Dictionary<string, string> validConnections = new Dictionary<string, string>()
    {
        {"Body_Gun", "connection_body_gun" }
    };

    private void DrawConnection(Clue a, Clue b)
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        lineObj.transform.SetParent(connectionContainer, false);



        Image line = lineObj.AddComponent<Image>();
        line.color = Color.black;

        RectTransform lineRect = lineObj.GetComponent<RectTransform>();

        Vector2 posA = a.GetComponent<RectTransform>().localPosition;
        Vector2 posB = b.GetComponent<RectTransform>().localPosition;

        Vector2 midpoint = (posA + posB) / 2f;
        float distance = Vector2.Distance(posA,posB);
        float angle = Mathf.Atan2(posB.y - posA.y, posB.x - posA.x) * Mathf.Rad2Deg;

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
