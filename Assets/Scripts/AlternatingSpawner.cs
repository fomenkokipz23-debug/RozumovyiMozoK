using UnityEngine;

public class AlternatingSpawner : MonoBehaviour
{
    // Теги для пошуку відповідних об'єктів у пулі (Орел та Пташка)
    public string eagleTag = "Eagle";
    public string birdTag = "Bird";
    
    // Посилання на файл конфігурації рівня (ScriptableObject)
    private LevelConfig config;
    
    // Таймер для відліку часу між спавнами
    private float timer = 0f;
    
    // Прапорець для чергування ворогів (якщо true — черга Орла, якщо false — Пташки)
    private bool isEagleTurn = true;

    // Метод Start викликається один раз на старті гри
    void Start()
    {
        // Шукаємо на сцені об'єкт менеджера гри з тегом "Logic"
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
        {
            // Отримуємо з нього посилання на конфігурацію рівня
            config = logicObj.GetComponent<LogicScript>().config;
        }
    }

    // Метод Update викликається кожного кадру
    void Update()
    {
        // Якщо конфігурацію не знайдено, виходимо, щоб уникнути помилок
        if (config == null) return;

        // Організація таймера: додаємо реальний час, що минув з минулого кадру
        if (timer < config.spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            // Коли таймер досяг заданого інтервалу (spawnRate), викликаємо логіку спавну
            SpawnLogic();
            // Скидаємо таймер назад на 0
            timer = 0f;
        }
    }

    // Метод, який вирішує: спавнити одного ворога чи одразу двох (подвійний спавн)
    void SpawnLogic()
    {
        // "Кидаємо кубик" (генеруємо випадкове число від 0 до 100)
        float roll = Random.Range(0f, 100f);

        // Якщо випало число, менше або рівне за шанс подвійного спавну з налаштувань
        if (roll <= config.doubleSpawnChance)
        {
            // Спавнимо двох ворогів одночасно: один гарантовано зверху, інший — знизу
            SpawnSingleEnemy(Random.Range(transform.position.y + 1f, transform.position.y + config.heightOffset));
            SpawnSingleEnemy(Random.Range(transform.position.y - config.heightOffset, transform.position.y - 1f));
        }
        else
        {
            // Інакше спавнимо лише одного ворога на випадковій висоті по всій доступній зоні Y
            SpawnSingleEnemy(Random.Range(transform.position.y - config.heightOffset, transform.position.y + config.heightOffset));
        }
    }

    // Метод, який безпосередньо дістає ворога з Object Pool
    void SpawnSingleEnemy(float yPosition)
    {
        // Обираємо тег залежно від того, чия зараз черга (Орла чи Пташки)
        string tagToSpawn = isEagleTurn ? eagleTag : birdTag;
        
        // Формуємо вектор позиції для появи ворога
        Vector3 spawnPosition = new Vector3(transform.position.x, yPosition, 0);

        // Перевіряємо, чи існує на сцені менеджер пулу об'єктів
        if (ObjectPooler.Instance != null)
        {
            // Замість Instantiate викликаємо оптимізований метод спавну з пулу за тегом
            ObjectPooler.Instance.SpawnFromPool(tagToSpawn, spawnPosition, transform.rotation);
        }

        // Змінюємо прапорець на протилежний (!isEagleTurn), щоб наступного разу з'явився інший тип ворога
        isEagleTurn = !isEagleTurn; 
    }
}