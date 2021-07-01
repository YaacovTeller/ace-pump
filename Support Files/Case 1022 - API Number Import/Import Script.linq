<Query Kind="VBProgram" />

' Per customer request (see case):
'		1. Import matching wells
'		2. Export all wells remaining w/ no API number
Private Property CURRENT_SCRIPT_PATH as string = Path.GetDirectoryName (Util.CurrentQueryPath)
Private Property SENECA_CSV_NAME As String = "original.csv"
Private Property SENECA_CSV_PATH As String = CURRENT_SCRIPT_PATH & "\" & SENECA_CSV_NAME
Private Property ACE_DB_CONN_STRING As String = "Data Source=web6\netsmith;Initial Catalog=AcePump;Persist Security Info=True;User ID=AcePump_IUsr;Password=AcePump; MultipleActiveResultSets=True"
Private Property OUTPUT_CSV_PATH As String = CURRENT_SCRIPT_PATH & "\output.csv"
Private Property INTERIM_CSV_PATH As String = CURRENT_SCRIPT_PATH & "\interim.csv"

Private Property AceWells As List(Of AceWellInfo)
Private Property AceLeases As List(Of AceLeaseInfo)
Private Property SenecaWells As List(Of SenecaWellInfo)
Private Property LeaseNameCorrections As New Dictionary(Of String, Integer)
Private Property LeaseSkips As New List(Of String)

Private Property SenecaWellLoader As IEnumerator(Of SenecaWellInfo)

Sub Main()
   RegisterSigIntAndErrorHandler()
   LoadAceWells()
   MatchSenecaWells()
   ImportMatchesToDb()
End Sub

Private Sub RegisterSigIntAndErrorHandler()
	AddHandler Console.CancelKeyPress, Sub(sender As Object, e As ConsoleCancelEventArgs)
									       CleanUpAndTerminate()
									   End Sub
									   
	AddHandler AppDomain.CurrentDomain.UnhandledException, Sub(sender As Object, e As UnhandledExceptionEventArgs)
														       CleanUpAndTerminate()
														   End Sub
End Sub

Private Sub CleanUpAndTerminate()
	Console.WriteLine()
	Console.WriteLine()
	Console.WriteLine("Terminating...")

	LoadRemainingSenecaWells()
	SenecaWellLoader.Dispose()
	SaveInterim()
	
	Dim wellsAsEnum As IEnumerable(Of SenecaWellInfo) = SenecaWells.AsEnumerable()
	Console.WriteLine()
	Console.WriteLine(String.Format("Matched: {0}, Skipped: {1}, Did not process: {2}", wellsAsEnum.Count(Function(x) x.MatchedAceWell IsNot Nothing), wellsAsEnum.Count(Function(x) x.Skip), wellsAsEnum.Count(Function(x) Not x.Skip And x.MatchedAceWell Is Nothing)))
End Sub

Private Sub ImportMatchesToDb()
	Using conn As New System.Data.SqlClient.SqlConnection(ACE_DB_CONN_STRING)
   	   conn.Open()
       
	   For Each well As SenecaWellInfo In SenecaWells
	   	   If Not well.Skip Then
		   	   Dim sql As String = "UPDATE tblWellLocation SET APINumber='" & well.ApiNumber & "' WHERE KPWellLocationID=" & well.MatchedAceWell.AceWellID
	   	   	   Dim cmd As New System.Data.SqlClient.SqlCommand(sql, conn)
			   cmd.ExecuteNonQuery()
		   End If
       Next
   End Using
End Sub

