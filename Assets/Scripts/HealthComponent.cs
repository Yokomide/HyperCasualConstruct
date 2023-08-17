using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    public int Health
    {

        get
        {
            return _health;
        }

        set
        {
            _health = value;
            if (_health < 0)
            {
                _health = 0;
            }
        }
    }
    private void Start()
    {
        _health = _maxHealth;
    }
    public void IncreaseHealth(int amount)
    {
        _health += amount;
    }


    public void DecreaseHealth(int amount)
    {
        _health -= amount;
    }
}
