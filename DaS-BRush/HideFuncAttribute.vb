<Flags()>
Public Enum HideReason
    ''' <summary>
    ''' Bypasses the hiding and makes the attribute available for use in Scripting so you can do tests more easily.
    ''' Please be sure to use Visual Studio's "Find All References" on <see cref="HideReason.DebugBypassHide"/>  and remove all instances
    ''' of it before committing changes.
    ''' </summary>
    DebugBypassHide = &B1

    ''' <summary>
    ''' The function is simply incomplete. Not necessarily some limitation preventing it from being used in scripts, unless
    ''' something like <see cref="HideReason.UsesStringParameter"/> is also defined.
    ''' </summary>
    Unfinished = &B10

    ''' <summary>
    ''' The function was never intended to be called from scripts in the first place. 
    ''' e.g. <see cref="Funcs.funccall(String, String, String, String, String, String)"/>, etc.
    ''' </summary>
    Invalid = &B100

    ''' <summary>
    ''' Uses a string parameter so we literally just cannot call it currently ¯\_(ツ)_/¯
    ''' (Comment <see cref="HideReason.UsesStringParameter"/> out of existance when we implement string parameters)
    ''' </summary>
    UsesStringParameter = &B1000
End Enum

<AttributeUsage(AttributeTargets.Method)> Public Class HideFuncAttribute
    Inherits Attribute

    Private ReadOnly Reason As HideReason

    ''' <summary>
    ''' Make attribute hidden in autocomplete and unusable from scripts.
    ''' </summary>
    ''' <param name="_reason">The reason it is hidden. This is so we can find them easily.</param>
    Public Sub New(_reason As HideReason)
        Reason = _reason
    End Sub

End Class
