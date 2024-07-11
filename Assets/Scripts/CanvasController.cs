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
    private GameObject _tower;
    private Button[] levelUpBtns;

    [SerializeField] private GameObject destroyButton;
    private void Awake()
    {
        destroyButton.SetActive(false);
        canvas.gameObject.SetActive(false);
        levelUpMenu.SetActive(false);
    }

    private void Update()
    {
        buildObject.SetActive(!setTower);
        if(_tower)
        {
            levelUpMenu.SetActive(setTower && _tower.GetComponent<Tower>().levelUpsRemain > 0);
            if (_tower.GetComponent<Tower>().updateLvlUp)
            {
                GenerateUps();
                _tower.GetComponent<Tower>().updateLvlUp = false;
            }
        }
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
        while(true)
        {
            int _try = Random.Range(0, _tower.GetComponent<Tower>().levelUpCallbacks.Count);
            if (_try == first) continue;
            else break;
        }
        levelUpBtns = levelUpMenu.GetComponentsInChildren<Button>();
        levelUpBtns[0].onClick.AddListener(() => _tower.GetComponent<Tower>().levelUpCallbacks[first](_tower.GetComponent<Tower>(), Random.Range(0, 4)));
        levelUpBtns[1].onClick.AddListener(() => _tower.GetComponent<Tower>().levelUpCallbacks[second](_tower.GetComponent<Tower>(), Random.Range(0, 4)));
        levelUpBtns[0].GetComponentInChildren<Text>().text = _tower.GetComponent<Tower>().levelUpCallbackNames[first];
        levelUpBtns[1].GetComponentInChildren<Text>().text = _tower.GetComponent<Tower>().levelUpCallbackNames[second];
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
