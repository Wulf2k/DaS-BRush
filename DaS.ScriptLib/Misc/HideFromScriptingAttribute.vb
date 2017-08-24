''' <summary>
''' Hides Public shit from the autocomplete. Note that all public stuff is still accessable from lua scripts; it just
''' doesn't show up in auto complete. Making something "Friend" ("internal" in real programming languages) rather than Public
''' will make it actually unusable from scripting and hide it as well (the autocomplete member enumeration code explicitly 
''' checks that it is accessable from outside code).
''' </summary>
<AttributeUsage(AttributeTargets.All)> Friend Class HideFromScriptingAttribute
    Inherits Attribute
    Friend Shared Function Testerino() As String
        Return "Nigger"
    End Function
End Class
