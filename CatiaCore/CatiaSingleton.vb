Imports INFITF

Public Class CatiaSingleton

    Private Shared CATIA As Application

    Private Sub New()

    End Sub

    Friend Shared Function GetApplication() As Application
        If CATIA Is Nothing Then
            Try
                CATIA = System.Runtime.InteropServices.Marshal.GetActiveObject("CATIA.Application")
            Catch ex As Exception
                CATIA = Activator.CreateInstance(Type.GetTypeFromProgID("CATIA.Application"))
                CATIA.Visible = True
            End Try
        End If
        Return CATIA
    End Function


End Class