Private Sub SaveInterim()
	Using csvOut As New System.IO.StreamWriter(INTERIM_CSV_PATH)
       csvOut.WriteLine("Skip, Ace Well ID, Ace Well Number, Ace Lease Name, Seneca Well Name, API Number, Seneca Well ID")

       For Each well As SenecaWellInfo In SenecaWells
	   	   If well.Skip Then
		   	   csvOut.WriteLine(String.Format("1,,,,{0},{1},{2}", well.SenecaWellName, well.ApiNumber, well.SenecaWellID))
		   ElseIf well.MatchedAceWell IsNot Nothing
               csvOut.WriteLine(String.Format("0,{0},{1},{2},{3},{4},{5}", well.MatchedAceWell.AceWellID, well.MatchedAceWell.WellNumber, well.MatchedAceWell.Lease.LeaseName, well.SenecaWellName, well.ApiNumber, well.SenecaWellID))
		   Else 'If Not Processed
		       csvOut.WriteLine(String.Format("0,,,,{0},{1},{2}", well.SenecaWellName, well.ApiNumber, well.SenecaWellID))
		   End If
       Next
   End Using
End Sub

Private Sub LoadRemainingSenecaWells()
	While SenecaWellLoader.MoveNext()
		SenecaWells.Add(SenecaWellLoader.Current)
	End While
End Sub

Sub LoadAceWells()
   AceWells = New List(Of AceWellInfo)()
   AceLeases = New List(Of AceLeaseInfo)()

   Using conn As New System.Data.SqlClient.SqlConnection(ACE_DB_CONN_STRING)
   	   conn.Open()
       Dim cmd As New System.Data.SqlClient.SqlCommand("SELECT w.KPWellLocationID, w.WellNumber, w.KFLeaseID, l.LocationName " & _
                                                       "FROM tblWellLocation w " & _
                                                       "INNER JOIN tblLeaseLocations l on w.KFLeaseID = l.KPLeaseID", conn)

       Dim wellReader As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
       While wellReader.Read()
           AceWells.Add(New AceWellInfo() With {
                           .AceWellID = wellReader.GetInt32(0),
                           .WellNumber = wellReader.GetString(1).Trim(),
                           .Lease = GetOrCreateLease(wellReader.GetInt32(2), wellReader.GetString(3).Trim())
                       })
       End While
   End Using
End Sub

Private Function GetOrCreateLease(leaseId As Integer, leaseName As String) As AceLeaseInfo
   Dim lease As AceLeaseInfo = AceLeases.FirstOrDefault(Function(x) x.LeaseID = leaseId)
   If lease Is Nothing Then
       lease = New AceLeaseInfo() With {
           .LeaseID = leaseId,
           .LeaseName = leaseName
       }

       AceLeases.Add(lease)
   End If

   Return lease
End Function

Private Function GetSenecaWellLoader() As IEnumerator(Of SenecaWellInfo)
	Dim loader As IEnumerator(Of SenecaWellInfo) = Nothing
	If IO.File.Exists(INTERIM_CSV_PATH) Then
		Console.Write("Interim file exists, (u)se it or (r)estart from original? ")
		Dim r As String = Console.ReadLine()
		
		If r = "U" Or r = "u" Then
			Dim interimWellInfos As IEnumerable(Of SenecaWellInfo) = New ApiNumberCsvEnumerable(INTERIM_CSV_PATH, AceWells)
			loader = interimWellInfos.GetEnumerator()
		End If
	End If
	
	If loader Is Nothing Then
		Dim originalWellInfos As IEnumerable(Of SenecaWellInfo) = New ApiNumberCsvEnumerable(SENECA_CSV_PATH, AceWells)
		loader = originalWellInfos.GetEnumerator()
	End If
	
	Return loader
End Function

