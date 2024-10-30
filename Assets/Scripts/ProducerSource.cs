using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}

public record ProducerSource(Transform Transform)
{
    public static ProducerSource Default { get; } = new ProducerSource(null as Transform);
}