//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class CanvasControllerFerm : MonoBehaviour
//{
//    [SerializeField] public Canvas canvas;
//    [SerializeField] private Button prefabButton;
//    [SerializeField] private GameObject buildObject;
//    [SerializeField] private GameObject marker;
//    private bool setFerma = false;
//    private GameObject _ferma;
//    [SerializeField] private GameObject destroyButton;
//    private void Awake()
//    {
//        destroyButton.SetActive(false);
//        canvas.gameObject.SetActive(false);
//    }

//    private void Update()
//    {
//        if (!setFerma)
//        {
//            buildObject.SetActive(true);
//        }
//        else
//        {
//            buildObject.SetActive(false);
//        }
//    }

//    private void Start()
//    {
//        foreach (var ferma in Camera.main.GetComponent<Player>().Farms)
//        {
//            Button _button = Instantiate(prefabButton);
//            _button.transform.SetParent(buildObject.transform, false);
//            _button.GetComponent<Image>().sprite = ferma.GetComponent<Farm>().spriteButton;
//            _button.onClick.AddListener(() => SetTower(ferma));
//        }
//    }

//    public void Exit()
//    {
//        canvas.gameObject.SetActive(false);
//    }

//    public void DestroyTower()
//    {
//        Destroy(_ferma);
//        setFerma = false;
//        destroyButton.SetActive(false);
//    }

//    private void SetTower(Entity ferma)
//    {
//        _ferma = Instantiate(ferma);
//        _ferma.transform.position = marker.transform.position;
//        if (Camera.main.GetComponent<Player>().resources.Subtract(_ferma.GetComponent<Farm>().resCost))
//        {
//            setFerma = true;
//            destroyButton.SetActive(true);
//            Debug.Log(ferma);
//            buildObject.SetActive(false);
//        }
//        else
//        {
//            Destroy(_ferma);
//        }
//    }
//}