Sub MatchSenecaWells()
   SenecaWells = New List(Of SenecaWellInfo)()
   SenecaWellLoader = GetSenecaWellLoader()

   SKIP_TO_NEXT:
   While SenecaWellLoader.MoveNext()
   	   Dim senecaWellInfo As SenecaWellInfo = SenecaWellLoader.Current
	   SenecaWells.Add(senecaWellInfo)
	   
	   If senecaWellInfo.Skip Or senecaWellInfo.MatchedAceWell IsNot Nothing Then GOTO SKIP_TO_NEXT
       
	   Dim matches As List(Of AceWellInfo) = AceWells.Where(Function(x) String.Equals(x.Lease.LeaseName, senecaWellInfo.SuggestedLeaseName, StringComparison.InvariantCultureIgnoreCase) And String.Equals(x.WellNumber, senecaWellInfo.SuggestedWellNumber, StringComparison.InvariantCultureIgnoreCase)).ToList()
       While matches.Count <> 1
           If matches.Count > 1 Then
               HandleTooManyMatches(matches, senecaWellInfo)
           ElseIf matches.Count = 0 AndAlso Not TryExactWellNameMatch(senecaWellInfo) Then
               HandleNoMatches(senecaWellInfo)
			   If senecaWellInfo.Skip Then
			       GOTO SKIP_TO_NEXT
			   End If
           End If

           matches = AceWells.Where(Function(x) String.Equals(x.Lease.LeaseName, senecaWellInfo.SuggestedLeaseName, StringComparison.InvariantCultureIgnoreCase) And String.Equals(x.WellNumber, senecaWellInfo.SuggestedWellNumber, StringComparison.InvariantCultureIgnoreCase)).ToList()
       End While

       senecaWellInfo.MatchedAceWell = matches(0)
   End While
End Sub

Private Sub HandleTooManyMatches(matches As List(Of AceWellInfo), target As SenecaWellInfo)
   Dim matchingIds As List(Of Integer) = matches.Select(Function(x) x.AceWellID).ToList()
   Dim matchingIdString As String = String.Join(", ", matchingIds)
   Console.WriteLine()
   Console.WriteLine(String.Format("Found {0} wells with lease '{1}' and number '{2}' (at line {3} in {4})", matches.Count, target.SuggestedLeaseName, target.SuggestedWellNumber, target.LineNumber, SENECA_CSV_PATH))
   Console.WriteLine("Matching Ace DB tblWellLocation.KPWellLocationIDs: " & matchingIdString)
   Console.WriteLine("Please correct the problem in SSMS and press any key to retry...")
   Console.Read()

   ReloadAceWells(matches)
End Sub

Private Sub HandleNoMatches(target As SenecaWellInfo)
   If LeaseSkips.Contains(target.SuggestedLeaseName) Then
       target.Skip = True
	   Return
	   
   ElseIf LeaseNameCorrections.ContainsKey(target.SuggestedLeaseName) AndAlso target.CurrentSplitIndex <> LeaseNameCorrections(target.SuggestedLeaseName) Then
       target.ResplitLeaseAndWell(LeaseNameCorrections(target.SuggestedLeaseName))
       Return
   End If

   Dim leaseExistsInDb As Boolean = AceWells.Any(Function(x) String.Equals(x.Lease.LeaseName, target.SuggestedLeaseName, StringComparison.InvariantCultureIgnoreCase))
   Console.WriteLine()
   Console.WriteLine(String.Format("LEASE: '{0}' WELL: '{1}' not in DB (split from '{2}' at line {3} in {4})", target.SuggestedLeaseName, target.SuggestedWellNumber, target.SenecaWellName, target.LineNumber, SENECA_CSV_NAME))
   Console.WriteLine(String.Format("(there is {0} lease '{1}' in the DB)", If(leaseExistsInDb, "a", "NO"), target.SuggestedLeaseName))
   Dim splitAt As Integer = ConsoleReadInteger("Enter a new character number to split at, or -1 for manual entry, -2 to skip well, -3 to skip entire lease: ")

   If splitAt = -1 Then
       Console.Write("Enter corrected lease name: ")
       target.SuggestedLeaseName = Console.ReadLine()

       Console.Write("Enter corrected well number: ")
       target.SuggestedWellNumber = Console.ReadLine()

   ElseIf splitAt = -2 Then
       target.Skip = True
	   
   ElseIf splitAt = -3 Then
   	   target.Skip = True
	   LeaseSkips.Add(target.SuggestedLeaseName)

   Else
       LeaseNameCorrections.Add(target.SuggestedLeaseName, splitAt)
       target.ResplitLeaseAndWell(splitAt)
   End If
