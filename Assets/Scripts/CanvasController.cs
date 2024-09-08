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
    [SerializeField] private GameObject marker; 
    private GameObject buildObject;
    private GameObject levelUpMenu; 
    private GameObject destroyButton;
    public bool showAgro = false;
    private int firstUp;
    private int secondUp;
    private GameObject _tower;
    [SerializeField] private GameObject cooldownRedBar;
    [SerializeField] private GameObject cooldownBar;
    [SerializeField] private GameObject agroRadius;
    private void Awake()
    {
        menu = Camera.main.GetComponentInChildren<Image>();
        levelUpMenu = GameObject.FindGameObjectWithTag("LevelUpMenu");
        buildObject = GameObject.FindGameObjectWithTag("CreateMenu");
        destroyButton = GameObject.FindGameObjectWithTag("DestroyBtn");
    }
    private void Start()
    {
        levelUpMenu.SetActive(false); 
        destroyButton.SetActive(false);
    }
    private void Update()
    {
        if (_tower)
        {
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
        buildObject.SetActive(!_tower);
        levelUpMenu.SetActive(_tower && _tower.GetComponent<Tower>().levelUpsRemain > 0);
        destroyButton.GetComponent<Button>().onClick.AddListener(() => DestroyTower());
        foreach (var btn in GameObject.FindGameObjectsWithTag("LevelUpBtn"))
            Destroy(btn);
        if (_tower)
        {
            destroyButton.SetActive(true);
            if (levelUpMenu.activeSelf && levelUpMenu.GetComponentsInChildren<Button>().Length > 0)
            {
                levelUpMenu.GetComponentsInChildren<Button>()[0].onClick.RemoveAllListeners();
                levelUpMenu.GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();
                levelUpMenu.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => _tower.GetComponent<Tower>().levelUpCallbacks[firstUp](_tower.GetComponent<Tower>()));
                levelUpMenu.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => _tower.GetComponent<Tower>().levelUpCallbacks[secondUp](_tower.GetComponent<Tower>()));
                levelUpMenu.GetComponentsInChildren<Button>()[0].GetComponent<Image>().sprite = _tower.GetComponent<Tower>().levelUpCallbackNames[_tower.GetComponent<Tower>().levelUpCallbacks[firstUp]];
                levelUpMenu.GetComponentsInChildren<Button>()[1].GetComponent<Image>().sprite = _tower.GetComponent<Tower>().levelUpCallbackNames[_tower.GetComponent<Tower>().levelUpCallbacks[secondUp]];
            }
        }
        else
        {
            destroyButton.SetActive(false);
            foreach (var tower in Player.instance.Towers)//
            {
                Button _button = Instantiate(prefabButton);
                _button.transform.SetParent(buildObject.transform, false);
                _button.GetComponent<Image>().sprite = tower.GetComponent<Tower>().spriteButton;
                _button.onClick.AddListener(() => SetTower(tower));
            }
        }
    }

    private void GenerateUps()
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
        destroyButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void DestroyTower()
    {
        Destroy(_tower);
        destroyButton.SetActive(false);
        menu.gameObject.SetActive(false);
        destroyButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void SetTower(GameObject tower)
    {
        _tower = Instantiate(tower);
        _tower.transform.position = transform.position;
        if (Player.instance.resources.Subtract(_tower.GetComponent<Tower>().cost))
        {
            destroyButton.SetActive(true);
            buildObject.SetActive(false);
            menu.gameObject.SetActive(false);
            destroyButton.GetComponent<Button>().onClick.RemoveAllListeners();
            Player.instance.create.Play();
        }
        else
        {
            Destroy(_tower);
        }
    }
}
