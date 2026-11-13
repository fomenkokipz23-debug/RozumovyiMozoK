using UnityEngine;

public class EagleSpawner : MonoBehaviour
{
    [Header("Налаштування пулу об'єктів")]
    // Унікальний тег (ідентифікатор), за яким спавнер буде просити об'єкт Орла у нашого ObjectPooler
    public string eagleTag = "Eagle 1"; 

    [Header("Параметри спавну")]
    // Частота появи ворогів (кожні 3 секунди з'являється новий Орел)
    public float spawnRate = 3; 
    
    // Внутрішній таймер для відліку часу між спавнами
    private float timer = 0;
    
    // Максимальне відхилення позиції появи Орла по вертикалі відносно спавнера
    public float heightOffset = 3; 

    // Метод Start викликається один раз на самому початку гри
    void Start()
    {
        // Ініціалізуємо таймер початковим значенням 0
        timer = 0;
    }

    // Метод Update викликається кожного кадру гри
    void Update()
    {
        // Поки таймер менший за встановлену частоту спавну (spawnRate)...
        if (timer < spawnRate)
        {
            // ...додаємо до таймера реальний час, який пройшов з моменту попереднього кадру
            timer += Time.deltaTime;
        }
        else
        {
            // Як тільки час вийшов — викликаємо метод створення Орла
            SpawnEagle();
            // Скидаємо таймер на 0 для початку нового відліку
            timer = 0; 
        }
    }

    // Власний метод для генерації та позиціонування Орла на сцені
    void SpawnEagle()
    {
        // Розраховуємо граничні точки для випадкової появи по осі Y
        float lowestPoint = transform.position.y - heightOffset; // Найнижча точка
        float highestPoint = transform.position.y + heightOffset; // Найвища точка
        
        // Формуємо вектор фінальної позиції спавну. 
        // Позиція Y обирається випадково (Random.Range) між мінімумом та максимумом.
        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0);

        // Перевіряємо, чи існує на сцені глобальний менеджер пулу об'єктів (Синглтон)
        if (ObjectPooler.Instance != null)
        {
            // Замість ресурсомісткого створення через Instantiate, дістаємо готовий об'єкт Орла
            // з пулу об'єктів за його тегом, передаючи йому розраховану позицію та обертання.
            ObjectPooler.Instance.SpawnFromPool(eagleTag, spawnPosition, transform.rotation);
        }
        else
        {
            // Захисний механізм: якщо пулу об'єктів на сцені немає, виводимо попередження в консоль.
            // (У поточному коді логіка Instantiate тут опущена для суворого контролю пулу).
            Debug.LogWarning("ObjectPooler не знайдено на сцені! Спавню через Instantiate.");
        }
    }
}