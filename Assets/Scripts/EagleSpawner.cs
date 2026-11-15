using UnityEngine;

public class EagleSpawner : MonoBehaviour
{
    [Header("Налаштування пулу об'єктів")]
    [Tooltip("Список тегів усіх об'єктів, які можуть спавнитися")]
    public string[] eagleTags = { "Eagle", "Eagle 1", "Eagle 2" };

    [Header("Параметри спавну")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2.5f;
    
    private float timer = 0f;
    private float nextSpawnTime = 0f;
    
    [Header("Діапазон спавну по вертикалі")]
    [Tooltip("Мінімальна та максимальна координата Y для спавну комет по всьому екрану")]
    public float minY = -24f;
    public float maxY = 24f;

    [Header("Захист від перетинання")]
    [Tooltip("Мінімальна відстань по вертикалі від попередньої комети, щоб вони не накладали одна на одну")]
    public float minVerticalDistance = 1.5f;
    
    private float lastSpawnY = -999f; // Зберігає позицію останньої спавненої комети

    void Start()
    {
        SetRandomSpawnTime();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnRandomEagle();
            timer = 0f;
            SetRandomSpawnTime();
        }
    }

    void SetRandomSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnRandomEagle()
    {
        if (eagleTags == null || eagleTags.Length == 0) return;

        string selectedTag = eagleTags[Random.Range(0, eagleTags.Length)];

        // Генеруємо випадкову позицію Y у заданому діапазоні від minY до maxY,
        // слідкуючи, щоб вона не була занадто близько до попередньої комети (щоб не перетиналися).
        float spawnY = lastSpawnY;
        int safetyCounter = 0; // Захист від нескінченного циклу

        while (Mathf.Abs(spawnY - lastSpawnY) < minVerticalDistance && safetyCounter < 10)
        {
            spawnY = Random.Range(minY, maxY);
            safetyCounter++;
        }

        // Запам'ятовуємо нову позицію
        lastSpawnY = spawnY;

        Vector3 spawnPosition = new Vector3(transform.position.x, spawnY, 0);

        if (ObjectPooler.Instance != null)
        {
            ObjectPooler.Instance.SpawnFromPool(selectedTag, spawnPosition, transform.rotation);
        }
        else
        {
            Debug.LogWarning("ObjectPooler не знайдено на сцені!");
        }
    }
}