End Sub

Private Function TryExactWellNameMatch(info As SenecaWellInfo) As Boolean
	Dim matches As List(Of AceWellInfo) = AceWells.Where(Function(x) String.Equals(x.WellNumber, info.SenecaWellName, StringComparison.InvariantCultureIgnoreCase)).ToList()
	If matches.Count > 1 Then
		Console.WriteLine()
		Console.WriteLine(String.Format("LEASE '{0}' WELL '{1}' (orig '{2}') not in DB, but found exact well number matches.", info.SuggestedLeaseName, info.SuggestedWellNumber, info.SenecaWellName))
		For i As Integer = 0 To matches.Count - 1
			Console.WriteLine(String.Format("{0}: lease '{1}', well '{2}'", i, matches(i).Lease.LeaseName, matches(i).WellNumber))
		Next
		
		Dim line As Integer = ConsoleReadInteger("Enter number to use or -1 for other options: ")
		If line >= 0 Then
			Dim exactMatch As AceWellInfo = matches(line)
			info.SuggestedLeaseName = exactMatch.Lease.LeaseName
			info.SuggestedWellNumber = exactMatch.WellNumber
			Return True
		End If
	
	ElseIf matches.Count = 1 Then
		Dim exactMatch As AceWellInfo = matches(0)
		info.SuggestedLeaseName = exactMatch.Lease.LeaseName
		info.SuggestedWellNumber = exactMatch.WellNumber
		Return True
	End If
	
	Return False
End Function

Private Function ConsoleReadInteger(msg As String) As Integer
   Console.Write(msg)
   Dim input As String = Console.ReadLine()

   Dim parsed As Integer
   While Not Integer.TryParse(input, parsed)
       Console.Write("Invalid entry. " & msg)
       input = Console.ReadLine()
   End While

   Return parsed
End Function

Private Sub ReloadAceWells(toReload As List(Of AceWellInfo))
   Dim reloadIds As List(Of Integer) = toReload.Select(Function(x) x.AceWellID).ToList()
   Dim reloadIdString As String = String.Join(", ", reloadIds)
   For Each i As AceWellInfo In toReload
       AceWells.Remove(i)
   Next

   Using conn As New System.Data.SqlClient.SqlConnection(ACE_DB_CONN_STRING)
   	   conn.Open()
       Dim cmd As New System.Data.SqlClient.SqlCommand("SELECT w.KPWellLocationID, w.WellNumber, w.KFLeaseID, l.LocationName " & _
                                                       "FROM tblWellLocation w " & _
                                                       "INNER JOIN tblLeaseLocations l on w.KFLeaseID = l.KPLeaseID _" & _
                                                       "WHERE w.KPWellLocationID IN (" & reloadIdString & ")", conn)

       Dim wellReader As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
       While wellReader.NextResult()
           AceWells.Add(New AceWellInfo() With {
                           .AceWellID = wellReader.GetInt32(0),
                           .WellNumber = wellReader.GetString(1),
                           .Lease = GetOrCreateLease(wellReader.GetInt32(2), wellReader.GetString(3))
                       })
       End While
   End Using
End Sub

Public Class ApiNumberCsvEnumerable
   Implements IEnumerable(Of SenecaWellInfo)

   Private Property CsvPath As String
   Private Property AceWells As List(Of AceWellInfo)

   Public Sub New(csvPath As String, aceWells As List(Of AceWellInfo))
       Me.CsvPath = csvPath
	   Me.AceWells = aceWells
   End Sub

   Public Function GetEnumerator() As IEnumerator(Of SenecaWellInfo) Implements IEnumerable(Of SenecaWellInfo).GetEnumerator
       Return New ApiNumberCsvEnumerator(CsvPath, AceWells)
   End Function

   Public Function GetEnumerator2() As IEnumerator Implements IEnumerable.GetEnumerator
       Return GetEnumerator()
   End Function
