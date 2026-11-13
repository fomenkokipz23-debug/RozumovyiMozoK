using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeMoveScript : MonoBehaviour
{
    // Швидкість руху перешкоди ліворуч (за замовчуванням 5)
    public float moveSpeed = 5;
    
    // Координата Х, після перетину якої об'єкт вважається за межами екрану
    public float deadZone = -28;    
    
    // Посилання на ScriptableObject з конфігурацією налаштувань рівня
    private LevelConfig config;

    // Метод Awake викликається один раз при ініціалізації об'єкта (раніше за Start)
    void Awake()
    {
        // Шукаємо об'єкт з налаштуваннями конфігурації
        FindConfig();
    }

    // Метод Update викликається кожного кадру (використовується для логіки руху)
    void Update()
    {
        // Переміщуємо об'єкт ліворуч (Vector3.left) з урахуванням швидкості та реального часу (Time.deltaTime)
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

        // Перевіряємо, чи вийшов об'єкт за ліву межу екрану (в "мертву зону")
        if (transform.position.x < deadZone)
        {
            // Оскільки в грі використовується Object Pool, ми не знищуємо об'єкт (Destroy),
            // а просто деактивуємо його (SetActive(false)), ховаючи назад у пул.
            gameObject.SetActive(false);
        }
    }

    // Метод OnEnable викликається щоразу, коли об'єкт стає активним (дістається з пулу)
    private void OnEnable()
    {
        // Якщо посилання на конфігурацію ще немає — шукаємо її
        if (config == null) 
        {
            FindConfig();
        }

        // Якщо конфігурацію успішно знайдено, оновлюємо параметри швидкості та мертвої зони під цей рівень
        if (config != null)
        {
            moveSpeed = config.globalMoveSpeed;
            deadZone = config.deadZone;
        }

        // Захист від критичної помилки: якщо швидкість випадково стала <= 0, повертаємо базові 5
        if (moveSpeed <= 0) 
        {
            moveSpeed = 5f; 
        }
    }

    // Власний метод для пошуку об'єкта логіки та витягування з нього конфігурації рівня
    private void FindConfig()
    {
        // Шукаємо на сцені менеджер гри за унікальним тегом "Logic"
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
        {
            // Беремо головний скрипт логіки LogicScript з цього об'єкта
            LogicScript logicScript = logicObj.GetComponent<LogicScript>();
            if (logicScript != null)
            {
                // Передаємо параметри з ScriptableObject конфігурації в це посилання
                config = logicScript.config;
            }
        }
    }
}