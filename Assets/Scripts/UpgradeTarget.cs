using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(TweenAnimationContoller))]
[Serializable]
public class UpgradeTarget : MonoBehaviour
{
    [SerializeField] bool _shouldDisappear;
    [SerializeField] private Spender _resourceSpender;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private List<GameObject> UpgradedObjects = new List<GameObject>();
    private TweenAnimationContoller _tweenAnimationContoller;
    // [SerializeField] private List<Mesh> meshes = new List<Mesh> ();
    private int updatesAmount;
    private bool _appearAnimationActive = true;
    [SerializeField] private int currentUpdateNumber;

    private GameObject _eventActivatorHolder;
    private IEventActivator _eventActivator;

    //public List<ResourceStorage> resourceRequirments = new List<ResourceStorage> ();
    public List<RequiredResourcesData> resourceRequirments = new List<RequiredResourcesData>();


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
        _tweenAnimationContoller = GetComponent<TweenAnimationContoller>();

        if (_resourceSpender.TryGetComponent(out IEventActivator eventActivator))
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
        if (currentUpdateNumber >= 0)
        {
            _resourceSpender.UpdateRequirments(resourceRequirments[currentUpdateNumber]);
            UpgradedObjects[currentUpdateNumber].SetActive(true);
        }
        else
        {
            _resourceSpender.UpdateRequirments(resourceRequirments[0]);
        }

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
        if(currentUpdateNumber>= UpgradedObjects.Count)
        {
            _resourceSpender.ClearDictionaries();
            _resourceSpender.LockSpend();
            return;
        }
        // _resourceSpender.UnlockSpend();
        _appearAnimationActive = true;
        // _eventActivatorHolder.GetComponent<ResourceSpender>().UnlockSpend();
    }
    private void SwitchUpgradeLevel()
    {
        currentUpdateNumber++;
        if (currentUpdateNumber < resourceRequirments.Count)
            _resourceSpender.UpdateRequirments(resourceRequirments[currentUpdateNumber]);
        else
        {
            //_resourceSpender._requiredResources = null;
            _resourceSpender.ClearDictionaries();
            _resourceSpender.LockSpend();
        }

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