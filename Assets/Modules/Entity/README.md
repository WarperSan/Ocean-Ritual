# Entity Module
This module covers the entity system, how to use it, how to create new entities, how to create new projectiles and what each component does.

# Entities
In order for an object to use this system, you need to make it an entity. For that, there are 3 ways:
1. Plain Entity
2. Entity Behaviour
3. Projectile

## Plain Entity
You might want to create an entity, but none of the existing presets are suited for it. You can create your own preset by inheriting the class `Entity`. With this, the system will be able to damage and heal your entity. You won't have to manage it's health manually.

## EntityBehaviour
You might want to create an entity, but still want to have a behaviour tree. By using `EntityBehaviour`, the entity will automatically fetch its tree. The tree will be updated each frame. If you want to manually update it, simply override the call of `MonoBehaviour.Update`.

## Projectile
Surprisingly, projectiles are also entities. This implementation allows projectiles to be affected by other projectiles. If you want this behaviour, simply override `Entity.TakeDamage`.

This class will manage most of the things a projectile is:
* Reset of itself
* Collision detection with other entities
* Update of its conditions
* Hold and modify its attack
* Despawn when killed

Important notes:
* Projectiles don't have any physical colliders. They need to be able to phase through entities
* `Projectile` does not automatically manage its movement. You will need to add your own logic to make the projectile move 

### Attack
You can damage entities by creating an `Attack` object. This class determines all the characterics of an attack: type of the attack, who this attack can hit, amount of damage this attack deals.

In most cases, you will need to create an attack when you're creating a projectile and calling `Projectile.Attribute`.

### Heal
You can heal entities by creating a `Heal` object. This class determines all the characterics of a heal: type of the heal, amount of health this heal recovers.

### ProjectileCondition
By default, projectiles are eternally alive. If they are not killed manually, they will stay alive forever. However, you can define conditions that the projectile must meet in order to stay alive. For example, a projectile could only stay alive for X seconds.

In order to achieve this, you need to add a class that inherits `ProjectileCondition` script to your projectile. This will allow you to configure your projectile without leaving the editor. These conditions are automatically managed by ̀`Projectile` and don't need to be manually updated or reset.

If you need to create a custom condition, you can simply create a class that inherits the appropriate parent.

## Object Pools
As an optimization technique, projectiles are made to use an `ObjectPool`. In this project, we have two types of pools:
* Local pool
* Global pool

No matter the type used, they both act the same and they both work as object pools.

### Local pools
Local pools are `ObjectPool` that are already inside a prefab. For example, the cannon's prefab contains an object pool that will follow it, no matter the scene. This is to specially avoid cases where an object needs a pool, but it has a pool in one scene but not the other.

In most cases, it is adviced to add the script `AutoNoParent` to the object pool. This will move the object pool to the root of the scene, preventing some weird behaviours.

### Global pools
Global pools are `ObjectPool`that are accessible from anywhere. This is mostly used for cases where a scene has objects to pool and multiple scripts want to be able to access it. 
