Imports System.Runtime.CompilerServices
Imports INFITF
Imports MECMOD
Imports PARTITF

Public Module PartHelpers

    <Extension>
    Friend Sub SimpleFillet(oPart As Part, constRadius As Double, ParamArray points() As CaPoint3D)

        Dim references As List(Of Reference) = New List(Of Reference)()
        For Each point As CaPoint3D In points
            Dim oEdge As Reference = oPart.GetNearestEdgeReferenceByPoint(point.X, point.Y, point.Z)
            If oEdge Is Nothing Then
                Continue For
            End If
            references.Add(oEdge)
        Next

        Dim i As Integer = 1
        Dim oEdgeFillet As ConstRadEdgeFillet

        For Each edge As Reference In references
            If i = 1 Then
                ' Define the fillet to be created with the first edge
                Dim oShapeFactory As ShapeFactory = oPart.ShapeFactory
                oEdgeFillet = oShapeFactory.AddNewEdgeFilletWithConstantRadius(edge, CatFilletEdgePropagation.catTangencyFilletEdgePropagation, constRadius)
            Else
                ' Add the others edges to be filleted
                oEdgeFillet.AddObjectToFillet(edge)
            End If
            i = i + 1
        Next

        oPart.Update()
    End Sub

End Module
