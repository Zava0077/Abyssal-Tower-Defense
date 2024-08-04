using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;
using static LevelUp;
using static UnityEngine.ParticleSystem;

public class CanvasController : MonoBehaviour
{
    public CanvasController()
    {
        cnv = this;
    }
    public static CanvasController cnv;
    [SerializeField] public Image menu;
    [SerializeField] private Button prefabButton;
    [SerializeField] private GameObject prefabStat;
    [SerializeField] private GameObject buildObject;
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject levelUpMenu;
    private bool setTower = false;
    public bool showAgro = false;
    int firstUp;
    int secondUp;
    private GameObject _tower;
    private Button[] levelUpBtns;
    [SerializeField] private GameObject cooldownRedBar;
    [SerializeField] private GameObject cooldownBar;
    private float barLimit;
    [SerializeField] private GameObject destroyButton;
    [SerializeField] private GameObject agroRadius;
    private void Awake()
    {
        menu = Camera.main.GetComponentInChildren<Image>();
        levelUpMenu = GameObject.FindGameObjectWithTag("LevelUpMenu");
        buildObject = GameObject.FindGameObjectWithTag("CreateMenu");
        //barLimit = cooldownRedBar.transform.localScale.x;
    }
    private void Start()
    {
        levelUpMenu.SetActive(false);
    }
    private void Update()
    {
        //cooldownBar.SetActive(setTower);
        //agroRadius.SetActive(setTower && showAgro);
        if (_tower)
        {
            //agroRadius.transform.localScale = new Vector3(_tower.GetComponent<Tower>().agroRadius/4, agroRadius.transform.localScale.y, _tower.GetComponent<Tower>().agroRadius/4);
            //cooldownRedBar.transform.localScale = new Vector3((_tower.GetComponent<Tower>().time / (1 / _tower.GetComponent<Tower>().attackSpeed)) * barLimit, cooldownRedBar.transform.localScale.y, cooldownRedBar.transform.localScale.z);
            if (_tower.GetComponent<Tower>().updateLvlUp)
            {
                GenerateUps();
                _tower.GetComponent<Tower>().updateLvlUp = false;
            }
        }
        showAgro = false;
    }
    public void Show()
    {
        menu.gameObject.SetActive(true);
        buildObject.SetActive(!setTower);
        levelUpMenu.SetActive(setTower && _tower.GetComponent<Tower>().levelUpsRemain > 0);
        foreach (var btn in GameObject.FindGameObjectsWithTag("LevelUpBtn"))
            Destroy(btn);
        if(levelUpMenu.activeSelf && _tower && levelUpMenu.GetComponentsInChildren<Button>().Length > 0)
        {
            levelUpMenu.GetComponentsInChildren<Button>()[0].onClick.RemoveAllListeners();
            levelUpMenu.GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();
            levelUpMenu.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => _tower.GetComponent<Tower>().levelUpCallbacks[firstUp](_tower.GetComponent<Tower>()));
            levelUpMenu.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => _tower.GetComponent<Tower>().levelUpCallbacks[secondUp](_tower.GetComponent<Tower>()));
            levelUpMenu.GetComponentsInChildren<Button>()[0].GetComponent<Image>().sprite = _tower.GetComponent<Tower>().levelUpCallbackNames[_tower.GetComponent<Tower>().levelUpCallbacks[firstUp]];
            levelUpMenu.GetComponentsInChildren<Button>()[1].GetComponent<Image>().sprite = _tower.GetComponent<Tower>().levelUpCallbackNames[_tower.GetComponent<Tower>().levelUpCallbacks[secondUp]];
        }
        if(!setTower)
            foreach (var tower in Camera.main.GetComponent<Player>().Towers)
            {
                Button _button = Instantiate(prefabButton);
                _button.transform.SetParent(buildObject.transform, false);
                _button.GetComponent<Image>().sprite = tower.GetComponent<Tower>().spriteButton;
                _button.onClick.AddListener(() => SetTower(tower));
            }
    }

    void GenerateUps()
    {
        firstUp = Random.Range(0, _tower.GetComponent<Tower>().levelUpCallbacks.Count);
        secondUp = 0;
        while (true)
        {
            secondUp = Random.Range(0, _tower.GetComponent<Tower>().levelUpCallbacks.Count);
            if (secondUp == firstUp) continue;
            else break;
        }
        menu.gameObject.SetActive(false);
    }

    public void DestroyTower()
    {
        Destroy(_tower);
        setTower = false;
        destroyButton.SetActive(false);
        menu.gameObject.SetActive(false);
    }

    private void SetTower(GameObject tower)
    {
        _tower = Instantiate(tower);
        _tower.transform.position = transform.position;
        if (Camera.main.GetComponent<Player>().resources.Subtract(_tower.GetComponent<Tower>().cost))
        {
            setTower = true;
            //destroyButton.SetActive(true);
            Debug.Log(tower);
            buildObject.SetActive(false);
            menu.gameObject.SetActive(false);
            Player.instance.create.Play();
        }
        else
        {
            Destroy(_tower);
        }
    }
}
