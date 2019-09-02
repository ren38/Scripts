using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    void apply(float deltaTime);
    float getEnd();
    void setEnd(float num);
    void end(ObjectActor subject);
    GameObject getIcon();
    void stack();
    float getDuration();
    void clearIconInstances();
}
