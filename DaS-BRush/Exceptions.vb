Imports DaS_BRush

Public MustInherit Class ExceptionMsg
    Inherits Exception

    Protected msg As String = "An exception ocurred."

    Public Overrides ReadOnly Property Message As String
        Get
            Return msg
        End Get
    End Property
End Class

Public Class FunctionNoExistException
    Inherits ExceptionMsg

    Public Sub New(ByVal funcName As String)
        msg = "The function name referenced (''" & funcName & "'') does not exist."
    End Sub
End Class

Public MustInherit Class ScriptVarException
    Inherits ExceptionMsg

    Protected ReadOnly svar As ScriptVarBase

    Public Sub New(ByRef svar As ScriptVarBase)
        Me.svar = svar
        msg = "A ScriptVar Exception Ocurred."
    End Sub
End Class

Public Class DidNotSetVariableValueException
    Inherits ScriptVarException

    Public Sub New(ByRef svar As ScriptVarBase)
        MyBase.New(svar)
        msg = "Unable to read the value of variable ''" & svar.Name & "'' before its value is set to something."
    End Sub
End Class

Public Class InvalidVariableValueTypeException
    Inherits ScriptVarException

    Public Sub New(ByRef svar As ScriptVarBase)
        MyBase.New(svar)
        msg = "The value of variable ''" & svar.Name & "'' (" & svar.ToString() & ") is not a valid " & svar.GetTypeName() & "-type value."
    End Sub
End Class

Public Class ScriptVarAlreadyDefinedException
    Inherits ScriptVarException

    Public Sub New(ByRef svar As ScriptVarBase)
        MyBase.New(svar)
        msg = "A ScriptVar with the name ''" & svar.Name & "'' has already been defined."
    End Sub
End Class