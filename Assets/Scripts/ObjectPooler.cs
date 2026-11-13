using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Реалізація патерну Singleton (Синглтон / Одинак). 
    public static ObjectPooler Instance;

    // Атрибут [System.Serializable] дозволяє відображати цей підклас в інспекторі Unity
    // та налаштовувати його як структуру даних.
    [System.Serializable]
    public class Pool {
        public string tag;          // Унікальний тег-ідентифікатор пулу (наприклад, "Eagle", "Bird")
        public GameObject prefab;   // Шаблон (префаб) об'єкта, який ми будемо розмножувати
        public int size;            // Кількість об'єктів, яку треба створити заздалегідь
    }

    // Список конфігурацій пулів, який налаштовується в інспекторі Unity
    public List<Pool> pools; 
    
    // Словник (Dictionary), де ключем є текстовий тег, а значенням — черга (Queue) ігрових об'єктів.
    // Використання Queue (черги) забезпечує роботу за принципом FIFO (First In, First Out).
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Метод Awake викликається найпершим під час завантаження сцени
    void Awake() { 
        // Записуємо посилання на цей конкретний екземпляр у статичну змінну (ініціалізація Singleton)
        Instance = this; 
    }

    // Метод Start викликається перед першим кадром гри
    void Start() {
        // Створюємо новий порожній словник для зберігання наших пулів об'єктів
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Проходимо циклом по кожній конфігурації пулу, яку ми налаштували в інспекторі
        foreach (Pool pool in pools) {
            // Створюємо нову порожню чергу для поточного типу об'єктів
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Цикл для попереднього створення об'єктів у заданій кількості (pool.size)
            for (int i = 0; i < pool.size; i++) {
                // Клонуємо об'єкт із префабу "у повітря"
                GameObject obj = Instantiate(pool.prefab);
                
                // Спочатку всі об'єкти вимкнені, вони не споживають ресурси рендерингу та фізики
                obj.SetActive(false); 
                
                // Додаємо створений об'єкт у кінець нашої черги
                objectPool.Enqueue(obj);
            }
            
            // Додаємо готову чергу в загальний словник, прив'язуючи її до унікального тегу
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Публічний метод для діставання об'єкта з пулу (заміна стандартного Instantiate)
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        // Якщо в словнику немає черги з таким тегом, повертаємо null, щоб уникнути виліту гри
        if (!poolDictionary.ContainsKey(tag)) return null;

        // Захисний механізм: якщо об'єкти в пулі закінчилися, а гра вимагає ще (Count == 0)
        if (poolDictionary[tag].Count == 0)
        {
            // Шукаємо конфігурацію цього пулу за тегом
            Pool currentPool = pools.Find(p => p.tag == tag);
            if (currentPool != null)
            {
                // Попереджаємо розробника в консолі, що початкового розміру пулу забракло
                Debug.LogWarning($"Пул для тегу {tag} закінчився! Створюємо додатковий об'єкт.");
                
                // Динамічно створюємо один новий об'єкт поверх ліміту
                GameObject newObj = Instantiate(currentPool.prefab);
                newObj.SetActive(true); // Вмикаємо його
                newObj.transform.position = position; // Задаємо позицію
                newObj.transform.rotation = rotation; // Задаємо обертання
                return newObj; // Повертаємо його спавнеру
            }
            return null;
        }
      
        // Якщо об'єкти в пулі є: дістаємо перший об'єкт із початку черги за допомогою Dequeue()
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // Активуємо об'єкт (він стає видимим на сцені, на ньому запускаються скрипти і фізика)
        objectToSpawn.SetActive(true); 
        
        // Переміщуємо його в потрібні координати
        objectToSpawn.transform.position = position;
        
        // Повертаємо у потрібний бік
        objectToSpawn.transform.rotation = rotation;

        // Повертаємо цей же об'єкт назад у кінець черги (Enqueue), щоб він міг бути використаний повторно
        poolDictionary[tag].Enqueue(objectToSpawn);
        
        // Повертаємо готовий об'єкт як результат роботи методу
        return objectToSpawn;
    }

    // Публічний метод для ручного повернення об'єкта в пул (заміна стандартного Destroy)
    public void ReturnToPool(string tag, GameObject obj)
    {
        // Деактивуємо об'єкт (ховаємо з екрану, вимикаємо фізику)
        obj.SetActive(false);
        
        // Перевіряємо, чи існує такий пул у словнику, і чи цей об'єкт уже випадково не знаходиться в черзі
        if (poolDictionary.ContainsKey(tag) && !poolDictionary[tag].Contains(obj))
        {
            // Додаємо об'єкт назад у чергу для подальшого перевикористання
            poolDictionary[tag].Enqueue(obj);
        }
    }
}