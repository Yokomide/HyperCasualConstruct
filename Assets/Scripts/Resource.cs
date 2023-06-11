using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Player Resources", menuName = "Player/Resources/Resource")]
public class Resource : ScriptableObject//, IStorage
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private GameObject _object;

    [SerializeField] private string _name;
    [SerializeField] private ResourceType id;

    // [SerializeField] private int _resourceAmount;

    //  [SerializeField] private bool _canBeLessZero;

    //   public int ResourceAmount
    //  {
    //     get { return _resourceAmount; }
    //    }

    /* public void AddValue(int value)
     {
         _resourceAmount += value;
     }

     public void RemoveValue(int value)
     {
         _resourceAmount -= value;

         if (!_canBeLessZero)
             _resourceAmount = 0;
     }

     public void ResetValue()
     {
         _resourceAmount = 0;
     }
    */
}