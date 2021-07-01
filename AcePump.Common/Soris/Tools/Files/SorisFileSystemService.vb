Imports System.IO

Namespace Tools.Files
    Public Class SorisFileSystemService
        Public Function SaveRandomFile(inputStream As Stream, extension As String, folderPath As String) As String
            Dim randomFileName As String = SafeGetRandomFileName(folderPath, extension)
            Dim saveFilePath As String = folderPath & randomFileName

            Using fileStream As FileStream = File.OpenWrite(saveFilePath)
                inputStream.CopyTo(fileStream)
            End Using

            Return randomFileName
        End Function

        Public Function SaveRandomFile(data() As Byte, extension As String, folderPath As String) As String
            Dim dataStream As MemoryStream = New MemoryStream(data)

            Return SaveRandomFile(dataStream, extension, folderPath)
        End Function

        Public Sub SaveFile(data() As Byte, filePath As String)
            File.WriteAllBytes(filePath, data)
        End Sub

        Public Sub DeleteFile(path As String)
            File.Delete(path)
        End Sub

        Public Shared Function SafeGetRandomFileName(ByVal inPath As String, ByVal withExtension As String) As String
            If inPath.Last <> Path.DirectorySeparatorChar Then
                inPath = inPath & Path.DirectorySeparatorChar
            End If

            If Not Directory.Exists(inPath) Then
                Directory.CreateDirectory(inPath)
            End If

            If withExtension.First <> "." Then
                withExtension = "." & withExtension
            End If

            Dim RandomFileName As String = ""
            Dim RandomFilePath As String = ""

            While RandomFilePath = ""
                RandomFileName = Path.GetRandomFileName().Remove(8) & withExtension
                RandomFilePath = inPath & RandomFileName

                'atomic create
                If Not File.Exists(RandomFilePath) Then
                    Try
                        Dim FileHandle As FileStream = File.Open(RandomFilePath, FileMode.CreateNew)
                        FileHandle.Close()
                    Catch ex As IOException
                        Dim ExpectedAlreadyExistsString As String = "The file '" & RandomFilePath & "' already exists."
                        If ex.Message.Equals(ExpectedAlreadyExistsString) Then
                            RandomFilePath = ""
                        Else
                            Throw ex
                        End If
                    End Try
                Else
                    RandomFilePath = ""
                End If
            End While

            Return RandomFileName
        End Function

        Public Function FileExists(filePath As String) As Boolean
            Return File.Exists(filePath)
        End Function

        Public Function OpenFileForWriting(filePath As String) As Stream
            Return File.OpenWrite(filePath)
        End Function
    End Class
End Namespace
