using UnityEngine;

// Дозволяє створювати нові конфігурації рівнів через меню Unity 
// (Project -> Create -> Configs -> Level Configuration).
[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Configs/Level Configuration")]
public class LevelConfig : ScriptableObject
{
    [Header("Налаштування руху")]
    // Швидкість руху всіх перешкод та ворогів на цьому рівні
    public float globalMoveSpeed = 5f;
    
    // Координата X, за якою об'єкти виходять за екран і вимикаються
    public float deadZone = -28f;

    [Header("Налаштування спавну")]
    // Інтервал часу (в секундах) між появою нових перешкод/ворогів
    public float spawnRate = 2f;
    
    // Максимальний розкид позиції спавну по вертикалі (вісь Y)
    public float heightOffset = 7f;
    
    // Затримка перед початком генерації об'єктів на старті рівня
    public float startDelay = 2f;

    [Header("Складність")]
    // Тривалість рівня в секундах (скільки часу гравець має протриматися до перемоги)
    public float levelDuration = 60f;
    
    // Шанс появи подвійного ворога одночасно (від 0% до 100%).
    // Атрибут [Range] створює зручний повзунок (слайдер) в інспекторі Unity.
    [Range(0, 100)]
    public float doubleSpawnChance = 75f;
}