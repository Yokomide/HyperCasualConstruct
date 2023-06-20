using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(TweenAnimationContoller))]
[Serializable]
public class UpgradeTarget : MonoBehaviour
{
    [SerializeField] bool _shouldDisappear;
    [SerializeField] private GameObject _eventActivatorHolder;
    [SerializeField] private ResourceSpender _resourceSpender;
    [SerializeField] private TweenAnimationContoller _tweenAnimationContoller;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private List<GameObject> UpgradedObjects = new List<GameObject>();

   // [SerializeField] private List<Mesh> meshes = new List<Mesh> ();
    private int updatesAmount;
    private bool _appearAnimationActive = true;
    [SerializeField] private int currentUpdateNumber;
    private IEventActivator _eventActivator;

    public List<ResourceStorage> resourceRequirments = new List<ResourceStorage> ();

    private void OnEnable()
    {
        _eventActivator.OnEventActivate += Upgrade;
        _tweenAnimationContoller.OnAnimationEnd += AppearAnimation;
    }
    private void OnDisable()
    {
        _eventActivator.OnEventActivate -= Upgrade;
        _tweenAnimationContoller.OnAnimationEnd -= AppearAnimation;
    }
    private void Awake()
    {
        if (_eventActivatorHolder.TryGetComponent(out IEventActivator eventActivator))
        {
            _eventActivator = eventActivator;
        }
        else
        {
            Debug.LogError("GameObject have no Script which realize EventActivator Interface");
        }
    }
    private void Start()
    {
        _resourceSpender.UpdateRequirments(resourceRequirments[currentUpdateNumber]);
            UpgradedObjects[currentUpdateNumber].SetActive(true);

    }
    private void Upgrade()
    {
            SwitchUpgradeLevel();
        if (currentUpdateNumber > 0 && _shouldDisappear)
        {
            DisappearAnimation();
            Debug.Log("Disappear");
            _appearAnimationActive = false;
        }
        AppearAnimation();
        _resourceSpender.ResetResourceAmount();
        _resourceSpender.UnlockSpend();
        if (currentUpdateNumber >= UpgradedObjects.Count)
            return;
        _appearAnimationActive = true;
        // _eventActivatorHolder.GetComponent<ResourceSpender>().UnlockSpend();
    }
    private void SwitchUpgradeLevel()
    {
        currentUpdateNumber++;
        if (currentUpdateNumber < resourceRequirments.Count)
            _resourceSpender.UpdateRequirments(resourceRequirments[currentUpdateNumber]);
        else
            _resourceSpender._resourceRequirements = null;

        //_resourceSpender._resourceRequired = UpdateParameters[currentUpdateNumber].ResourceAmount[0];

    }
    
    private void AppearAnimation()
    {
        if (_appearAnimationActive)
        {
            Debug.Log("Appear");
            UpgradedObjects[currentUpdateNumber].SetActive(true);
            _tweenAnimationContoller.StartAnimation(UpgradedObjects[currentUpdateNumber], currentUpdateNumber);
            _appearAnimationActive = false;
        }
    }
    private void DisappearAnimation()
    {
        _tweenAnimationContoller.StartAnimation(UpgradedObjects[currentUpdateNumber - 1], AnimationType.ScaleToZero, true, currentUpdateNumber-1);
    }
}