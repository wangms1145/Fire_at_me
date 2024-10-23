using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptBoy.PolygonCollider2DSolverDemo
{
    public class Spawner : MonoBehaviour
    {
        public GameObject prefab;
        public int count;
        public float radius;
        public float waitTimePerSpawn;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(waitTimePerSpawn);

            Transform parent = transform;
            Quaternion q = Quaternion.identity;
            Vector2 position = parent.position;

            for (int i = 0; i < count; i++)
            {
                yield return waitForSeconds;
                Instantiate(prefab, position + Random.insideUnitCircle * radius, q, parent);
            }
        }
    }
}
