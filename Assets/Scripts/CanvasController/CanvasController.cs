using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{    
    public static CanvasController cnv;
    public CanvasController()
    {
        cnv = this;
    }
    [SerializeField] public Image menu;
    [SerializeField] private ButtonCanvasController prefabButton;
    [SerializeField] private GameObject prefabStat;
    [SerializeField] private GameObject marker;
    [SerializeField] private Button _exitButton;
    public GameObject buildObject;
    public GameObject levelUpMenu; 
    private GameObject destroyButton;
    public bool showAgro = false;
    private int firstUp;
    private int secondUp;
    [SerializeField] private GameObject cooldownRedBar;
    [SerializeField] private GameObject cooldownBar;
    [SerializeField] private GameObject agroRadius;
    private GroundToPlace _ground;

    enum CanvasState
    {
        SelectTowerOrFarm,
        Select,
        ShowStats
    }
    
    private CanvasState state;
    
    private void Awake()
    {
        menu.gameObject.SetActive(false);
        _exitButton.onClick.AddListener(() => ExitButton());
        //menu = Camera.main.GetComponentInChildren<Image>();
        levelUpMenu = GameObject.FindGameObjectWithTag("LevelUpMenu");
        //buildObject = GameObject.FindGameObjectWithTag("CreateMenu");
        destroyButton = GameObject.FindGameObjectWithTag("DestroyBtn");
    }

    private void ExitButton()
    {
        switch (state)
        {
            case CanvasState.SelectTowerOrFarm:
                menu.gameObject.SetActive(false);
                break;
            case CanvasState.Select:
                for (int i = 0; i < buildObject.transform.childCount; i++)
                {
                    Destroy(buildObject.transform.GetChild(i).gameObject);
                }
                menu.gameObject.SetActive(false);
                break;
        }
    }

    private void Start()
    {
        //levelUpMenu.SetActive(false); 
        //destroyButton.SetActive(false);
    }

    public void Show(Entity entity, GroundToPlace ground)
    {
        _ground = ground;
        state = CanvasState.SelectTowerOrFarm;
        menu.gameObject.SetActive(true);
        for (int i = 0; i < buildObject.transform.childCount; i++)
        {
            Destroy(buildObject.transform.GetChild(i).gameObject);
        }

        switch (entity)
            {
                case Tower:
                    {
                    //levelup ebanu
                        break;
                    }
                case Farm:
                    {
                    //levelup
                        break;
                    }
                default:
                    {
                    ButtonCanvasController buttonForFarmMenu = Instantiate(prefabButton);
                    buttonForFarmMenu.transform.SetParent(buildObject.transform, false);
                    //buttonForFarmMenu.buttonImage.sprite = 
                    buttonForFarmMenu.buttonImage.color = Color.red;
                    buttonForFarmMenu.button.onClick.AddListener(() => ShowTowerFarm(true));

                    ButtonCanvasController buttonForTowerMenu = Instantiate(prefabButton);
                    buttonForTowerMenu.transform.SetParent(buildObject.transform, false);
                    //buttonForTowerMenu.buttonImage.sprite = 
                    buttonForTowerMenu.buttonImage.color = Color.green;
                    buttonForTowerMenu.button.onClick.AddListener(() => ShowTowerFarm(false));
                    break;
                    }
            }
        
    }

    private void ShowTowerFarm(bool tower)
    {
        state = CanvasState.Select;
        for (int i = 0; i < buildObject.transform.childCount; i++)
        {
            Destroy(buildObject.transform.GetChild(i).gameObject);
        }
        List<Entity> entities;
        if (tower)
        {
            entities = new List<Entity>(Player.instance.Towers);
        }
        else
        {
            entities = new List<Entity>(Player.instance.Farms);
        }
        foreach(var entity in entities)
        {
            if (entity is Tower tow)
            {
                ButtonCanvasController buttonTower = Instantiate(prefabButton);
                buttonTower.transform.SetParent(buildObject.transform, false);
                buttonTower.buttonImage.sprite = tow.spriteButton;
                buttonTower.button.onClick.AddListener(() => SetTower(tow));
            }
            else if (entity is Farm farm)
            {
                ButtonCanvasController buttonFarm = Instantiate(prefabButton);
                buttonFarm.transform.SetParent(buildObject.transform, false);
                buttonFarm.buttonImage.sprite = farm.spriteButton;
                buttonFarm.button.onClick.AddListener(() => SetTower(farm));
            }
        }
    }

    private void GenerateUps()
    {
        //firstUp = Random.Range(0, _tower.GetComponent<Tower>().levelUpCallbacks.Count);
        //secondUp = 0;
        //while (true)
        //{
        //    secondUp = Random.Range(0, _tower.GetComponent<Tower>().levelUpCallbacks.Count);
        //    if (secondUp == firstUp) continue;
        //    else break;
        //}
        //menu.gameObject.SetActive(false);
        //destroyButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void DestroyTower()
    {
        //Destroy(_tower);
        //destroyButton.SetActive(false);
        //menu.gameObject.SetActive(false);
        //destroyButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void SetTower(Entity tower)
    {
        GameObject _tower = Instantiate(tower.gameObject);
        Resources cost = new Resources(0,0,0,0);
        if (tower is Tower twr)
            cost = new Resources(twr.costs[0], twr.costs[1], twr.costs[2], twr.costs[3]);
        else if (tower is Farm farm)
            cost = new Resources(farm.costs[0], farm.costs[1], farm.costs[2], farm.costs[3]);
        _tower.transform.position = _ground.transform.position;
        if (Player.instance.resources.Subtract(cost))
        {
            _ground.entity = tower;
            ExitButton();
            Player.instance.create.Play();
        }
        else
        {
            Destroy(_tower);
        }
    }
}
