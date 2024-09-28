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
    [SerializeField] private ButtonCanvasController _prefabButton;
    [SerializeField] private Button _exitButton;
    public GameObject buildObject;
    public GameObject levelUpMenu; 
    [SerializeField] private ButtonCanvasController _destroyButton;
    public bool showAgro = false;
    [SerializeField] private GroundToPlace _ground;
    private Entity _entity;

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
        _destroyButton.button.onClick.AddListener(() => DestroyTower());
    }

    public static void ExitButton()
    {
        switch (cnv.state) //€ какую-то хуйню сделал, главное работает!
        {
            case CanvasState.SelectTowerOrFarm: //тип тавера
                for (int i = 0; i < cnv.levelUpMenu.transform.childCount; i++)
                {
                    Destroy(cnv.levelUpMenu.transform.GetChild(i).gameObject);
                }
                for (int i = 0; i < cnv.buildObject.transform.childCount; i++)
                {
                    Destroy(cnv.buildObject.transform.GetChild(i).gameObject);
                }
                cnv.menu.gameObject.SetActive(false);
                break;
            case CanvasState.Select: //конкретный тавер
                for (int i = 0; i < cnv.buildObject.transform.childCount; i++)
                {
                    Destroy(cnv.buildObject.transform.GetChild(i).gameObject);
                }
                cnv.menu.gameObject.SetActive(false);
                break;
        }
    }

    public void Show(Entity entity, GroundToPlace ground)
    {
        _ground = ground;
        _entity = entity;
        state = CanvasState.SelectTowerOrFarm;
        ExitButton();
        menu.gameObject.SetActive(true);
        _destroyButton.gameObject.SetActive(true);
        switch (entity)
        {
            case Tower twr:
                {
                    for(int i =0; i < 2; i++)
                    {
                        levelUpMenu.gameObject.SetActive(true);
                        ButtonCanvasController firstLevelUp = Instantiate(_prefabButton);
                        firstLevelUp.transform.SetParent(levelUpMenu.transform, false);
                        LevelUp.LevelUpCallback action = twr.levelUpCallbacks[i == 1 ? twr.firstUp : twr.secondUp];
                        firstLevelUp.buttonImage.sprite = Tower.levelUpCallbackNames[action];
                        firstLevelUp.button.onClick.AddListener(() => action(twr));
                    }
                    break;
                }
            case Farm:
                {
                    //levelup
                    break;
                }
            default:
                {
                    ButtonCanvasController buttonForFarmMenu = Instantiate(_prefabButton);
                    buttonForFarmMenu.transform.SetParent(buildObject.transform, false);
                    //buttonForFarmMenu.buttonImage.sprite = 
                    buttonForFarmMenu.buttonImage.color = Color.red;
                    buttonForFarmMenu.button.onClick.AddListener(() => ShowTowerFarm(true));

                    ButtonCanvasController buttonForTowerMenu = Instantiate(_prefabButton);
                    buttonForTowerMenu.transform.SetParent(buildObject.transform, false);
                    //buttonForTowerMenu.buttonImage.sprite = 
                    buttonForTowerMenu.buttonImage.color = Color.green;
                    buttonForTowerMenu.button.onClick.AddListener(() => ShowTowerFarm(false));
    
                    _destroyButton.gameObject.SetActive(false);
                    return;
                }
        }
        //показ статов
        Debug.Log("Ѕашн€€ стоит!");
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
                ButtonCanvasController buttonTower = Instantiate(_prefabButton);
                buttonTower.transform.SetParent(buildObject.transform, false);
                buttonTower.buttonImage.sprite = tow.spriteButton;
                buttonTower.button.onClick.AddListener(() => SetTower(tow));
            }
            else if (entity is Farm farm)
            {
                ButtonCanvasController buttonFarm = Instantiate(_prefabButton);
                buttonFarm.transform.SetParent(buildObject.transform, false);
                buttonFarm.buttonImage.sprite = farm.spriteButton;
                buttonFarm.button.onClick.AddListener(() => SetTower(farm));
            }
        }
    }

    public void DestroyTower()
    {
        Destroy(_entity.gameObject);
        _ground.entity = null;
        ExitButton();
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
            _ground.entity = _tower.GetComponent<Entity>();
            ExitButton();
            Player.instance.create.Play();
        }
        else
        {
            Destroy(_tower);
        }
    }
}
