Namespace Models
    Public Class Pump
        Public Property PumpID As Integer

        Public Property InstalledInWellID As Integer?
        Public Overridable Property InstalledInWell As Well

        Public Property PumpTemplateID As Integer
        Public Overridable Property PumpTemplate As PumpTemplate

        Public Property ShopLocationID As Integer
        Public Overridable Property ShopLocation As ShopLocation
        Public Property PumpNumber As String
    End Class
End Namespace