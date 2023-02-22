using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawner/Wave")]
public class EnemyWave : ScriptableObject
{
    public EnemyScript type;
    public float spawnRate = 1;
    public float totalCount = 16;
    public float maxCount = 3;
    public float spawnRandomness = 0.3f;

    float currentCount = 0;
    float totalCurrentCount = 0;

    public event System.Action OnFinish;

    public IEnumerator Spawn(Bounds bounds, Transform parent) {
        currentCount = 0;
        totalCurrentCount = 0;
        yield return new WaitForSeconds(Random.Range(0, 1f / spawnRate));
        while(totalCurrentCount < totalCount) {
            if(currentCount < maxCount){
                EnemyScript enemy = Instantiate(type, parent.position + RandomPointInBounds(bounds), Quaternion.identity);
                enemy.transform.parent = parent;
                currentCount++;
                totalCurrentCount++;
                enemy.OnKilled += () => {currentCount--;};
                yield return new WaitForSeconds(1f / spawnRate + Random.Range(0, 2 * spawnRandomness) - spawnRandomness);
            }else yield return new WaitForSeconds(0.2f);
        }

        while(currentCount > 0){
            yield return new WaitForSeconds(0.2f);
        }

        OnFinish?.Invoke();
        yield return null;
    }

    Vector3 RandomPointInBounds(Bounds b){
        return new Vector3(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y), Random.Range(b.min.z, b.max.z));
    }
}
