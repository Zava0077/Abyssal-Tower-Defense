using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LevelUp;

public class CanvasController : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] private Button prefabButton;
    [SerializeField] private GameObject buildObject;
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject levelUpMenu;
    private bool setTower = false;
    public bool showAgro = false;
    private GameObject _tower;
    private Button[] levelUpBtns;
    [SerializeField] private GameObject cooldownRedBar;
    [SerializeField] private GameObject cooldownBar;
    private float barLimit;
    [SerializeField] private GameObject destroyButton;
    [SerializeField] private GameObject agroRadius;
    private void Awake()
    {
        destroyButton.SetActive(false);
        canvas.gameObject.SetActive(false);
        levelUpMenu.SetActive(false);
        barLimit = cooldownRedBar.transform.localScale.x;
    }

    private void Update()
    {
        buildObject.SetActive(!setTower);
        cooldownBar.SetActive(setTower);
        agroRadius.SetActive(setTower && showAgro);
        if (_tower)
        {
            agroRadius.transform.localScale = new Vector3(_tower.GetComponent<Tower>().agroRadius/4, agroRadius.transform.localScale.y, _tower.GetComponent<Tower>().agroRadius/4);
            cooldownRedBar.transform.localScale = new Vector3((_tower.GetComponent<Tower>().time / (1 / _tower.GetComponent<Tower>().attackSpeed)) * barLimit, cooldownRedBar.transform.localScale.y, cooldownRedBar.transform.localScale.z);
            levelUpMenu.SetActive(setTower && _tower.GetComponent<Tower>().levelUpsRemain > 0);
            if (_tower.GetComponent<Tower>().updateLvlUp)
            {
                GenerateUps();
                _tower.GetComponent<Tower>().updateLvlUp = false;
            }
        }
        showAgro = false;
    }

    private void Start()
    {
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
        int first = Random.Range(0, _tower.GetComponent<Tower>().levelUpCallbacks.Count);
        int second = 0;

        while (true)
        {
            second = Random.Range(0, _tower.GetComponent<Tower>().levelUpCallbacks.Count);
            if (second == first || (first > 12 && second > 12)) continue;
            else break;
        }
        levelUpBtns = levelUpMenu.GetComponentsInChildren<Button>();
        levelUpBtns[0].onClick.RemoveAllListeners();
        levelUpBtns[1].onClick.RemoveAllListeners();
        levelUpBtns[0].onClick.AddListener(() => _tower.GetComponent<Tower>().levelUpCallbacks[first](_tower.GetComponent<Tower>()));
        levelUpBtns[1].onClick.AddListener(() => _tower.GetComponent<Tower>().levelUpCallbacks[second](_tower.GetComponent<Tower>()));
        levelUpBtns[0].GetComponent<Image>().sprite = _tower.GetComponent<Tower>().levelUpCallbackNames[_tower.GetComponent<Tower>().levelUpCallbacks[first]];
        levelUpBtns[1].GetComponent<Image>().sprite = _tower.GetComponent<Tower>().levelUpCallbackNames[_tower.GetComponent<Tower>().levelUpCallbacks[second]];
    }
    public void Exit()
    {
        canvas.gameObject.SetActive(false);
    }

    public void DestroyTower()
    {
        Destroy(_tower);
        setTower = false;
        destroyButton.SetActive(false);
    }

    private void SetTower(GameObject tower)
    {
        _tower = Instantiate(tower);
        _tower.transform.position = marker.transform.position;
        if (Camera.main.GetComponent<Player>().resources.Subtract(_tower.GetComponent<Tower>().cost))
        {
            setTower = true;
            destroyButton.SetActive(true);
            Debug.Log(tower);
            buildObject.SetActive(false);
        }
        else
        {
            Destroy(_tower);
        }
    }
}
