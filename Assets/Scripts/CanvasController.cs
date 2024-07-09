using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] private Button prefabButton;
    [SerializeField] private GameObject buildObject;
    [SerializeField] private GameObject marker;
    private bool setTower = false;
    private GameObject _tower;
    [SerializeField] private GameObject destroyButton;
    private void Awake()
    {
        destroyButton.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!setTower)
        {
            buildObject.SetActive(true);
        }
        else
        {
            buildObject.SetActive(false);
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
