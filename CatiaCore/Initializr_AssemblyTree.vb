Imports CommonLibrary.DataAccess
Imports INFITF
Imports ProductStructureTypeLib

Public Class Initializr_AssemblyTree

    Public Sub New()

        Dim CATIA As Application = CatiaSingleton.GetApplication()
        oProductDoc = CATIA.ActiveDocument

        CompList = New List(Of CompTree)()
        GetNextNode(oProductDoc.Product, 1, False)

    End Sub

    Property CompList As List(Of CompTree)
    Dim oProductDoc As ProductDocument

    Public Sub Export()
        CompList.ExportCsv(oProductDoc.Name)
    End Sub

    Private Sub GetNextNode(oCurrentProduct As Product, SubLevel As Integer, IsSub As Boolean)

        Dim oCurrentTreeNode As Product
        Dim Level As Integer = 1
        Dim i As Integer

        ' Loop through every tree node for the current product
        For i = 1 To oCurrentProduct.Products.Count
            Dim Comp As CompTree = New CompTree()
            oCurrentTreeNode = oCurrentProduct.Products.Item(i)

            ' Determine if the current node is a part, product or component
            If IsPart(oCurrentTreeNode) = True Then
                Comp.ItemType = "Part"
            ElseIf IsProduct(oCurrentTreeNode) = True Then
                Comp.ItemType = "Product"
            Else
                Comp.ItemType = "Component"
            End If

            If IsSub Then
                Comp.ItemLevel = SubLevel
            Else
                Comp.ItemLevel = Level
            End If

            Comp.ItemName = oCurrentTreeNode.PartNumber
            Comp.Quantity = 1
            If CompList.Any(Function(x) x.ItemName = Comp.ItemName) Then
                CompList.Find(Function(x) x.ItemName = Comp.ItemName).Quantity += 1
            Else
                CompList.Add(Comp)
            End If


            ' if sub-nodes exist below the current tree node, call the sub recursively
            If oCurrentTreeNode.Products.Count > 0 Then
                SubLevel = SubLevel + 1
                GetNextNode(oCurrentTreeNode, SubLevel, True)
            End If

        Next

    End Sub


End Class

Public Class CompTree

    Public Property ItemName As String
    Public Property ItemType As String
    Public Property ItemLevel As String
    Public Property Quantity As Double

End Class

