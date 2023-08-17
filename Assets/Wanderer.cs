using DG.Tweening;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _speed;
    private Vector3 _startPoint;
   public Vector3 _endPoint;

    private void Awake()
    {
        _startPoint = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetEndPosition();
        StartWander();
    }

    private void StartWander()
    {
        transform.DOMove(_endPoint, Vector3.Distance(transform.position, _endPoint) / _speed).SetEase(Ease.Linear);
    }
    private void SetEndPosition()
    {
        _endPoint = _startPoint + new Vector3(Random.Range(-_radius, _radius), 0, Random.Range(-_radius, _radius));
    }
    void FixedUpdate()
    {
        if (_endPoint == null)
            return;

        if (Vector3.Distance(transform.position, _endPoint) <= 0.1f)
        {
            SetEndPosition();
            StartWander();
        }
    }
}
