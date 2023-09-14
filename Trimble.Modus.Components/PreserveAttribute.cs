﻿namespace Trimble.Modus.Components;

[AttributeUsage(AttributeTargets.All)]
public sealed class PreserveAttribute : Attribute
{
    public bool AllMembers;
    public bool Conditional;

    public PreserveAttribute(bool allMembers, bool conditional)
    {
        AllMembers = allMembers;
        Conditional = conditional;
    }

    public PreserveAttribute()
    {
    }
}
