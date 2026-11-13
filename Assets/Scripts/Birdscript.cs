using UnityEngine;

public class Birdscript : MonoBehaviour
{
    // Система частинок для ефекту вибуху чи пір'я під час смерті пташки
    [SerializeField] private ParticleSystem deathParticles;
    
    // Посилання на компонент фізичного тіла 2D для керування гравітацією та швидкістю
    public Rigidbody2D myRigidbody;
    
    // Сила поштовху (стрибка) пташки вгору
    public float flapStrength = 5;
    
    // Посилання на загальний менеджер логіки гри
    public LogicScript logic;
    
    // Логічний прапорець, який вказує, чи живий персонаж зараз
    public bool birdIsAlive = true;

    [Header("Магазин пташок у грі")]
    // Компонент для відображення і зміни картинки (спрайту) пташки
    public SpriteRenderer birdSpriteRenderer; 
    
    // Масив з усіма шаблонами даних пташок, які доступні в магазині
    public BirdData[] allBirds;              

    // Метод Start викликається при запуску об'єкта на сцені
    void Start()
    {
        // Автоматично отримуємо компонент Rigidbody2D, що висить на цьому ж об'єкті
        myRigidbody = GetComponent<Rigidbody2D>();
        
        // Якщо посилання на SpriteRenderer не задано в інспекторі, шукаємо його автоматично
        if (birdSpriteRenderer == null)
        {
            birdSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Завантажуємо з пам'яті пристрою ID обраної пташки. Якщо нічого не збережено — беремо "default"
        string selectedBirdId = PlayerPrefs.GetString("SelectedBird", "default");

        // Логіка зміни зовнішнього вигляду (скіна) пташки під час старту гри:
        if (allBirds != null && allBirds.Length > 0)
        {
            for (int i = 0; i < allBirds.Length; i++)
            {
                // Якщо ID пташки в базі даних збігається із тим, що вибрав гравець у магазині
                if (allBirds[i] != null && allBirds[i].birdId == selectedBirdId)
                {
                    // Змінюємо поточний спрайт на картинку обраної пташки
                    birdSpriteRenderer.sprite = allBirds[i].birdSprite;
                    break; // Перериваємо цикл, оскільки потрібну пташку знайдено
                }
            }
        }
        
        // Шукаємо на сцені об'єкт логіки гри за його тегом
        GameObject logicObject = GameObject.FindGameObjectWithTag("Logic");
        if (logicObject != null)
        {
            // Витягуємо з нього скрипт керування грою
            logic = logicObject.GetComponent<LogicScript>();
        }
        else
        {
            // Виводимо червону помилку в консоль, якщо забули поставити об'єкт логіки на сцену
            Debug.LogError("ОБ'ЄКТ З ТЕГОМ 'Logic' НЕ ЗНАЙДЕНО!");
        }
    }

    // Метод Update викликається кожного кадру
    void Update()
    {
        // Перевірка натискання клавіші Пропуск або лівої кнопки миші (чи тапа по екрану), якщо пташка жива
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && birdIsAlive)
        {
            // Надаємо фізичному тілу імпульс вгору (Vector2.up) з нашою силою flapStrength
            myRigidbody.velocity = Vector2.up * flapStrength;
        }
    }

    // Метод обробки перетину тригерних (примарних) колайдерів (наприклад, зон з ворогами)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Якщо об'єкт, у який ми влетіли, має тег "Enemy" і пташка ще жива
        if (collision.gameObject.CompareTag("Enemy") && birdIsAlive)
        {
            Die(); // Викликаємо метод смерті
        }
    }

    // Метод обробки фізичного зіткнення з твердими колайдерами (земля, стеля або труби)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Перевіряємо зіткнення з ворогом АБО з об'єктом, який знаходиться на фізичному шарі "Obstacle"
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) && birdIsAlive)
        {
            Die(); // Викликаємо метод смерті
        }
    }

    // Приватний метод, який зупиняє ігровий процес для цього персонажа
    private void Die()
    {
        Debug.Log("Die() викликано!");
        birdIsAlive = false; // Змінюємо стан на "мертвий"

        // Повідомляємо менеджер логіки про кінець гри
        if (logic != null)
        {
            logic.gameOver(); // Запускаємо екран Game Over та зупиняємо час
        }
        else
        {
            Debug.LogError("Скрипт Logic не призначено!");
        }

        // Логіка відтворення ефекту смерті (частинок):
        if (deathParticles != null)
        {
            // Встановлюємо позицію ефекту в точку смерті пташки (трохи ближче до камери по Z)
            deathParticles.transform.position = transform.position + Vector3.forward * -5;
            
            // Відв'язуємо ефект від пташки, щоб він не видалився разом із нею
            deathParticles.transform.parent = null; 
            
            // Налаштовуємо систему частинок так, щоб вона працювала навіть при зупиненому часі гри (Time.timeScale = 0)
            var main = deathParticles.main;
            main.useUnscaledTime = true;
            
            // Запускаємо відтворення візуального ефекту
            deathParticles.Play();
            
            // Повністю знищуємо об'єкт ефекту з пам'яті через 2 секунди після відтворення
            Destroy(deathParticles.gameObject, 2f);
        }

        // Ховаємо пташку з екрану (вимикаємо її рендерер)
        if (birdSpriteRenderer != null)
        {
            birdSpriteRenderer.enabled = false;
        }
        
        // Вимикаємо симуляцію фізики для Rigidbody2D, щоб мертва пташка більше не падала і не взаємодіяла зі світом
        myRigidbody.simulated = false;
    }
}