using UnityEngine;

public static class ProducerExtensions
{
    public static ProducerSource FindSource(this IShootable producer)
    {
        foreach (var element in producer.Producer.GetComponentsInChildren<Transform>()) //���������
            if (element.tag == "Projectile")
                return new ProducerSource(element);
        return ProducerSource.Default;
    }
}
