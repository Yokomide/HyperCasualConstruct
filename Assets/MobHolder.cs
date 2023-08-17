using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MobHolder : MonoBehaviour
{
    private FormationBase _formation;

    public FormationBase Formation
    {
        get
        {
            if (_formation == null) _formation = GetComponent<FormationBase>();
            return _formation;
        }
        set => _formation = value;
    }

    [SerializeField] private List<Entity> _mobList = new List<Entity>();
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private float _unitSpeed = 2;

    private readonly List<GameObject> _spawnedUnits = new List<GameObject>();
    private List<Vector3> _points = new List<Vector3>();
    private Transform _parent;
    private void OnEnable()
    {
        Formation.OnValueChange += SetFormation;

    }
    private void OnDisable()
    {
        Formation.OnValueChange -= SetFormation;

    }
    private void Awake()
    {
        _parent = new GameObject("Unit Parent").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetFormation();
        }
    }
    public void AddMob(Entity mob)
    {
        _mobList.Add(mob);
        SetFormation();

    }
    private void SetFormation()
    {
        _points = Formation.EvaluatePoints().ToList();

        //  if (_points.Count > _mobList.Count)
        // {
        //     var remainingPoints = _points.Skip(_mobList.Count);
        //     Spawn(remainingPoints);
        // }
        // else if (_points.Count < _mobList.Count)
        //  {
        //       Kill(_mobList.Count - _points.Count);
        //   }

        for (var i = 0; i < _mobList.Count; i++)
        {
            // _mobList[i].transform.position = Vector3.MoveTowards(_mobList[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
            var MobAI = _mobList[i].GetComponent<SimpleAI>();
            MobAI._destinationVector3 = transform.position + _points[i];
            MobAI.Move();
        }
    }
    /*
    private void Spawn(IEnumerable<Vector3> points)
    {
        foreach (var pos in points)
        {
            var unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, _parent);
            _mobList.Add(unit);
        }
    }
    */

    public void Kill(Entity entity)
    {
        _mobList.Remove(entity);
    }



















    /*
    private FormationBase _formation;
    public FormationBase Formation
    {
        get
        {
            if (_formation == null) _formation = GetComponent<FormationBase>();
            return _formation;
        }
        set => _formation = value;
    }

    [SerializeField] private List<GameObject> _mobList = new List<GameObject>();

    public List<GameObject> MobList
    {
        get => _mobList;
        set => _mobList = value;
    }

    public void AddMob(GameObject mob)
    {
        MobList.Add(mob);
        UpdateFormation();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UpdateFormation();
        }
    }
    public void UpdateFormation()
    {
        if (Formation == null) return;
        var i = 0;
        /*
        for (i = 0; i < MobList.Count; i++)
        { 
            var pos = Formation.EvaluatePoints();
            var MobAI = MobList[i].GetComponent<SimpleAI>();
            MobAI._destinationVector3 = transform.position + Formation.EvaluatePoints().ToList()[i];
            MobAI.Move();
        }
        
        foreach (var pos in Formation.EvaluatePoints())
        {
            if (i > MobList.Count-1)
                return;

            var MobAI = MobList[i].GetComponent<SimpleAI>();
            MobAI._destinationVector3 = transform.position + pos;
            MobAI.Move();
            //MobList[i].transform.position = transform.position + pos;
            i++;
        }
        
    }
*/
}
