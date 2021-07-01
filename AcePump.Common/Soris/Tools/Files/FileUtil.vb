Imports System.IO

Namespace Tools.Files
    Public Class FileUtil
        ''' <summary>
        ''' Is this a JPG?
        ''' </summary>
        ''' <param name="FileBytes">FileBytes is a member of the FileUpload control and provides the underlying data in the uploaded file.</param>
        ''' <returns>True if the magic number indicates this is an image, otherwise false.</returns>
        ''' <remarks></remarks>
        Public Shared Function IsJPG(ByVal FileBytes As Byte()) As Boolean
            'jpg magic number is four bytes 0xFF, 0xD8, 0xFF, 0xE0
            'last byte could be 0xE1 (undocumented based on observation by YY)
            If FileBytes.Length < 4 Then
                Return False
            End If

            'Check the magic number
            If FileBytes(0) = &HFF And FileBytes(1) = &HD8 And FileBytes(2) = &HFF And (FileBytes(3) = &HE0 Or FileBytes(3) = &HE1) Then
                Return True
            End If

            'Else this is not a jpg
            Return False
        End Function

        Public Shared Function SafeGetRandomFileName(ByVal Path As String, ByVal Extension As String) As String
            'Make sure path ends in a "/"
            If Path.Last <> System.IO.Path.DirectorySeparatorChar Then
                Path = Path & System.IO.Path.DirectorySeparatorChar
            End If

            'Make sure the path exists
            If Not Directory.Exists(Path) Then
                Directory.CreateDirectory(Path)
            End If

            'Make sure extension starts with a .
            If Extension.First <> "." Then
                Extension = "." & Extension
            End If

            'Random file name variable
            Dim RandomFileName As String = ""
            Dim RandomFilePath As String = ""

            'Cycle until we find an available name
            While RandomFilePath = ""
                'Get a random file name and remove the random extension (System.IO.Path.GetRandomFileName() returns a 8.3 file name).
                RandomFileName = System.IO.Path.GetRandomFileName().Remove(8) & Extension

                'Add full path data
                RandomFilePath = Path & RandomFileName

                'Make sure it's actually available (atomic)
                If Not File.Exists(RandomFilePath) Then
                    'Try to create this file.  This will prevent another person from stealing this file.
                    'On fail, reset the path
                    Try
                        'Create the file (do not need to write to keep file there)
                        Dim FileHandle As FileStream = File.Open(RandomFilePath, FileMode.CreateNew)

                        'Close the file so we'll have access to it in calling function
                        FileHandle.Close()
                    Catch ex As IOException
                        'Check string to make sure this is a file already exists exception
                        Dim ExpectedAlreadyExistsString As String = "The file '" & RandomFilePath & "' already exists."
                        If ex.Message.Equals(ExpectedAlreadyExistsString) Then
                            'Already exits, reset filename
                            RandomFilePath = ""
                        Else
                            'Else throw a real IOException
                            Throw ex
                        End If
                    End Try
                Else
                    'Already exists, reset filename
                    RandomFilePath = ""
                End If
            End While

            'Return the name
            Return RandomFileName
        End Function

        Public Shared Function GetExtension(ByVal Format As Format) As String
            Select Case Format
                Case Format.BMP
                    Return ".bmp"
                Case Format.GIF
                    Return ".gif"
                Case Format.WMP
                    Return ".?"
                Case Format.JPG
                    Return ".jpg"
                Case Format.PNG
                    Return ".png"
                Case Format.TIFF
                    Return ".tiff"
                Case Else
                    Return ".txt"
            End Select
        End Function

        Public Enum Format
            JPG
            BMP
            GIF
            PNG
            TIFF
            WMP
        End Enum

    End Class
End Namespace
