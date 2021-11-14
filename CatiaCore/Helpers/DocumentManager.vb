Imports System.IO
Imports System.Runtime.CompilerServices
Imports INFITF
Imports MECMOD
Imports ProductStructureTypeLib

Public Module DocumentManager

    <Extension>
    Sub SaveDoc(document As Document, targetFolder As String, name As String, docType As Doctype)
        If Directory.Exists(targetFolder) = False Then
            Directory.CreateDirectory(targetFolder)
        End If
        Dim extn As String = GetFileExtn(docType)

        Dim bSaveDFA
        Dim CATIA As Application = CatiaSingleton.GetApplication()
        bSaveDFA = CATIA.DisplayFileAlerts
        CATIA.DisplayFileAlerts = False
        document.SaveAs($"{targetFolder}\\{name}{extn}")
        CATIA.DisplayFileAlerts = bSaveDFA
    End Sub

    Public Function GetFileExtn(docType As Doctype) As String
        Dim extn As String = ""
        Select Case docType
            Case Doctype.Part
                extn = ".CATPart"
            Case Doctype.Product
                extn = ".CATProduct"
            Case Doctype.Drawing
                extn = ".CATDrawing"
            Case Else

        End Select
        Return extn
    End Function

    Function IsPart(objCurrentProduct As Product) As Boolean
        Dim CATIA As Application = CatiaSingleton.GetApplication()
        Dim oTestPart As PartDocument = Nothing
        On Error Resume Next
        oTestPart = CATIA.Documents.Item(objCurrentProduct.PartNumber & ".CATPart")

        If Not oTestPart Is Nothing Then
            IsPart = True
        Else
            IsPart = False
        End If
    End Function

    Function IsProduct(objCurrentProduct As Product) As Boolean
        Dim CATIA As Application = CatiaSingleton.GetApplication()
        Dim oTestProduct As ProductDocument = Nothing
        On Error Resume Next
        oTestProduct = CATIA.Documents.Item(objCurrentProduct.PartNumber & ".CATProduct")

        If Not oTestProduct Is Nothing Then
            IsProduct = True
        Else
            IsProduct = False
        End If
    End Function

End Module

Public Enum Doctype
    Part
    Product
    Drawing
End Enum
