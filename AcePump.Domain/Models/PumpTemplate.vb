Namespace Models
    Public Class PumpTemplate
        Public Property PumpTemplateID As Integer

        Public Property ConciseSpecificationSummary As String
        Public Property VerboseSpecificationSummary As String

        Public Overridable Property Parts As ICollection(Of TemplatePartDef)

        Public Property Barrel As PumpBarrel
        Public Property Plunger As PumpPlunger
        Public Property Seating As PumpSeating

        Public Property TubingSize As String
        Public Property PumpBoreBasic As String
        Public Property LowerExtension As String
        Public Property UpperExtension As String
        Public Property PumpType As String
        Public Property HoldDownType As String
        Public Property StandingValveCages As String
        Public Property StandingValve As String
        Public Property BallsAndSeats As String
        Public Property TravellingCages As String
        Public Property Collet As String
        Public Property TopSeals As String
        Public Property OnOffTool As String
        Public Property SpecialtyItems As String
        Public Property PonyRods As String
        Public Property Strainers As String
        Public Property KnockOut As String

        Public Property Markup As Decimal?
        Public Property Discount As Decimal?
    End Class
End Namespace