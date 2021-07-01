Imports System.Text
Imports System.Text.RegularExpressions

Namespace Models
    Public Class PumpTemplateNameBuilder
        Public Shared Property Separator As String = "x"
        Public Shared Property Break As String = "-"

        Private Property PumpTemplate As PumpTemplate
        Private Property OutputBuilder As New StringBuilder()

        Public Sub New(pumpTemplate As PumpTemplate)
            Me.PumpTemplate = New PumpTemplate() With {
                .PumpTemplateID = pumpTemplate.PumpTemplateID,
                .Barrel = New PumpBarrel() With {
                    .Type = pumpTemplate.Barrel.Type.Prepare(),
                    .Material = pumpTemplate.Barrel.Material.Prepare(),
                    .Washer = pumpTemplate.Barrel.Washer.Prepare(),
                    .Length = pumpTemplate.Barrel.Length.Prepare()
                },
                .Plunger = New PumpPlunger() With {
                    .Fit = pumpTemplate.Plunger.Fit.Prepare(),
                    .Length = pumpTemplate.Plunger.Length.Prepare(),
                    .Material = pumpTemplate.Plunger.Material.Prepare()
                },
                .Seating = New PumpSeating() With {
                    .Location = pumpTemplate.Seating.Location.Prepare(),
                    .Type = pumpTemplate.Seating.Type.Prepare()
                },
                .TubingSize = pumpTemplate.TubingSize.Prepare(),
                .PumpBoreBasic = pumpTemplate.PumpBoreBasic.Prepare(),
                .LowerExtension = pumpTemplate.LowerExtension.Prepare(),
                .UpperExtension = pumpTemplate.UpperExtension.Prepare(),
                .PumpType = pumpTemplate.PumpType.Prepare(),
                .HoldDownType = pumpTemplate.HoldDownType.Prepare(),
                .StandingValveCages = pumpTemplate.StandingValveCages.Prepare(),
                .StandingValve = pumpTemplate.StandingValve.Prepare(),
                .BallsAndSeats = pumpTemplate.BallsAndSeats.Prepare(),
                .TravellingCages = pumpTemplate.TravellingCages.Prepare(),
                .Collet = pumpTemplate.Collet.Prepare(),
                .TopSeals = pumpTemplate.TopSeals.Prepare(),
                .OnOffTool = pumpTemplate.OnOffTool.Prepare(),
                .SpecialtyItems = pumpTemplate.SpecialtyItems.Prepare(),
                .PonyRods = pumpTemplate.PonyRods.Prepare(),
                .Strainers = pumpTemplate.Strainers.Prepare(),
                .KnockOut = pumpTemplate.KnockOut.Prepare(),
                .Markup = pumpTemplate.Markup,
                .Discount = pumpTemplate.Discount
            }

            Me.PumpTemplate.Plunger.Length = Me.PumpTemplate.Plunger.Length.Replace(" ", "")
        End Sub

        Public Function GenerateName() As String
            RebuildNameFromTemplate()

            Return OutputBuilder.ToString()
        End Function

        Private Sub RebuildNameFromTemplate()
            OutputBuilder.Clear()

            AppendValueOrDefault(PumpTemplate.TubingSize)

            AppendSeparator()
            AppendValueOrDefault(PumpTemplate.PumpBoreBasic, [default]:="Error")

            AppendSeparator()
            AppendLeftWords(PumpTemplate.Barrel.Length, 1)

            AppendBreak()
            AppendTernary(PumpTemplate.LowerExtension = "none", "0", PumpTemplate.LowerExtension)

            AppendSeparator()
            AppendTernary(PumpTemplate.UpperExtension = "none", "0", PumpTemplate.UpperExtension)

            AppendBreak()
            AppendPumpType()
            AppendBarrelType()

            AppendBreak()
            AppendSeatingLocation()

            AppendBreak()
            AppendSeatingType()
            AppendBarrelMaterial()

            AppendBreak()
            AppendPlungerMaterial()

            AppendBreak()
            AppendValueOrDefault(PumpTemplate.Plunger.Length, "Error")
            Append(PumpTemplate.Plunger.Fit)

            AppendBreak()
            AppendPumpHoldDownType()

            AppendBreak()
            AppendTravelingCages()

            AppendBreak()
            AppendStandingValveCages()

            AppendBreak()
            AppendStandingValve()

            AppendBreak()
            AppendBallsAndSeats()

            AppendBreak()
            AppendTernary(PumpTemplate.Barrel.Washer = "barrel washer", "BW", "NA")

            AppendBreak()
            AppendCollet()

            AppendBreak()
            AppendTopSeals()

            AppendBreak()
            AppendTernary(PumpTemplate.OnOffTool = "included", "OnOff", "NA")

            AppendBreak()
            AppendSpecialtyItems()

            AppendBreak()
            AppendPonyRods()

            AppendBreak()
            AppendStrainers()

            AppendBreak()
            AppendTernary(PumpTemplate.KnockOut = "included", "KNO", "NA")
        End Sub

        Private Sub Append(value As String)
            OutputBuilder.Append(value)
        End Sub

        Private Sub AppendSeparator()
            Append(Separator)
        End Sub

        Private Sub AppendBreak()
            Append(Break)
        End Sub

        Private Sub AppendValueOrDefault(value As String, Optional [default] As String = "None")
            If Not String.IsNullOrEmpty(value) Then
                Append(value)
            Else
                Append([default])
            End If
        End Sub

        Private Sub AppendLeftWords(from As String, wordCount As Integer)
            Dim endIndex As Integer = 0
            While wordCount > 0
                endIndex = from.IndexOf(" "c, endIndex)
                If endIndex < 0 Then
                    endIndex = from.Length
                    Exit While
                End If

                wordCount -= 1
            End While

            Append(from.Substring(0, endIndex))
        End Sub

        Private Sub AppendTernary(condition As Boolean, valueIfTrue As String, valueIfFalse As String)
            If condition Then
                Append(valueIfTrue)
            Else
                Append(valueIfFalse)
            End If
        End Sub

        Private Sub AppendPumpType()
            Select Case PumpTemplate.PumpType
                Case "rod"
                    Append("R")
                Case "tubing"
                    Append("T")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendBarrelType()
            Select Case PumpTemplate.Barrel.Type
                Case "heavy wall for metal plunger pumps"
                    Append("H")
                Case "thin wall for metal plunger pumps"
                    Append("W")
                Case "heavy wall for soft packed plunger pumps"
                    Append("P")
                Case "thin wall for soft packed plunger pumps"
                    Append("S")
                Case "heavy wall for metal plunger pumps, thin"
                    Append("X")
                Case "wall thread configuration"
                    Append("C")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendSeatingLocation()
            Select Case PumpTemplate.Seating.Location
                Case "bottom, traveling barrel"
                    Append("T")
                Case "bottom"
                    Append("B")
                Case "top"
                    Append("A")
                Case Else
                    Append("ERR")
            End Select
        End Sub

        Private Sub AppendSeatingType()
            Select Case PumpTemplate.Seating.Type
                Case "cup"
                    Append("C")
                Case "mechanical"
                    Append("M")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendBarrelMaterial()
            Select Case PumpTemplate.Barrel.Material
                Case "brass/cp"
                    Append("BRZCP")
                Case "brass/nickel"
                    Append("BRZNP")
                Case "carburized"
                    Append("CARB")
                Case "chrome plated"
                    Append("CP")
                Case "nickel carbide"
                    Append("NIC")
                Case "stainless steel chrome plated"
                    Append("SSCP")
                Case "tbg"
                    Append("TBG")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendPlungerMaterial()
            Select Case PumpTemplate.Plunger.Material
                Case "spay metal"
                    Append("SM")
                Case "chrome plate"
                    Append("CP")
                Case "cast iron"
                    Append("CI")
                Case "other"
                    Append("OTH")
                Case "spay metal w/mon pin"
                    Append("SMW")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendPumpHoldDownType()
            Select Case PumpTemplate.HoldDownType
                Case "2"" api mech t/l"
                    Append("2.0T")
                Case "2.5"" api mech t/l"
                    Append("2.5T")
                Case "3"" api mech t/l"
                    Append("3.0T")
                Case "4"" api mech t/l"
                    Append("4.0T")
                Case "2"" api mech b/l"
                    Append("2.0B")
                Case "2.5"" api mech b/l"
                    Append("2.5B")
                Case "3"" api mech b/l"
                    Append("3.0B")
                Case "4"" api mech b/l"
                    Append("4.0B")
                Case "cup hold down"
                    Append("CUP")
                Case "none"
                    Append("NA")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendTravelingCages()
            Select Case PumpTemplate.TravellingCages
                Case "alloy full open"
                    Append("AFO")
                Case "alloy full open tv/igc sv"
                    Append("ATIS")
                Case "igc/tv/alloy sv"
                    Append("ITAS")
                Case "steel hard lined"
                    Append("SH")
                Case "steel insert guided cage"
                    Append("IGC")
                Case "steel spring loaded"
                    Append("SL")
                Case "stainless steel full open"
                    Append("SSFO")
                Case "stainless steel hard lined"
                    Append("SSHL")
                Case "stainless steel insert guided cage"
                    Append("SSIGC")
                Case "stainless steel spring loaded"
                    Append("SSSL")
                Case "standard"
                    Append("STD")
                Case "brass"
                    Append("BRZ")
                Case "nickel copper alloy"
                    Append("NCA")
                Case "monel"
                    Append("MON")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendStandingValveCages()
            Select Case PumpTemplate.StandingValveCages
                Case "alloy full open"
                    Append("AFO")
                Case "alloy full open tv/igc sv"
                    Append("ATIS")
                Case "igc/tv/alloy sv"
                    Append("ITAS")
                Case "steel hard lined"
                    Append("SH")
                Case "steel insert guided cage"
                    Append("IGC")
                Case "steel spring loaded"
                    Append("SL")
                Case "stainless steel full open"
                    Append("SSFO")
                Case "stainless steel hard lined"
                    Append("SSHL")
                Case "stainless steel insert guided cage"
                    Append("SSIGC")
                Case "stainless steel spring loaded"
                    Append("SSSL")
                Case "standard"
                    Append("STD")
                Case "brass"
                    Append("BRZ")
                Case "nickel copper alloy"
                    Append("NCA")
                Case "monel"
                    Append("MON")
                Case "dump valve"
                    Append("DMPV")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendStandingValve()
            Select Case PumpTemplate.StandingValve
                Case "dump valve"
                    Append("DMP")
                Case "positive"
                    Append("POS")
                Case "retrievable standing valve"
                    Append("WSV")
                Case "none"
                    Append("NA")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendBallsAndSeats()
            Select Case PumpTemplate.BallsAndSeats
                Case "acid/tungsten carbide"
                    Append("RX/TC")
                Case "silicon nitride/tungsten carbide"
                    Append("SN/TC")
                Case "stainless steel/stainless steel"
                    Append("SS/SS")
                Case "stainless steel/tungsten carbide"
                    Append("SS/TC")
                Case "titanium/tungsten carbide"
                    Append("A+/TC")
                Case "tungsten carbide/tungsten carbide"
                    Append("TC/TC")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendCollet()
            Select Case PumpTemplate.Collet
                Case "vrb"
                    Append("VRB")
                Case "vrb/tpc"
                    Append("TPC")
                Case "none"
                    Append("NA")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendTopSeals()
            Select Case PumpTemplate.TopSeals
                Case "page top seals"
                    Append("PTS")
                Case "evitop seal"
                    Append("EVI")
                Case "none"
                    Append("NA")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendSpecialtyItems()
            Select Case PumpTemplate.SpecialtyItems
                Case "gas valve"
                    Append("GV")
                Case "ring valve"
                    Append("RV")
                Case "charger valve"
                    Append("CV")
                Case "none"
                    Append("NA")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendPonyRods()
            Select Case PumpTemplate.PonyRods
                Case "0.75"" c"
                    Append("0.75""C")
                Case "0.75"" d"
                    Append("0.75""D")
                Case "0.75"" 97/hd"
                    Append("0.75""97HD")
                Case "1.0"" d"
                    Append("1.0""D")
                Case ".875"" d"
                    Append(".875"" D")
                Case ".875"" 97/hd"
                    Append(".875"" 97/HD")
                Case "none"
                    Append("NA")
                Case Else
                    Append("Error")
            End Select
        End Sub

        Private Sub AppendStrainers()
            Select Case PumpTemplate.Strainers
                Case "1"" x 6"""
                    Append("1x6")
                Case "1"" x 12"""
                    Append("1x12")
                Case "1"" x 24"""
                    Append("1x24")
                Case "1"" x 36"""
                    Append("1x36")
                Case "1.25"" x 6"""
                    Append("1.25x6")
                Case "1.25"" x 12"""
                    Append("1.25x12")
                Case "1.25"" x 24"""
                    Append("1.25x24")
                Case "1.25"" x 36"""
                    Append("1.25x36")
                Case "1.5"" x 6"""
                    Append("1.5x6")
                Case "1.5"" x 12"""
                    Append("1.5x12")
                Case "1.5"" x 24"""
                    Append("1.5x24")
                Case "1.5"" x 36"""
                    Append("1.5x36")
                Case "none"
                    Append("NA")
                Case Else
                    Append("Error")
            End Select
        End Sub
    End Class

    Friend Module PumpTemplateNameBuilderExtensions
        <System.Runtime.CompilerServices.Extension()> _
        Public Function Prepare(value As String) As String
            If Not String.IsNullOrEmpty(value) Then
                Return value.Trim().ToLower()
            Else
                Return String.Empty
            End If
        End Function
    End Module
End Namespace