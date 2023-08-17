using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TweenAnimationContoller _tweenerAnimation;
    [SerializeField] private int _animationIndex;
    private bool _isOpened;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        _menu.SetActive(true);
        _tweenerAnimation.StartAnimation(_menu, _animationIndex);

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

        _tweenerAnimation.RewindAnimation(_animationIndex);
        _isOpened = false;

    }
}
