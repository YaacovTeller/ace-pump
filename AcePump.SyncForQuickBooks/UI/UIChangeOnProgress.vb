Namespace UI
    Public Class UIChangeOnProgress
        Public ProgressStage As UIProgressStage
        Public OkButtonVisible As Boolean
        Public ProgressCancelButtonEnabled As Boolean
        Public StatusLabelText As String
        Public WorkerProgressBarValue As UIChangeProgressBarState
        Public ProgressWeightOutOf100 As Integer
    End Class

    Public Enum UIChangeProgressBarState As Integer
        Zero = 0
        AddPercentage = 1
        Maximum = 2
    End Enum
End Namespace