End Class

Public Class ApiNumberCsvEnumerator
   Implements IEnumerator(Of SenecaWellInfo)

   Private Property CsvPath As String
   Private Property LineNumber As Integer

   Private _Reader As System.IO.StreamReader
   Private ReadOnly Property Reader As System.IO.StreamReader
       Get
           If _Reader Is Nothing Then
               _Reader = New StreamReader(CsvPath)
			   _Reader.ReadLine()
           End If

           Return _Reader
       End Get
   End Property
   
   Private Property AceWells As List(Of AceWellInfo)

   Public Sub New(csvPath As String, aceWells As List(Of AceWellInfo))
       Me.CsvPath = csvPath
	   Me.AceWells = aceWells
       LineNumber = 0
   End Sub

   Private _Current As SenecaWellInfo
   Public ReadOnly Property Current As SenecaWellInfo Implements IEnumerator(Of SenecaWellInfo).Current
       Get
           Return _Current
       End Get
   End Property

   Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
       Get
           Return Current
       End Get
   End Property

   Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
       If Reader.Peek() > 0 Then
           Dim currentLine As String = Reader.ReadLine()
           Dim vals As String() = currentLine.Split(",")

           _Current = New SenecaWellInfo() With {
		       .LineNumber = LineNumber,
		   	   .Skip = Integer.Parse(vals(0)) = 1, 
			   .MatchedAceWell = TryLoadPrematchedWell(vals(1)),
               .SenecaWellName = vals(4),
               .ApiNumber = vals(5),
               .SenecaWellID = Integer.Parse(vals(6))
           }

           Dim splitIndex As Integer = _Current.SenecaWellName.IndexOf(" ")
		   If splitIndex < 0 Then splitIndex = _Current.SenecaWellName.IndexOf("-")
		   If splitIndex < 0 Then splitIndex = 0
           _Current.ResplitLeaseAndWell(splitIndex)

           LineNumber += 1

           Return True

       Else
           Return False
       End If
   End Function

   Public Sub Reset() Implements IEnumerator.Reset
       Throw New NotSupportedException()
   End Sub
   
   Private Function TryLoadPrematchedWell(wellId As String) As AceWellInfo
   	   Dim match As AceWellInfo = Nothing

	   If Not String.IsNullOrEmpty(wellId) Then
	       Dim parsed As Integer = Integer.Parse(wellId)
		   match = AceWells.SingleOrDefault(Function(x) x.AceWellID = parsed)
	   End If
	   
	   Return match
   End Function

   Public Sub Dispose() Implements IDisposable.Dispose
       If _Reader IsNot Nothing Then
           _Reader.Dispose()
       End If
   End Sub
End Class

Public Class SenecaWellInfo
   Public Property LineNumber As Integer
   Public Property SenecaWellName As String
   Public Property ApiNumber As String
   Public Property SenecaWellID As Integer

   Public Property SuggestedLeaseName As String
   Public Property SuggestedWellNumber As String
   Public Property [Skip] As Boolean
   Public Property CurrentSplitIndex As Integer

   Public Sub ResplitLeaseAndWell(splitIndex As Integer)
   	   CurrentSplitIndex = splitIndex
       SuggestedLeaseName = SenecaWellName.Substring(0, splitIndex)
       SuggestedWellNumber = SenecaWellName.Substring(splitIndex + 1)
   End Sub

   Public Property MatchedAceWell As AceWellInfo
End Class

Public Class AceWellInfo
   Public Property AceWellID As Integer
   Public Property WellNumber As String
   Public Property Lease As AceLeaseInfo
End Class

Public Class AceLeaseInfo
   Public Property LeaseID As Integer
   Public Property LeaseName As String
End Class