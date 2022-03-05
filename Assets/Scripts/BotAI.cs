using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class BotAI : MonoBehaviour
{
    [Header("PATROLLING")]
    public float maxPatrolDist = 5f;
    public float timeToPatrol = 2f;

    [Header("CHASING")]
    public float chaseRange = 5f;
    public LayerMask targetLayer;
    public Transform target;
    public Transform detectPlayer;
    public float visionAngle = 90f;
    public LayerMask obstacleLayer;

    public float attackRange = 3f;
    public float fireRate = 1f;

    [Header("TAKE COVER")]
    public Transform coverSpot;
    public Transform coverTarget;
    public float coverFactor = .5f;
    public float findCoverRadius = 15f;
    public float findPlayerRadius = 1f;
    public float timeCovering = 2f;

    public NavMeshAgent agent;
    public GameObject rata;
    public Animator anim_rata;

    public bool muerto = false;

    public int count = 1;

    public List<Transform> Waypoints = new List<Transform>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim_rata = rata.GetComponent<Animator>();
    }

    public void FindRandomPoint()
    {
        Vector3 _direction = new Vector3(Random.Range(-1, 1), 0f, Random.Range(-1, 1)) * maxPatrolDist;
        if (NavMesh.SamplePosition(_direction, out NavMeshHit _hit, 1f, 1))
        {
            agent.SetDestination(_hit.position);
        }
    }

    public void FindWaypoint()
    {
        int nextWaypoint = Random.Range(0, Waypoints.Count);
        Vector3 _direction = new Vector3(Waypoints[nextWaypoint].transform.position.x, Waypoints[nextWaypoint].transform.position.y, Waypoints[nextWaypoint].transform.position.z);
        Debug.Log(_direction);
        if (NavMesh.SamplePosition(_direction, out NavMeshHit _hit, 1f, 1))
        {
            agent.SetDestination(_hit.position);
        }
    }

    public Vector3 GetDestinationDirection()
    {
        return agent.destination - transform.position;
    }

    public Vector3 GetTargetDirection()
    {
        Vector3 _targetDir = target.position - transform.position;
        _targetDir.y = 0;
        return _targetDir;
    }

    public bool HasTarget()
    {
        Collider[] _targets = Physics.OverlapSphere(transform.position, chaseRange, targetLayer);
        if (_targets.Length > 0)
        {
            Vector3 _dir = _targets[0].transform.position - transform.position;
            if (Vector3.Angle(transform.forward, _dir) < visionAngle / 2f)
            {
                if (Physics.Raycast(transform.position + Vector3.up * .8f, _dir.normalized, _dir.magnitude, obstacleLayer) == false)
                {
                    target = _targets[0].transform;
                }
            }
        }
        else
        {
            target = null;
        }
        return target != null;
    }

    public bool DetectPlayer()
    {
        Collider[] _dPlayer = Physics.OverlapSphere(transform.position, findPlayerRadius, targetLayer);
        if (_dPlayer.Length > 0)
        {
            Debug.Log("hay player");
            //agent.enabled = false;
            Vector3 _dir = _dPlayer[0].transform.position - transform.position;
            if (Vector3.Angle(transform.forward, _dir) < visionAngle / 2f)
            {
                //Debug.Log("campo vision");
                if (Physics.Linecast(transform.position + Vector3.up * .8f, _dPlayer[0].transform.position, out RaycastHit hit, obstacleLayer) == false)
                {
                    Debug.Log("no obstaculos");
                    detectPlayer = _dPlayer[0].transform;
                }
                else
                {
                    Debug.Log(hit.collider.name);
                }
            }
        }
        else
        {
            detectPlayer = null;
        }
        return detectPlayer != null;
    }

    public void FindCover()
    {
        Collider[] _covers = Physics.OverlapSphere(transform.position, findCoverRadius, obstacleLayer);
        //Ordenamos los elementos del array en funcion de la distancia al bot para que siempre busque primero los puntos mas cercanos
        _covers = _covers.OrderBy(_cover => (_cover.transform.position - transform.position).sqrMagnitude).ToArray();
        for (int i = 0; i < _covers.Length; i++)
        {
            //Con los bounds accedemos a la altura del objeto. Si es menor que la altura del agente, no es un punto de cobertura valido
            if (_covers[i].bounds.size.y < agent.height)
            {
                continue;
            }
            coverSpot = _covers[i].transform;
            //Buscamos una posicion valida del NavMesh que sea cercana al objeto de cobertura
            if (NavMesh.SamplePosition(coverSpot.position, out NavMeshHit _hit, 2f, agent.areaMask))
            {
                //Debug.Log("Sampled normal: " + _hit.normal);
                //Como necesitamos una normal para poder comprobar si es un punto valido, buscamos la linea mas cercana al punto calculado
                if (NavMesh.FindClosestEdge(_hit.position, out _hit, agent.areaMask))
                {
                    //Debug.Log("Sampled  EDGE normal: " + _hit.normal);
                    Vector3 _targetDir = coverTarget.position - _hit.position;
                    //Con el producto escalar entre dos direcciones obtenemos un numero entre -1 y 1. Cuanto mas parecido a 1, mas "en frente" esta una direccion de otra. Cuanto mas parecido a -1, es mas opuesta.
                    float _dot = Vector3.Dot(_targetDir.normalized, _hit.normal);
                    //Debug.Log("Sampled DOT from edge: " + _dot);
                    //Si el producto escalar es menor a la "sensibilidad" de buscar cobertura, lo hace su nuevo destino
                    if (_dot < coverFactor)
                    {
                        agent.SetDestination(_hit.position);
                        break;
                    }
                    //Si no es un punto valido, tiene que buscar otro
                    else
                    {
                        //Buscamos una nueva posicion desplazandola en la direccion del punto de cobertura con respecto al player para encontrar la superficie opuesta
                        if (NavMesh.SamplePosition(coverSpot.position - _targetDir.normalized, out _hit, 1.5f, agent.areaMask))
                        {
                            //Buscamos el borde mas cercano a esa nueva posicion para encontrar la normal de la superficie
                            if (NavMesh.FindClosestEdge(_hit.position, out _hit, agent.areaMask))
                            {
                                _dot = Vector3.Dot(_targetDir.normalized, _hit.normal);
                                //Debug.Log("New sampled DOT from edge: " + _dot);
                                if (_dot < coverFactor)
                                {
                                    agent.SetDestination(_hit.position);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public IEnumerator RataCogida()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(5f);
        agent.enabled = true;
    }

    private void OnDrawGizmos()
    {
        if (agent != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, agent.destination);
            Gizmos.DrawWireSphere(agent.destination, .5f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, findCoverRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, findPlayerRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Waypoint")
        {
            count++;
            Debug.Log("La rata llegó al waypoint");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (gameObject.tag == "Waypoint")
        {
            count++;
            Debug.Log("La rata sigue en el waypoint");
        }
    }
}
