using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _easterObject;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _easterObject.transform.localScale = Vector3.zero;
            _easterObject.SetActive(true);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(_easterObject.transform.DOScale(new Vector3(11, 11, 11), 0.3f)).Join(_easterObject.transform.DOShakeRotation(0.5f, 50, 20, 90));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(_easterObject.transform.DOScale(new Vector3(0, 0, 0), 0.2f)).Join(_easterObject.transform.DOShakeRotation(0.5f, 50, 20, 90))
                .OnComplete(() => _easterObject.SetActive(false));
        }
    }
}
