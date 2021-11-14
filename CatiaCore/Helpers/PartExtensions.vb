Imports System.Runtime.CompilerServices
Imports HybridShapeTypeLib
Imports INFITF
Imports MECMOD
Imports PARTITF
Imports ProductStructureTypeLib
Imports SPATypeLib

Module PartExtensions

    <Extension>
    Public Function GetNearestFaceReferenceByPoint(wpart As Part, x As Double, y As Double, z As Double) As Face
        'CATIA.HSOSynchronized = False
        'CATIA.RefreshDisplay = False
        Dim CATIA As Application = CatiaSingleton.GetApplication()
        Dim shapeFactory1 As ShapeFactory = wpart.ShapeFactory
        Dim hybridShapeFactory1 As HybridShapeFactory = wpart.HybridShapeFactory
        'Dim Body As Body = wpart.MainBody
        Dim HybridBody1 As HybridBody
        If wpart.HybridBodies.Count <= 0 Then
            HybridBody1 = wpart.HybridBodies.Add()
            HybridBody1.Name = "ReferenceBody"
        Else
            HybridBody1 = wpart.HybridBodies.Item(1)
        End If
        Dim point As HybridShapePointCoord = hybridShapeFactory1.AddNewPointCoord(x, y, z)
        HybridBody1.AppendHybridShape(point)
        wpart.Update()
        Dim selection As Selection = CATIA.ActiveDocument.Selection
        selection.Clear()
        selection.Search("Topology.CGMFace, all")
        Dim pointReference As Reference = wpart.CreateReferenceFromObject(point)
        Dim spaworkbench As Workbench = CATIA.ActiveDocument.GetWorkbench("SPAWorkbench")
        Dim pointmeasure As Measurable = spaworkbench.GetMeasurable(point)
        Dim mindistance As Integer = 308000000
        Dim targetface As Face = Nothing
        For i = 1 To selection.Count
            Dim distance = pointmeasure.GetMinimumDistance(selection.Item(i).Reference)
            If distance < mindistance Then
                targetface = selection.Item(i).Reference
                mindistance = distance
                If mindistance < 0.001 Then Exit For
            End If
        Next
        selection.Clear()
        selection.Add(point)
        selection.Delete()
        GetNearestFaceReferenceByPoint = targetface
        'CATIA.HSOSynchronized = True
        'CATIA.RefreshDisplay = True
    End Function

    <Extension>
    Public Function GetNearestEdgeReferenceByPoint(wpart As Part, x As Double, y As Double, z As Double) As Reference
        'CATIA.HSOSynchronized = False
        'CATIA.RefreshDisplay = False
        Dim CATIA As Application = CatiaSingleton.GetApplication()
        Dim shapeFactory1 As ShapeFactory = wpart.ShapeFactory
        Dim hybridShapeFactory1 As HybridShapeFactory = wpart.HybridShapeFactory
        'Dim Body As Body = wpart.MainBody
        Dim HybridBody1 As HybridBody
        If wpart.HybridBodies.Count <= 0 Then
            HybridBody1 = wpart.HybridBodies.Add()
            HybridBody1.Name = "ReferenceBody"
        Else
            HybridBody1 = wpart.HybridBodies.Item(1)
        End If
        Dim point As HybridShapePointCoord = hybridShapeFactory1.AddNewPointCoord(x, y, z)
        HybridBody1.AppendHybridShape(point)
        wpart.Update()
        Dim selection As Selection = CATIA.ActiveDocument.Selection
        selection.Clear()
        selection.Search("Topology.CGMEdge, all")
        Dim pointReference As Reference = wpart.CreateReferenceFromObject(point)
        Dim spaworkbench As Workbench = CATIA.ActiveDocument.GetWorkbench("SPAWorkbench")
        Dim pointmeasure As Measurable = spaworkbench.GetMeasurable(point)
        Dim mindistance As Integer = 308000000
        Dim targetEdge As Edge = Nothing
        For i = 1 To selection.Count
            Dim distance = pointmeasure.GetMinimumDistance(selection.Item(i).Reference)
            If distance < mindistance Then
                targetEdge = selection.Item(i).Reference
                mindistance = distance
                If mindistance < 0.001 Then Exit For
            End If
        Next
        selection.Clear()
        selection.Add(point)
        selection.Delete()
        GetNearestEdgeReferenceByPoint = targetEdge
        'CATIA.HSOSynchronized = True
        'CATIA.RefreshDisplay = True
    End Function

    <Extension>
    Public Function GetFaceFaceEdgeSelections(document As Document, faceSelection1 As Face, faceSelection2 As Face) As Reference
        Dim searchStr1 As String = faceSelection1.DisplayName
        searchStr1 = Mid(searchStr1, InStr(searchStr1, "Brp:"))
        searchStr1 = Left(searchStr1, InStr(searchStr1, "Cf11:()") + 6)
        Dim searchStr2 As String = faceSelection2.DisplayName
        searchStr2 = Mid(searchStr2, InStr(searchStr2, "Brp:"))
        searchStr2 = Left(searchStr2, InStr(searchStr2, "Cf11:()") + 6)
        Dim selection As Selection = document.Selection
        selection.Clear()
        selection.Search("Topology.CGMEdge, all")
        Dim edgeArr(1000)
        Dim i As Integer = 1
        Dim selRefernce As Reference
        For j = 1 To selection.Count
            Dim selEdge As SelectedElement = selection.Item2(j)
            Dim edgeRef As String = selEdge.Reference.DisplayName
            If (InStr(edgeRef, searchStr1) And InStr(edgeRef, searchStr2)) Then
                'edgeRef = edgeRef.Replace("Selection_", "")
                selRefernce = selEdge.Reference
                Exit For
                'edgeArr(i) = selEdge
                'i = i + 1
            End If
        Next
        edgeArr(0) = i - 1
        GetFaceFaceEdgeSelections = selRefernce
        'GetFaceFaceEdgeSelections = edgeRef
        selection.Clear()
    End Function

    <Extension>
    Public Function AddHybridFaceByPoints(wpart As Part, bodyName As String, constName As String, point1 As CaPoint3D, point2 As CaPoint3D, point3 As CaPoint3D)
        Dim CATIA As Application = CatiaSingleton.GetApplication()
        Dim shpFac As ShapeFactory = wpart.ShapeFactory
        Dim hypFact As HybridShapeFactory = wpart.HybridShapeFactory
        Dim hypBody As HybridBody = wpart.HybridBodies.GetItem(bodyName)
        If hypBody Is Nothing Then
            hypBody = wpart.HybridBodies.Add()
            hypBody.Name = bodyName
        End If
        Dim hyPoint1 As HybridShapePointCoord = hypFact.AddNewPointCoord(point1.X, point1.Y, point1.Z)
        hypBody.AppendHybridShape(hyPoint1)
        Dim hyPoint2 As HybridShapePointCoord = hypFact.AddNewPointCoord(point2.X, point2.Y, point2.Z)
        hypBody.AppendHybridShape(hyPoint2)
        Dim hyPoint3 As HybridShapePointCoord = hypFact.AddNewPointCoord(point3.X, point3.Y, point3.Z)
        hypBody.AppendHybridShape(hyPoint3)
        Dim absPlane = hypFact.AddNewPlane3Points(
            wpart.CreateReferenceFromGeometry(hyPoint1), wpart.CreateReferenceFromGeometry(hyPoint2),
            wpart.CreateReferenceFromGeometry(hyPoint3))
        'absPlane.Name = "PlaneContact:body_to_pipe"
        absPlane.Name = constName

    End Function



End Module
