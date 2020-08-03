using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MetaGoo
{
    public class GooController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _ballPrefab;
    
        [SerializeField]
        private Transform _centerOfGravity;

        [SerializeField]
        [Range(0f, 100f)]
        private float _moveSpeed = 18f;

        [SerializeField]
        [Range(0f, 100f)]
        private float _attractForce = 30f;

        [SerializeField]
        private float _flatRedundancy = 3f;
    
        [SerializeField]
        private float _redundancyPerBall = 0.09f;

        [SerializeField]
        private MetaBallDrawer _metaBallDrawerDrawer;
    
        private readonly List<GameObject> _balls = new List<GameObject>();
    
        private readonly List<Rigidbody> _rbs = new List<Rigidbody>();
    
        private readonly List<SphereCollider> _sphereColliders = new List<SphereCollider>();

        private const int StartCount = 7;

        private float _radius;

        private Transform _camera;

        private void Awake()
        {
            _radius = _ballPrefab.GetComponent<SphereCollider>().radius;
            _camera = Camera.main.transform;
        }

        private void Start()
        {
            _centerOfGravity.position = transform.position + new Vector3(0f, _radius, 0f);
            for (int i = 0; i < StartCount; i++)
            {
                SpawnBall(_centerOfGravity.position + (i * _radius * 2f * Vector3.up));
            }
        }

        private void SpawnBall()
        {
            SpawnBall(_centerOfGravity.position);
        }

        private void SpawnBall(Vector3 pos)
        {
            GameObject ball = GameObject.Instantiate(_ballPrefab, pos, Quaternion.identity, transform);
            _balls.Add(ball);
            _rbs.Add(ball.GetComponent<Rigidbody>());
            _sphereColliders.Add(ball.GetComponent<SphereCollider>());
        }

        private void RemoveBall()
        {
            if (_balls.Count < 1)
            {
                return;
            }
        
            int last = _balls.Count - 1;
            var removed = _balls[last];
            _balls.RemoveAt(last);
            _rbs.RemoveAt(last);
            _sphereColliders.RemoveAt(last);
            Object.Destroy(removed);
        }

        private void FixedUpdate()
        {
            UpdateCenter();
            ApplyInput();
            ApplyAttraction();
        }

        private void Update()
        {
            var bounds = GetBounds();
            _metaBallDrawerDrawer.DrawMetaBalls(_sphereColliders, bounds);
        
#if UNITY_EDITOR
            _boundsThisFrame = bounds;
#endif
        }

        private void ApplyAttraction()
        {
        
            Vector3 center = _centerOfGravity.position;
            for (int i = 0; i < _rbs.Count; i++)
            {
                Rigidbody rb = _rbs[i];
                Vector3 toCenter = center - rb.position;
                Vector3 force;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    force = Vector3.up * 500f;
                }
                else
                {
                    force = toCenter.normalized * _attractForce;
                
                }
            
                rb.AddForce(force);
            }
        }

        private void ApplyInput()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 fwd = _camera.forward;
            fwd.y = 0f;
            fwd.Normalize();
            Vector3 right = _camera.right;
            right.y = 0f;
            right.Normalize();
            Vector3 movement = (fwd * moveVertical) + (right * moveHorizontal);
            for (int i = 0; i < _rbs.Count; i++)
            {
                _rbs[i].AddForce(movement * _moveSpeed);
            }
        }

        private void UpdateCenter()
        {
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < _rbs.Count; i++)
            {
                pos += _rbs[i].position;
            }

            pos /= _rbs.Count;
            _centerOfGravity.position = pos;
        }

        private Bounds GetBounds()
        {
            var metaBounds = _sphereColliders[0].bounds;
            for (var i = 1; i < _sphereColliders.Count; i++)
            {
                var bounds = _sphereColliders[i].bounds;
                metaBounds.Encapsulate(bounds.min);
                metaBounds.Encapsulate(bounds.max);
            }
        
            metaBounds.Expand(_flatRedundancy + (_sphereColliders.Count * _redundancyPerBall));
            return metaBounds;
        }
    
#if UNITY_EDITOR
        private Bounds _boundsThisFrame;

        public void SpawnBallExternal()
        {
            SpawnBall();
        }

        public void RemoveBallExternal()
        {
            RemoveBall();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_boundsThisFrame.center, _boundsThisFrame.size);
        }
#endif
    }
}
