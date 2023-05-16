using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity2DComponent : MonoBehaviour
{
    public Entity2D ParentEntity => _parentEntity;
    private Entity2D _parentEntity;

    public virtual void SetParentEntity(Entity2D parentEntity)
    {
        _parentEntity = parentEntity;
    }
}
