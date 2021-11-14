Imports HybridShapeTypeLib
Imports KnowledgewareTypeLib
Imports NavigatorTypeLib
Imports ProductStructureTypeLib
Imports SURFACEMACHINING
Imports PROCESSITF
Imports GenKwe
Imports INFITF
Imports MECMOD
Imports PARTITF
Imports SPATypeLib

Public Class Initialization

    Public Shared Sub CreateAngle(XLength As Double, YLength As Double, width As Double, thickness As Double,
                                  pipeHoleRad As Double, X1 As Double, X2 As Double, boltHolesRad As Double,
                                  R1 As Double, R2 As Double, targetFolder As String, fileName As String)
        Dim CATIA As Application = CatiaSingleton.GetApplication()

        Dim oPartDoc As PartDocument = CATIA.Documents.Add(CaDocType.Part)
        Dim oPart As Part = oPartDoc.Part
        Dim oPartBody As Body = oPart.MainBody
        Dim oPlaneXY As Reference = oPart.CreateReferenceFromGeometry(oPart.OriginElements.PlaneXY)
        Dim oPlaneYZ As Reference = oPart.CreateReferenceFromGeometry(oPart.OriginElements.PlaneYZ)

        Dim oLine As Line2D
        Dim oShapeFactory As ShapeFactory = oPart.ShapeFactory
        Dim oCurrentSketch As Sketch = oPartBody.Sketches.Add(oPlaneYZ)
        Dim oFactory2D As Factory2D = oCurrentSketch.OpenEdition()
        oLine = oFactory2D.CreateLine(0, 0, XLength, 0)
        oLine = oFactory2D.CreateLine(XLength, 0, XLength, thickness)
        oLine = oFactory2D.CreateLine(XLength, thickness, thickness, thickness)
        oLine = oFactory2D.CreateLine(thickness, thickness, thickness, YLength)
        oLine = oFactory2D.CreateLine(thickness, YLength, 0, YLength)
        oLine = oFactory2D.CreateLine(0, YLength, 0, 0)
        oCurrentSketch.CloseEdition()
        Dim oPad As Pad = oShapeFactory.AddNewPad(oCurrentSketch, width)
        oPart.Update()

        Dim oHoleFace As Face = GetNearestFaceReferenceByPoint(oPart, width / 2, thickness, YLength / 2)
        Dim oHoleFaceRef As Reference = oPart.CreateReferenceFromName(oHoleFace.DisplayName)
        oPart.InWorkObject = oPartBody
        oCurrentSketch = oPartBody.Sketches.Add(oHoleFaceRef)
        oCurrentSketch.Name = "PipeHole Sketch"
        Dim oFactory2D1 As Factory2D = oCurrentSketch.OpenEdition()
        Dim oCircle1 As Circle2D = oFactory2D1.CreateClosedCircle(YLength - (pipeHoleRad * 2), width / 2, pipeHoleRad)
        oCurrentSketch.CloseEdition()
        Dim oPocket1 As Pocket = oShapeFactory.AddNewPocket(oCurrentSketch, 200.0)
        oPocket1.DirectionOrientation = CatPrismOrientation.catRegularOrientation
        oPocket1.FirstLimit.LimitMode = CatLimitMode.catUpToNextLimit
        oPart.Update()

        Dim oBoltFace As Face = GetNearestFaceReferenceByPoint(oPart, width / 2, XLength / 2, thickness)
        Dim oBoltFaceRef As Reference = oPart.CreateReferenceFromName(oBoltFace.DisplayName)
        oPart.InWorkObject = oPartBody
        oCurrentSketch = oPartBody.Sketches.Add(oBoltFaceRef)
        oCurrentSketch.Name = "BoltHole Sketch"
        oFactory2D1 = oCurrentSketch.OpenEdition()
        oCircle1 = oFactory2D1.CreateClosedCircle(X2, X1, boltHolesRad)
        oCircle1 = oFactory2D1.CreateClosedCircle(X2, width - X1, boltHolesRad)
        oCurrentSketch.CloseEdition()
        oPocket1 = oShapeFactory.AddNewPocket(oCurrentSketch, 200.0)
        oPocket1.DirectionOrientation = CatPrismOrientation.catInverseOrientation
        oPocket1.FirstLimit.LimitMode = CatLimitMode.catUpToNextLimit
        oPart.Update()

        ' Define the fillet to be created with the first Inner edge
        oPart.SimpleFillet(thickness, New CaPoint3D(width / 2, thickness, thickness))
        ' Define the fillet to be created with the first Outer edge
        oPart.SimpleFillet(thickness * 2, New CaPoint3D(width / 2, 0, 0))

        ' Define the fillet to be created with the Horizontal section of angle edge
        oPart.SimpleFillet(R2, New CaPoint3D(width, XLength, thickness / 2), New CaPoint3D(0, XLength, thickness / 2))
        ' Define the fillet to be created with the VErtical section of angle edge
        oPart.SimpleFillet(R1, New CaPoint3D(width, thickness / 2, YLength), New CaPoint3D(0, thickness / 2, YLength))

        'Save Face Id for Constraint in Assembly
        AngleVertFace = oHoleFace.DisplayName
        'Dim InnerHoleFace As Face = oPart.GetNearestFaceReferenceByPoint(thickness / 2, YLength - pipeHoleRad, -width / 2)
        Dim InnerHoleFace As Face = oPart.GetNearestFaceReferenceByPoint(width / 2, thickness / 2, YLength - pipeHoleRad)
        AnglePipeHoleFace = InnerHoleFace.DisplayName

        'Save the Document
        oPartDoc.Product.PartNumber = fileName
        oPartDoc.SaveDoc(targetFolder, fileName, Doctype.Part)
    End Sub

    Public Shared Sub CreatePipe(OutsideDia As Double, InsideDia As Double, Length As Double, targetFolder As String, fileName As String)

        Dim CATIA As Application = CatiaSingleton.GetApplication()

        Dim oPartDoc As PartDocument = CATIA.Documents.Add(CaDocType.Part)
        Dim oPart As Part = oPartDoc.Part
        Dim oPartBody As Body = oPart.MainBody
        Dim oPlaneXY As Reference = oPart.CreateReferenceFromGeometry(oPart.OriginElements.PlaneXY)
        Dim oPlaneYZ As Reference = oPart.CreateReferenceFromGeometry(oPart.OriginElements.PlaneYZ)
        Dim oPlaneZX As Reference = oPart.CreateReferenceFromGeometry(oPart.OriginElements.PlaneZX)

        Dim oCircle As Circle2D
        Dim oShapeFactory As ShapeFactory = oPart.ShapeFactory
        Dim oCurrentSketch As Sketch = oPartBody.Sketches.Add(oPlaneZX)
        Dim oFactory2D As Factory2D = oCurrentSketch.OpenEdition()
        oCircle = oFactory2D.CreateClosedCircle(0, 0, OutsideDia / 2)
        oCircle = oFactory2D.CreateClosedCircle(0, 0, InsideDia / 2)
        oCurrentSketch.CloseEdition()
        Dim oPad As Pad = oShapeFactory.AddNewPad(oCurrentSketch, Length)
        oPart.Update()

        'Save Face Id for Constraint in Assembly
        Dim OuterFace As Face = oPart.GetNearestFaceReferenceByPoint(OutsideDia / 2, Length / 2, 0)
        Dim pcd As Double = (OutsideDia - InsideDia) / 4
        Dim RightFace As Face = oPart.GetNearestFaceReferenceByPoint((InsideDia / 2) + pcd, Length, 0)
        PipeOuterFace = OuterFace.DisplayName
        PipeRgtFace = RightFace.DisplayName

        'Save the Document
        oPartDoc.Product.PartNumber = fileName
        oPartDoc.SaveDoc(targetFolder, fileName, Doctype.Part)
    End Sub

    Shared AnglePipeHoleFace As String = ""
    Shared AngleVertFace As String = ""
    Shared PipeOuterFace As String = ""
    Shared PipeRgtFace As String = ""

    Public Shared Sub CreateAssm(targetFolder As String, angleFile As String, pipeFile As String, assyName As String)

        Dim CATIA As Application = CatiaSingleton.GetApplication()
        Dim oProductDoc As ProductDocument = CATIA.Documents.Add(CaDocType.Product)
        Dim oProduct As Product = oProductDoc.Product

        Dim oAngleDoc As PartDocument = CATIA.Documents.Item($"{angleFile}{DocumentManager.GetFileExtn(Doctype.Part)}")
        Dim oAngle As Product = oProduct.Products.AddComponent(oAngleDoc.Product)

        Dim oPipeDoc As PartDocument = CATIA.Documents.Item($"{pipeFile}{DocumentManager.GetFileExtn(Doctype.Part)}")
        Dim oPipe As Product = oProduct.Products.AddComponent(oPipeDoc.Product)

        Dim oConsts As Constraints = oProduct.Connections("CATIAConstraints")

        Dim HoleRef1 As Reference = oProduct.CreateReferenceFromName(oProduct.Name & "/" & oAngle.Name & "/!" & AnglePipeHoleFace)
        Dim HoleRef2 As Reference = oProduct.CreateReferenceFromName(oProduct.Name & "/" & oPipe.Name & "/!" & PipeOuterFace)
        Dim oConstraint1 As Constraint = oConsts.AddBiEltCst(CatConstraintType.catCstTypeSurfContact, HoleRef1, HoleRef2)
        oProduct.Update()

        Dim FaceRef1 As Reference = oProduct.CreateReferenceFromName(oProduct.Name & "/" & oAngle.Name & "/!" & AngleVertFace)
        Dim FaceRef2 As Reference = oProduct.CreateReferenceFromName(oProduct.Name & "/" & oPipe.Name & "/!" & PipeRgtFace)
        Dim oConstraint2 As Constraint = oConsts.AddBiEltCst(CatConstraintType.catCstTypeDistance, FaceRef1, FaceRef2)
        oConstraint2.Dimension.Value = 10
        oConstraint2.Orientation = CatConstraintOrientation.catCstOrientSame
        oProduct.Update()

        'Save the Document
        oProductDoc.Product.PartNumber = assyName
        oProductDoc.SaveDoc(targetFolder, assyName, Doctype.Product)

    End Sub

    Public Shared Sub GetAssemTree()

        Dim CATIA As Application = CatiaSingleton.GetApplication()

        'oProductDoc.Product.ExtractBOM(CatFileType.catFileTypeHTML, "D:\Works\CAD\Solidworks\4 - AnglePipe\Assy.txt")
        Dim oProductDoc As ProductDocument = CATIA.ActiveDocument
        GetNextNode(oProductDoc.Product, 1, False)

    End Sub

    Shared Comps As List(Of CompTree) = New List(Of CompTree)()

    Shared Sub GetNextNode(oCurrentProduct As Product, SubLevel As Integer, IsSub As Boolean)

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
            If Comps.Any(Function(x) x.ItemName = Comp.ItemName) Then
                Comps.Find(Function(x) x.ItemName = Comp.ItemName).Quantity += 1
            Else
                Comps.Add(Comp)
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
