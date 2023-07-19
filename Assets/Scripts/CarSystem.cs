using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarSystem : MonoBehaviour
{
    public List<GameObject> cars = new List<GameObject>();

    public List<Orders> ordersContainer = new List<Orders>();

    [SerializeField] private Spender _resourceSpender;

    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _orderPoint;
    [SerializeField] private Transform _endPoint;

    [SerializeField] private float _duration;

    [SerializeField] private int _difficulty;

    private Vector3 _startScale;
    private GameObject _currentCar;

    [Serializable]
    public class Orders
    {
        public List<RequiredResourcesData> orders = new List<RequiredResourcesData>();
    }

    private void OnEnable()
    {
        _resourceSpender.OnEventActivate += CompleteOrder;
    }
    private void OnDisable()
    {
        _resourceSpender.OnEventActivate -= CompleteOrder;
    }
    private void Start()
    {
        LaunchCar();
    }
    public void LaunchCar()
    {
        var car = cars[Random.Range(0, cars.Count - 1)];
        car.SetActive(true);
        _currentCar = car;
        _startScale = _currentCar.transform.localScale;
        MoveToOrderPosition();
    }

    public void MoveToOrderPosition()
    {
        _currentCar.transform.DOMove(_orderPoint.position, _duration).OnComplete(() => MakeAnOrder());

    }

    public void MoveToEndPosition()
    {
        _currentCar.transform.DOMove(_endPoint.position, _duration).OnComplete(() => MoveToStartPosition());

    }

    public void MoveToStartPosition()
    {
        _currentCar.transform.position = _startPoint.position;
        _currentCar.SetActive(false);
        _currentCar = null;
        LaunchCar();

    }

    public void MakeAnOrder()
    {
        int listDifficulty = Random.Range(0, _difficulty+1);
        Debug.Log(listDifficulty);
        _resourceSpender.resourceTarget = _currentCar.transform;
        _resourceSpender.startResourceTargetScale = _startScale;
        _resourceSpender.UpdateRequirments(ordersContainer[listDifficulty].orders[Random.Range(0, ordersContainer[listDifficulty].orders.Count)]);
    }

    public void CompleteOrder()
    {
       // Sequence complete = DOTween.Sequence();
       // var startScale = _currentCar.transform.localScale;
       // complete.Append(_currentCar.transform.DOScale(_currentCar.transform.localScale * 1.5f, 0.2f))
       //     .Append(_currentCar.transform.DOScale(startScale, 0.2f))
        //    .OnComplete(() =>
        //    {
                MoveToEndPosition();
                _resourceSpender.ClearDictionaries();
         //   } 
         //   );
    }
}
