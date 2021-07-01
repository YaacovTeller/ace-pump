Namespace UI.Tasks
    Public Interface IUITask
        ReadOnly Property ChangeOnProgressList As List(Of UIChangeOnProgress)
        ReadOnly Property TaskAction As Action
    End Interface
End Namespace
