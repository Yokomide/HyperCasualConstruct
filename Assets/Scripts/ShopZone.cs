using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenAnimationContoller))]

public class ShopZone : MonoBehaviour
{
    [SerializeField] private List<GameObject> _shopUIList = new List<GameObject>();
    private TweenAnimationContoller _animController;
    private bool _isOpened;
    private void Start()
    {
        _animController = GetComponent<TweenAnimationContoller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        for (int i = 0; i < _shopUIList.Count; i++)
        {
            _shopUIList[i].SetActive(true);
            _animController.StartAnimation(_shopUIList[i], i);

        }
        _isOpened = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        CloseUI();
    }
    public void CloseUI()
    {
        if (_isOpened == false)
            return;

        for (int i = 0; i < _shopUIList.Count; i++)
        {
            _animController.RewindAnimation(i);
        }
        _isOpened = false;

    }
}
