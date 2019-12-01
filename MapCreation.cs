using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour
{

    public GameObject row;
    public GameObject CheckZone;
    public Transform origin;
    public GameObject playerprefab;
    private GameObject player;

    public GameObject textMeshAnswer;

    public Color [] RowColors = new Color[8];

    HashSet<int> UsedNumbers = new HashSet<int>();

    Queue<Cluster> clusters = new Queue<Cluster>();
    Queue<GameObject> CheckZones = new Queue<GameObject>();
    public Queue<float> CheckPoints;

    private Cluster PastCluster;

    private int CreationNumber;

    private PlayerMovement pm;

    // Start is called before the first frame update
    void Awake()
    {
        CheckPoints = new Queue<float>();
        origin.position = Vector3.zero;
        CreationNumber = 0;
       
        Create(2);
        CreatePlayer();
        print("Complete");
       
        foreach(GameObject obj in CheckZones)
        {
            obj.GetComponent<ColorCheckZone>().Deactivate();
        }
        
    }

    private void Create(int num)
    {
        float rowX = row.transform.localScale.x;


        for (int i = 0; i < num; i++)
        {
            Cluster clust = new Cluster();
            for (int j = 0; j < 3; j++)
            {
                GameObject tmp = Instantiate(row, new Vector3(origin.position.x + (rowX * j), origin.position.y, origin.position.z), Quaternion.identity);
                clust.Add(tmp);
               
                    GameObject tmpText = Instantiate(textMeshAnswer, new Vector3(tmp.transform.position.x, tmp.transform.position.y + 1, tmp.transform.position.z + (row.transform.localScale.z / 2)), Quaternion.identity);
                //if (i < 1)
                //{
                    if (GetComponent<UImanager>().textMeshAnswers[j] != null) GetComponent<UImanager>().oldAnswers[j] = GetComponent<UImanager>().textMeshAnswers[j];
                    GetComponent<UImanager>().textMeshAnswers[j] = tmpText;
                //}
            }
            clusters.Enqueue(clust);
            origin.position = new Vector3(origin.transform.position.x, origin.transform.position.y, origin.transform.position.z + (row.transform.localScale.z / 2) + (CheckZone.transform.localScale.z/2));
            CheckZones.Enqueue( Instantiate(CheckZone, origin.position, Quaternion.identity));
            CheckPoints.Enqueue(origin.position.z + (CheckZone.transform.localScale.z / 2));
            origin.position = new Vector3(origin.transform.position.x, origin.transform.position.y, origin.transform.position.z + (row.transform.localScale.z / 2) + (CheckZone.transform.localScale.z / 2));

        }


        
       
    }

    private void CreatePlayer()
    {
        playerprefab = GameStateStorage.instance.GetSelectedCharacter();
        player = Instantiate(playerprefab, new Vector3(0, .75f, 0 - (row.transform.localScale.z/2)+(playerprefab.transform.localScale.z * 4) ), Quaternion.identity);
       
        
        ColorCluster(clusters.Peek().GetRows());
     
    }

    public GameObject getPlayer()
    {
        return player;
    }

    private void AssignRandomColor(GameObject tmp)
    {
        int x = RandomNumber();
        tmp.GetComponent<Renderer>().material.color = RowColors[x];

    }
    private int RandomNumber()
    {
        int x = Random.Range(0, RowColors.Length);
        if (UsedNumbers.Contains(x))
        {
           x =  RandomNumber();
        }
        
        UsedNumbers.Add(x);
        return x;
    }

    private void ColorCluster(GameObject [] objs)
    {
        UsedNumbers.Clear();
        for (int i = 0; i < objs.Length; i++)
        {
            AssignRandomColor(objs[i]);
        }
    }

    public void NextZone()
    {
        PastCluster = clusters.Dequeue();
        ColorCluster(clusters.Peek().GetRows());
        Create(1);
    }

    public void DeactivateCheckZone(GameObject obj)
    {
        obj.GetComponent<ColorCheckZone>().Deactivate();
    }
    public void ActivateCheckZone(GameObject obj, int i)
    {
        obj.GetComponent<ColorCheckZone>().Activate(i);
        CheckZones.Dequeue();
    }

    public void HighlightCheckZone(int i )
    {
        ActivateCheckZone(CheckZones.Peek(), i);
    }
}
