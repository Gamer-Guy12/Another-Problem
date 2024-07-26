using Unity.Mathematics;

public struct RandomZombieData
{

    public float moveSpeed;
    public float rotationSpeed;

    public float attackStrength;
    public float attackCooldown;
    public float attackRange;

    public float attackSensoryRange;

    public float health;
    public float regenRate;

    public static RandomZombieData CalculateRandomData (ref Random random, float difficulty)
    {

        float shiftedDifficulty;

        if (difficulty > 7) shiftedDifficulty = difficulty + random.NextFloat (-(difficulty / 10), difficulty / 10);
        else shiftedDifficulty = difficulty + random.NextFloat (-0.5f, 0.3f);

        float moveSpeed = 0.5f + (shiftedDifficulty / 50) * 4.5f;
        float rotationSpeed = 2 + (shiftedDifficulty / 50) * 5;

        float attackStrength = 5 + (shiftedDifficulty / 50) * 10;
        float attackCooldown = 1 - (shiftedDifficulty / 50) * 0.9f;
        float attackRange = 3 + (shiftedDifficulty / 50) * 7;

        float attackSensoryRange = 20 + (shiftedDifficulty / 50) * 10;

        float health = 100 + (shiftedDifficulty / 50) * 400;
        float regenRate = 1 + (shiftedDifficulty / 50) * 9;

        return new RandomZombieData
        {
            moveSpeed = moveSpeed,
            rotationSpeed = rotationSpeed,
            attackStrength = attackStrength,
            attackCooldown = attackCooldown,
            attackRange = attackRange,
            attackSensoryRange = attackSensoryRange,
            health = health,
            regenRate = regenRate
        };

    }

}
