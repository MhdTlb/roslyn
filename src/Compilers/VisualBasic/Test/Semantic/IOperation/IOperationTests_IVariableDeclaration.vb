﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Microsoft.CodeAnalysis.Test.Utilities
Imports Roslyn.Test.Utilities

Namespace Microsoft.CodeAnalysis.VisualBasic.UnitTests.Semantics
    Partial Public Class IOperationTests
        Inherits SemanticModelTestBase
#Region "Dim Declarations"

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub SingleVariableDeclaration()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1 As Integer'BIND:"Dim i1 As Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1 As Integer')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Dim i1 As Integer'BIND:"Dim i1 As Integer"
            ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub MultipleVariableDeclarations()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1 As Integer, i2 As Integer, b1 As Boolean'BIND:"Dim i1 As Integer, i2 As Integer, b1 As Boolean"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (3 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1 As I ...  As Boolean')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      null
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      null
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'b1 As Boolean')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: b1 As System.Boolean) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'b1')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Dim i1 As Integer, i2 As Integer, b1 As Boolean'BIND:"Dim i1 As Integer, i2 As Integer, b1 As Boolean"
            ~~
BC42024: Unused local variable: 'i2'.
        Dim i1 As Integer, i2 As Integer, b1 As Boolean'BIND:"Dim i1 As Integer, i2 As Integer, b1 As Boolean"
                           ~~
BC42024: Unused local variable: 'b1'.
        Dim i1 As Integer, i2 As Integer, b1 As Boolean'BIND:"Dim i1 As Integer, i2 As Integer, b1 As Boolean"
                                          ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub SingleVariableDeclarationNoType()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Dim i1'BIND:"Dim i1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Dim i1'BIND:"Dim i1"
            ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub


        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub MultipleVariableDeclarationNoTypes()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Dim i1, i2'BIND:"Dim i1, i2"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1, i2')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1, i2')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Dim i1, i2'BIND:"Dim i1, i2"
            ~~
BC42024: Unused local variable: 'i2'.
        Dim i1, i2'BIND:"Dim i1, i2"
                ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub InvalidMultipleVariableDeclaration()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Dim i1 As Integer,'BIND:"Dim i1 As Integer,"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim i1 As Integer,')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      null
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: '')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol:  As System.Object) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: '')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Dim i1 As Integer,'BIND:"Dim i1 As Integer,"
            ~~
BC30203: Identifier expected.
        Dim i1 As Integer,'BIND:"Dim i1 As Integer,"
                          ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub InvalidMultipleVariableDeclarationsNoType()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Dim i1,'BIND:"Dim i1,"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim i1,')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1,')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol:  As System.Object) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: '')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Dim i1,'BIND:"Dim i1,"
            ~~
BC30203: Identifier expected.
        Dim i1,'BIND:"Dim i1,"
               ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub SingleVariableDeclarationLocalReferenceInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1 = 1
        Dim i2 = i1'BIND:"Dim i2 = i1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i2 = i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Int32) (Syntax: 'i1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub MultipleVariableDeclarationLocalReferenceInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1 = 1
        Dim i2 = i1, i3 = i1'BIND:"Dim i2 = i1, i3 = i1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i2 = i1, i3 = i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Int32) (Syntax: 'i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i3 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i3 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i3')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Int32) (Syntax: 'i1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub SingleVariableDeclarationExpressionInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1 = ReturnInt()'BIND:"Dim i1 = ReturnInt()"
    End Sub

    Function ReturnInt() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1 = ReturnInt()')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = ReturnInt()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= ReturnInt()')
        IInvocationOperation (Function Program.ReturnInt() As System.Int32) (OperationKind.Invocation, Type: System.Int32) (Syntax: 'ReturnInt()')
          Instance Receiver: 
            null
          Arguments(0)
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub MultipleVariableDeclarationExpressionInitializers()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1 = ReturnInt(), i2 = ReturnInt()'BIND:"Dim i1 = ReturnInt(), i2 = ReturnInt()"
    End Sub

    Function ReturnInt() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1 = Re ... ReturnInt()')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = ReturnInt()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= ReturnInt()')
        IInvocationOperation (Function Program.ReturnInt() As System.Int32) (OperationKind.Invocation, Type: System.Int32) (Syntax: 'ReturnInt()')
          Instance Receiver: 
            null
          Arguments(0)
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = ReturnInt()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= ReturnInt()')
        IInvocationOperation (Function Program.ReturnInt() As System.Int32) (OperationKind.Invocation, Type: System.Int32) (Syntax: 'ReturnInt()')
          Instance Receiver: 
            null
          Arguments(0)
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub DimAsNew()
            Dim source = <![CDATA[
Module Program
    Class C
    End Class
    Sub Main(args As String())
        Dim p1 As New C'BIND:"Dim p1 As New C"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim p1 As New C')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'p1 As New C')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: p1 As Program.C) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'p1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New C')
        IObjectCreationOperation (Constructor: Sub Program.C..ctor()) (OperationKind.ObjectCreation, Type: Program.C) (Syntax: 'New C')
          Arguments(0)
          Initializer: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub MultipleDimAsNew()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1, i2 As New Integer'BIND:"Dim i1, i2 As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1, i2  ... New Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1, i2 As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Integer')
        IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32) (Syntax: 'New Integer')
          Arguments(0)
          Initializer: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub


        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub DimAsNewNoObject()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1 As New'BIND:"Dim i1 As New"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim i1 As New')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1 As New')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsInvalid) (Syntax: 'New')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30182: Type expected.
        Dim i1 As New'BIND:"Dim i1 As New"
                     ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub MultipleDimAsNewNoObject()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1, i2 As New'BIND:"Dim i1, i2 As New"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim i1, i2 As New')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1, i2 As New')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsInvalid) (Syntax: 'New')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30182: Type expected.
        Dim i1, i2 As New'BIND:"Dim i1, i2 As New"
                         ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub MixedDimAsNewAndEqualsDeclarations()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1, i2 As New Integer, b1 As Boolean = False'BIND:"Dim i1, i2 As New Integer, b1 As Boolean = False"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1, i2  ... ean = False')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1, i2 As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Integer')
        IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32) (Syntax: 'New Integer')
          Arguments(0)
          Initializer: 
            null
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'b1 As Boolean = False')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: b1 As System.Boolean) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'b1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= False')
        ILiteralOperation (OperationKind.Literal, Type: System.Boolean, Constant: False) (Syntax: 'False')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub MixedDimAsNewAndEqualsDeclarationsReversedOrder()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim b1 As Boolean, i1, i2 As New Integer'BIND:"Dim b1 As Boolean, i1, i2 As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim b1 As B ... New Integer')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'b1 As Boolean')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: b1 As System.Boolean) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'b1')
          Initializer: 
            null
    Initializer: 
      null
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1, i2 As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Integer')
        IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32) (Syntax: 'New Integer')
          Arguments(0)
          Initializer: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'b1'.
        Dim b1 As Boolean, i1, i2 As New Integer'BIND:"Dim b1 As Boolean, i1, i2 As New Integer"
            ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ArrayDeclarationWithLength()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1(2) As Integer'BIND:"Dim i1(2) As Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1(2) As Integer')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1(2) As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1(2)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'i1(2)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'i1(2)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 3, IsImplicit) (Syntax: '2')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 2) (Syntax: '2')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '2')
                Initializer: 
                  null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ArrayDeclarationMultipleVariables()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1(), i2 As Integer'BIND:"Dim i1(), i2 As Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim i1(), i2 As Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1(), i2 As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1()')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Dim i1(), i2 As Integer'BIND:"Dim i1(), i2 As Integer"
            ~~
BC42024: Unused local variable: 'i2'.
        Dim i1(), i2 As Integer'BIND:"Dim i1(), i2 As Integer"
                  ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ArrayDeclarationInvalidAsNew()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Dim i1(2) As New Integer'BIND:"Dim i1(2) As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim i1(2) As New Integer')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1(2) As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1(2)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'i1(2)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'i1(2)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 3, IsImplicit) (Syntax: '2')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 2) (Syntax: '2')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '2')
                Initializer: 
                  null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New Integer')
        IInvalidOperation (OperationKind.Invalid, Type: System.Int32(), IsInvalid, IsImplicit) (Syntax: 'As New Integer')
          Children(1):
              IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32, IsInvalid) (Syntax: 'New Integer')
                Arguments(0)
                Initializer: 
                  null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30053: Arrays cannot be declared with 'New'.
        Dim i1(2) As New Integer'BIND:"Dim i1(2) As New Integer"
                     ~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(22362, "https://github.com/dotnet/roslyn/issues/22362")>
        Public Sub ArrayRangeDeclaration()
            Dim source = <![CDATA[
Option Strict On
Imports System.Text

Module M1
    Sub Sub1()
        Dim a(0 To 4) As Integer'BIND:"a(0 To 4) As Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'a(0 To 4) As Integer')
  Declarations:
      ISingleVariableDeclarationOperation (Symbol: a As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'a(0 To 4)')
        Initializer: 
          IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'a(0 To 4)')
            IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'a(0 To 4)')
              Dimension Sizes(1):
                  IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 5, IsImplicit) (Syntax: '0 To 4')
                    Left: 
                      ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 4) (Syntax: '4')
                    Right: 
                      ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '0 To 4')
              Initializer: 
                null
  Initializer: 
    null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of VariableDeclaratorSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(22362, "https://github.com/dotnet/roslyn/issues/22362")>
        Public Sub ArrayDeclarationCollectionInitializer()
            Dim source = <![CDATA[
Option Strict On
Imports System.Text

Module M1
    Sub Sub1()
        Dim s As String() = {"Hello", "World"}'BIND:"s As String() = {"Hello", "World"}"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 's As String ... ", "World"}')
  Declarations:
      ISingleVariableDeclarationOperation (Symbol: s As System.String()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 's')
        Initializer: 
          null
  Initializer: 
    IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= {"Hello", "World"}')
      IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.String()) (Syntax: '{"Hello", "World"}')
        Dimension Sizes(1):
            ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 2, IsImplicit) (Syntax: '{"Hello", "World"}')
        Initializer: 
          IArrayInitializerOperation (2 elements) (OperationKind.ArrayInitializer, Type: null, IsImplicit) (Syntax: '{"Hello", "World"}')
            Element Values(2):
                ILiteralOperation (OperationKind.Literal, Type: System.String, Constant: "Hello") (Syntax: '"Hello"')
                ILiteralOperation (OperationKind.Literal, Type: System.String, Constant: "World") (Syntax: '"World"')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of VariableDeclaratorSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(22362, "https://github.com/dotnet/roslyn/issues/22362")>
        Public Sub PercentTypeSpecifierWithNullableAndInitializer()
            Dim source = <![CDATA[
Option Strict On
Imports System.Text

Module M1
    Sub Sub1()
        Dim d%? = 42'BIND:"d%? = 42"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'd%? = 42')
  Declarations:
      ISingleVariableDeclarationOperation (Symbol: d As System.Nullable(Of System.Int32)) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'd%?')
        Initializer: 
          null
  Initializer: 
    IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 42')
      IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Nullable(Of System.Int32), IsImplicit) (Syntax: '42')
        Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        Operand: 
          ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 42) (Syntax: '42')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of VariableDeclaratorSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(22362, "https://github.com/dotnet/roslyn/issues/22362")>
        Public Sub MultipleIdentifiersWithSingleInitializer_Invalid()
            Dim source = <![CDATA[
Option Strict On
Imports System.Text

Module M1
    Sub Sub1()
        Dim d, x%? = 42'BIND:"d, x%? = 42"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'd, x%? = 42')
  Declarations:
      ISingleVariableDeclarationOperation (Symbol: d As System.Object) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'd')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: x As System.Nullable(Of System.Int32)) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x%?')
        Initializer: 
          null
  Initializer: 
    IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '= 42')
      IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Nullable(Of System.Int32), IsInvalid, IsImplicit) (Syntax: '42')
        Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        Operand: 
          ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 42, IsInvalid) (Syntax: '42')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30209: Option Strict On requires all variable declarations to have an 'As' clause.
        Dim d, x%? = 42'BIND:"d, x%? = 42"
            ~
BC42024: Unused local variable: 'd'.
        Dim d, x%? = 42'BIND:"d, x%? = 42"
            ~
BC30671: Explicit initialization is not permitted with multiple variables declared with a single type specifier.
        Dim d, x%? = 42'BIND:"d, x%? = 42"
            ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of VariableDeclaratorSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub MultipleIdentifiersWithSingleInitializer_Invalid_ManyIdentifiers()
            Dim source = <![CDATA[
Option Strict On
Imports System.Text

Module M1
    Sub Sub1()
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IMultiVariableDeclarationOperation (26 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'a, b, c, d, ... Integer = 1')
  Declarations:
      ISingleVariableDeclarationOperation (Symbol: a As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'a')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: b As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'b')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: c As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'c')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: d As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'd')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: e As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'e')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: f As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'f')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: g As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'g')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: h As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'h')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: i As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: j As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'j')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: k As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'k')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: l As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'l')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: m As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'm')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: n As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'n')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: o As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'o')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: p As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'p')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: q As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'q')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: r As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'r')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: s As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 's')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: t As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 't')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: u As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'u')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: v As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'v')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: w As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'w')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: x As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: y As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'y')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: z As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'z')
        Initializer: 
          null
  Initializer: 
    IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '= 1')
      ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'a'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
            ~
BC30671: Explicit initialization is not permitted with multiple variables declared with a single type specifier.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
            ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
BC42024: Unused local variable: 'b'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
               ~
BC42024: Unused local variable: 'c'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                  ~
BC42024: Unused local variable: 'd'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                     ~
BC42024: Unused local variable: 'e'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                        ~
BC42024: Unused local variable: 'f'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                           ~
BC42024: Unused local variable: 'g'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                              ~
BC42024: Unused local variable: 'h'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                 ~
BC42024: Unused local variable: 'i'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                    ~
BC42024: Unused local variable: 'j'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                       ~
BC42024: Unused local variable: 'k'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                          ~
BC42024: Unused local variable: 'l'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                             ~
BC42024: Unused local variable: 'm'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                ~
BC42024: Unused local variable: 'n'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                   ~
BC42024: Unused local variable: 'o'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                      ~
BC42024: Unused local variable: 'p'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                         ~
BC42024: Unused local variable: 'q'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                            ~
BC42024: Unused local variable: 'r'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                               ~
BC42024: Unused local variable: 's'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                                  ~
BC42024: Unused local variable: 't'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                                     ~
BC42024: Unused local variable: 'u'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                                        ~
BC42024: Unused local variable: 'v'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                                           ~
BC42024: Unused local variable: 'w'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                                              ~
BC42024: Unused local variable: 'x'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                                                 ~
BC42024: Unused local variable: 'y'.
        Dim a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1'BIND:"a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z As Integer = 1"
                                                                                    ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of VariableDeclaratorSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub SingleIdentifierArray_Initializer()
            Dim source = <![CDATA[
Option Strict On
Imports System.Text

Module M1
    Sub Sub1()
        Dim x() As Integer = New Integer() {1, 2, 3, 4}'BIND:"Dim x() As Integer = New Integer() {1, 2, 3, 4}"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim x() As  ... 1, 2, 3, 4}')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'x() As Inte ... 1, 2, 3, 4}')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x()')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= New Integ ... 1, 2, 3, 4}')
        IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32()) (Syntax: 'New Integer ... 1, 2, 3, 4}')
          Dimension Sizes(1):
              ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 4, IsImplicit) (Syntax: 'New Integer ... 1, 2, 3, 4}')
          Initializer: 
            IArrayInitializerOperation (4 elements) (OperationKind.ArrayInitializer, Type: System.Int32()) (Syntax: '{1, 2, 3, 4}')
              Element Values(4):
                  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
                  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 2) (Syntax: '2')
                  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 3) (Syntax: '3')
                  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 4) (Syntax: '4')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub SingleIdentifier_ArrayBoundsAndAsNew()
            Dim source = <![CDATA[
Option Strict On
Imports System.Text

Module M1
    Sub Sub1()
        Dim x(1) As New Integer'BIND:"Dim x(1) As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim x(1) As New Integer')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x(1) As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x(1)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'x(1)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'x(1)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 2, IsImplicit) (Syntax: '1')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '1')
                Initializer: 
                  null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New Integer')
        IInvalidOperation (OperationKind.Invalid, Type: System.Int32(), IsInvalid, IsImplicit) (Syntax: 'As New Integer')
          Children(1):
              IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32, IsInvalid) (Syntax: 'New Integer')
                Arguments(0)
                Initializer: 
                  null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30053: Arrays cannot be declared with 'New'.
        Dim x(1) As New Integer'BIND:"Dim x(1) As New Integer"
                    ~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub SingleIdentifier_ArrayBoundsAndInitializer()
            Dim source = <![CDATA[
Option Strict On
Imports System.Text

Module M1
    Sub Sub1()
        Dim x(1) As Integer = New Integer() {1, 2, 3, 4}'BIND:"Dim x(1) As Integer = New Integer() {1, 2, 3, 4}"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim x(1) As ... 1, 2, 3, 4}')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x(1) As Int ... 1, 2, 3, 4}')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x(1)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid, IsImplicit) (Syntax: 'x(1)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsInvalid, IsImplicit) (Syntax: 'x(1)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 2, IsInvalid, IsImplicit) (Syntax: '1')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsInvalid, IsImplicit) (Syntax: '1')
                Initializer: 
                  null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= New Integ ... 1, 2, 3, 4}')
        IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32()) (Syntax: 'New Integer ... 1, 2, 3, 4}')
          Dimension Sizes(1):
              ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 4, IsImplicit) (Syntax: 'New Integer ... 1, 2, 3, 4}')
          Initializer: 
            IArrayInitializerOperation (4 elements) (OperationKind.ArrayInitializer, Type: System.Int32()) (Syntax: '{1, 2, 3, 4}')
              Element Values(4):
                  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
                  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 2) (Syntax: '2')
                  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 3) (Syntax: '3')
                  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 4) (Syntax: '4')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30672: Explicit initialization is not permitted for arrays declared with explicit bounds.
        Dim x(1) As Integer = New Integer() {1, 2, 3, 4}'BIND:"Dim x(1) As Integer = New Integer() {1, 2, 3, 4}"
            ~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub MultipleIdentifiers_ArrayAndAsNew()
            Dim source = <![CDATA[
Option Strict On

Module M1
    Sub Sub1()
        Dim x(1), y(2) As New Integer'BIND:"Dim x(1), y(2) As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim x(1), y ... New Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x(1), y(2)  ... New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x(1)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'x(1)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'x(1)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 2, IsImplicit) (Syntax: '1')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '1')
                Initializer: 
                  null
        ISingleVariableDeclarationOperation (Symbol: y As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'y(2)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'y(2)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'y(2)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 3, IsImplicit) (Syntax: '2')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 2) (Syntax: '2')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '2')
                Initializer: 
                  null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New Integer')
        IInvalidOperation (OperationKind.Invalid, Type: System.Int32(), IsInvalid, IsImplicit) (Syntax: 'As New Integer')
          Children(1):
              IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32, IsInvalid) (Syntax: 'New Integer')
                Arguments(0)
                Initializer: 
                  null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30053: Arrays cannot be declared with 'New'.
        Dim x(1), y(2) As New Integer'BIND:"Dim x(1), y(2) As New Integer"
                          ~~~
BC30053: Arrays cannot be declared with 'New'.
        Dim x(1), y(2) As New Integer'BIND:"Dim x(1), y(2) As New Integer"
                          ~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub MultipleIdentifiers_MixedArrayAndNonArray()
            Dim source = <![CDATA[
Option Strict On

Module M1
    Sub Sub1()
        Dim x(10), y As Integer'BIND:"Dim x(10), y As Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim x(10), y As Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'x(10), y As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x(10)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'x(10)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'x(10)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 11, IsImplicit) (Syntax: '10')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 10) (Syntax: '10')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '10')
                Initializer: 
                  null
        ISingleVariableDeclarationOperation (Symbol: y As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'y')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'y'.
        Dim x(10), y As Integer'BIND:"Dim x(10), y As Integer"
                   ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub MultipleIdentifiers_MixedArrayAndNonArrayAsNew_ArrayFirst()
            Dim source = <![CDATA[
Option Strict On

Module M1
    Sub Sub1()
        Dim x(10), y As New Integer'BIND:"Dim x(10), y As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim x(10),  ... New Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x(10), y As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x(10)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'x(10)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'x(10)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 11, IsImplicit) (Syntax: '10')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 10) (Syntax: '10')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '10')
                Initializer: 
                  null
        ISingleVariableDeclarationOperation (Symbol: y As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'y')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New Integer')
        IInvalidOperation (OperationKind.Invalid, Type: System.Int32(), IsInvalid, IsImplicit) (Syntax: 'As New Integer')
          Children(1):
              IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32, IsInvalid) (Syntax: 'New Integer')
                Arguments(0)
                Initializer: 
                  null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30053: Arrays cannot be declared with 'New'.
        Dim x(10), y As New Integer'BIND:"Dim x(10), y As New Integer"
                        ~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub MultipleIdentifiers_MixedArrayAndNonArrayAsNew_ArrayLast()
            Dim source = <![CDATA[
Option Strict On

Module M1
    Sub Sub1()
        Dim x, y(10) As New Integer'BIND:"Dim x, y(10) As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim x, y(10 ... New Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x, y(10) As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: y As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'y(10)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'y(10)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'y(10)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 11, IsImplicit) (Syntax: '10')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 10) (Syntax: '10')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '10')
                Initializer: 
                  null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New Integer')
        IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32, IsInvalid) (Syntax: 'New Integer')
          Arguments(0)
          Initializer: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30053: Arrays cannot be declared with 'New'.
        Dim x, y(10) As New Integer'BIND:"Dim x, y(10) As New Integer"
                        ~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub


        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub MultipleIdentifiers_MultipleArrays()
            Dim source = <![CDATA[
Option Strict On

Module M1
    Sub Sub1()
        Dim x%(10), y$(11)'BIND:"Dim x%(10), y$(11)"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim x%(10), y$(11)')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'x%(10), y$(11)')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Int32()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x%(10)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'x%(10)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.Int32(), IsImplicit) (Syntax: 'x%(10)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 11, IsImplicit) (Syntax: '10')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 10) (Syntax: '10')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '10')
                Initializer: 
                  null
        ISingleVariableDeclarationOperation (Symbol: y As System.String()) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'y$(11)')
          Initializer: 
            IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsImplicit) (Syntax: 'y$(11)')
              IArrayCreationOperation (OperationKind.ArrayCreation, Type: System.String(), IsImplicit) (Syntax: 'y$(11)')
                Dimension Sizes(1):
                    IBinaryOperation (BinaryOperatorKind.Add, Checked) (OperationKind.BinaryOperator, Type: System.Int32, Constant: 12, IsImplicit) (Syntax: '11')
                      Left: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 11) (Syntax: '11')
                      Right: 
                        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsImplicit) (Syntax: '11')
                Initializer: 
                  null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

#End Region

#Region "Using Statements"

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact, WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub UsingStatementDeclarationAsNew()
            Dim source = <![CDATA[
Imports System

Module Program
    Class C
        Implements IDisposable
        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Class
    Sub Main(args As String())
        Using c1 As New C'BIND:"Using c1 As New C"
            Console.WriteLine(c1)
        End Using
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IUsingOperation (OperationKind.Using, Type: null) (Syntax: 'Using c1 As ... End Using')
  Resources: 
    IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Using c1 As New C')
      IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'c1 As New C')
        Declarations:
            ISingleVariableDeclarationOperation (Symbol: c1 As Program.C) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'c1')
              Initializer: 
                null
        Initializer: 
          IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New C')
            IObjectCreationOperation (Constructor: Sub Program.C..ctor()) (OperationKind.ObjectCreation, Type: Program.C) (Syntax: 'New C')
              Arguments(0)
              Initializer: 
                null
  Body: 
    IBlockOperation (1 statements) (OperationKind.Block, Type: null, IsImplicit) (Syntax: 'Using c1 As ... End Using')
      IExpressionStatementOperation (OperationKind.ExpressionStatement, Type: null) (Syntax: 'Console.WriteLine(c1)')
        Expression: 
          IInvocationOperation (Sub System.Console.WriteLine(value As System.Object)) (OperationKind.Invocation, Type: System.Void) (Syntax: 'Console.WriteLine(c1)')
            Instance Receiver: 
              null
            Arguments(1):
                IArgumentOperation (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument, Type: null) (Syntax: 'c1')
                  IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Object, IsImplicit) (Syntax: 'c1')
                    Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: True, IsUserDefined: False) (MethodSymbol: null)
                    Operand: 
                      ILocalReferenceOperation: c1 (OperationKind.LocalReference, Type: Program.C) (Syntax: 'c1')
                  InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                  OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of UsingBlockSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub


        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(Skip:="https://github.com/dotnet/roslyn/issues/17917"), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub UsingStatementDeclaration()
            Dim source = <![CDATA[
Module Program
    Class C
        Implements IDisposable
        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Class
    Sub Main(args As String())
        Using c1 As C = New C'BIND:"c1 As C = New C"
            Console.WriteLine(c1)
        End Using
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'c1 As C = New C')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'c1')
    Variables: Local_1: c1 As Program.C
    Initializer: IObjectCreationExpression (Constructor: Sub Program.C..ctor()) (OperationKind.ObjectCreationExpression, Type: Program.C) (Syntax: 'New C')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

#End Region

#Region "Const Declarations"

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstDeclaration()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 As Integer = 1'BIND:"Const i1 As Integer = 1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Const i1 As Integer = 1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 As Integer = 1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 1')
        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42099: Unused local constant: 'i1'.
        Const i1 As Integer = 1'BIND:"Const i1 As Integer = 1"
              ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstMultipleDeclaration()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Const i1 = 1, i2 = 2'BIND:"Const i1 = 1, i2 = 2"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Const i1 = 1, i2 = 2')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = 1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 1')
        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = 2')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 2')
        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 2) (Syntax: '2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42099: Unused local constant: 'i1'.
        Const i1 = 1, i2 = 2'BIND:"Const i1 = 1, i2 = 2"
              ~~
BC42099: Unused local constant: 'i2'.
        Const i1 = 1, i2 = 2'BIND:"Const i1 = 1, i2 = 2"
                      ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstAsNew()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 As New Integer'BIND:"Const i1 As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Const i1 As New Integer')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1 As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New Integer')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsInvalid, IsImplicit) (Syntax: 'i1')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30438: Constants must have a value.
        Const i1 As New Integer'BIND:"Const i1 As New Integer"
              ~~
BC30246: 'New' is not valid on a local constant declaration.
        Const i1 As New Integer'BIND:"Const i1 As New Integer"
                    ~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstAsNewMultipleDeclarations()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1, i2 As New Integer'BIND:"Const i1, i2 As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Const i1, i ... New Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1, i2 As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New Integer')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsInvalid, IsImplicit) (Syntax: 'i1')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30438: Constants must have a value.
        Const i1, i2 As New Integer'BIND:"Const i1, i2 As New Integer"
              ~~
BC42099: Unused local constant: 'i1'.
        Const i1, i2 As New Integer'BIND:"Const i1, i2 As New Integer"
              ~~
BC30438: Constants must have a value.
        Const i1, i2 As New Integer'BIND:"Const i1, i2 As New Integer"
                  ~~
BC30246: 'New' is not valid on a local constant declaration.
        Const i1, i2 As New Integer'BIND:"Const i1, i2 As New Integer"
                        ~~~
BC30246: 'New' is not valid on a local constant declaration.
        Const i1, i2 As New Integer'BIND:"Const i1, i2 As New Integer"
                        ~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstSingleDeclarationNoType()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 = 1'BIND:"Const i1 = 1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Const i1 = 1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = 1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 1')
        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42099: Unused local constant: 'i1'.
        Const i1 = 1'BIND:"Const i1 = 1"
              ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstMultipleDeclarationsNoTypes()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 = 1, i2 = 'BIND:"Const i1 = 1, i2 = "
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Const i1 = 1, i2 = ')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = 1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 1')
        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i2 = ')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '= ')
        IInvalidOperation (OperationKind.Invalid, Type: null, IsInvalid) (Syntax: '')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42099: Unused local constant: 'i1'.
        Const i1 = 1, i2 = 'BIND:"Const i1 = 1, i2 = "
              ~~
BC30201: Expression expected.
        Const i1 = 1, i2 = 'BIND:"Const i1 = 1, i2 = "
                           ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstSingleDeclarationLocalReferenceInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 = 1
        Const i2 = i1'BIND:"Const i2 = i1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Const i2 = i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Int32, Constant: 1) (Syntax: 'i1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42099: Unused local constant: 'i2'.
        Const i2 = i1'BIND:"Const i2 = i1"
              ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstMultipleDeclarationsLocalReferenceInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 = 1
        Const i2 = i1, i3 = i1'BIND:"Const i2 = i1, i3 = i1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Const i2 = i1, i3 = i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Int32, Constant: 1) (Syntax: 'i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i3 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i3 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i3')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Int32, Constant: 1) (Syntax: 'i1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42099: Unused local constant: 'i2'.
        Const i2 = i1, i3 = i1'BIND:"Const i2 = i1, i3 = i1"
              ~~
BC42099: Unused local constant: 'i3'.
        Const i2 = i1, i3 = i1'BIND:"Const i2 = i1, i3 = i1"
                       ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstSingleDeclarationExpressionInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 = Int1()'BIND:"Const i1 = Int1()"
    End Sub

    Function Int1() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Const i1 = Int1()')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1 = Int1()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '= Int1()')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsInvalid, IsImplicit) (Syntax: 'Int1()')
          Children(1):
              IInvocationOperation (Function Program.Int1() As System.Int32) (OperationKind.Invocation, Type: System.Int32, IsInvalid) (Syntax: 'Int1()')
                Instance Receiver: 
                  null
                Arguments(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30059: Constant expression is required.
        Const i1 = Int1()'BIND:"Const i1 = Int1()"
                   ~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstMultipleDeclarationsExpressionInitializers()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 = Int1(), i2 = Int1()'BIND:"Const i1 = Int1(), i2 = Int1()"
    End Sub

    Function Int1() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Const i1 =  ... i2 = Int1()')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1 = Int1()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '= Int1()')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsInvalid, IsImplicit) (Syntax: 'Int1()')
          Children(1):
              IInvocationOperation (Function Program.Int1() As System.Int32) (OperationKind.Invocation, Type: System.Int32, IsInvalid) (Syntax: 'Int1()')
                Instance Receiver: 
                  null
                Arguments(0)
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i2 = Int1()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '= Int1()')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsInvalid, IsImplicit) (Syntax: 'Int1()')
          Children(1):
              IInvocationOperation (Function Program.Int1() As System.Int32) (OperationKind.Invocation, Type: System.Int32, IsInvalid) (Syntax: 'Int1()')
                Instance Receiver: 
                  null
                Arguments(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30059: Constant expression is required.
        Const i1 = Int1(), i2 = Int1()'BIND:"Const i1 = Int1(), i2 = Int1()"
                   ~~~~~~
BC30059: Constant expression is required.
        Const i1 = Int1(), i2 = Int1()'BIND:"Const i1 = Int1(), i2 = Int1()"
                                ~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstDimAsNewNoInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 As New'BIND:"Const i1 As New"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Const i1 As New')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1 As New')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsImplicit) (Syntax: 'i1')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30246: 'New' is not valid on a local constant declaration.
        Const i1 As New'BIND:"Const i1 As New"
                    ~~~
BC30182: Type expected.
        Const i1 As New'BIND:"Const i1 As New"
                       ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstDimAsNewMultipleDeclarationsNoInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1, i2 As New'BIND:"Const i1, i2 As New"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Const i1, i2 As New')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1, i2 As New')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: 'As New')
        IInvalidOperation (OperationKind.Invalid, Type: ?, IsImplicit) (Syntax: 'i1')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42099: Unused local constant: 'i1'.
        Const i1, i2 As New'BIND:"Const i1, i2 As New"
              ~~
BC30246: 'New' is not valid on a local constant declaration.
        Const i1, i2 As New'BIND:"Const i1, i2 As New"
                        ~~~
BC30246: 'New' is not valid on a local constant declaration.
        Const i1, i2 As New'BIND:"Const i1, i2 As New"
                        ~~~
BC30182: Type expected.
        Const i1, i2 As New'BIND:"Const i1, i2 As New"
                           ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub ConstInvalidMultipleDeclaration()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Const i1 = 1,'BIND:"Const i1 = 1,"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Const i1 = 1,')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = 1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 1')
        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: '')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol:  As System.Object) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: '')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid, IsImplicit) (Syntax: '')
        IInvalidOperation (OperationKind.Invalid, Type: null, IsInvalid, IsImplicit) (Syntax: '')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42099: Unused local constant: 'i1'.
        Const i1 = 1,'BIND:"Const i1 = 1,"
              ~~
BC30203: Identifier expected.
        Const i1 = 1,'BIND:"Const i1 = 1,"
                     ~
BC30438: Constants must have a value.
        Const i1 = 1,'BIND:"Const i1 = 1,"
                     ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

#End Region

#Region "Static Declarations"

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticDeclaration()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Static i1 As Integer'BIND:"Static i1 As Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1 As Integer')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Static i1 As Integer'BIND:"Static i1 As Integer"
               ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticMultipleDeclarations()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Static i1, i2 As Integer'BIND:"Static i1, i2 As Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1, i2 As Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1, i2 As Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Static i1, i2 As Integer'BIND:"Static i1, i2 As Integer"
               ~~
BC42024: Unused local variable: 'i2'.
        Static i1, i2 As Integer'BIND:"Static i1, i2 As Integer"
                   ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticAsNewDeclaration()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Static i1 As New Integer'BIND:"Static i1 As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1 As New Integer')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Integer')
        IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32) (Syntax: 'New Integer')
          Arguments(0)
          Initializer: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticMulipleDeclarationAsNew()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Static i1, i2 As New Integer'BIND:"Static i1, i2 As New Integer"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1,  ... New Integer')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1, i2 As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Integer')
        IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32) (Syntax: 'New Integer')
          Arguments(0)
          Initializer: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticMixedAsNewAndEqualsDeclaration()
            Dim source = <![CDATA[
Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main(args As String())
        Static i1, i2 As New Integer, b1 As Boolean = False'BIND:"Static i1, i2 As New Integer, b1 As Boolean = False"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1,  ... ean = False')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1, i2 As New Integer')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Integer')
        IObjectCreationOperation (Constructor: Sub System.Int32..ctor()) (OperationKind.ObjectCreation, Type: System.Int32) (Syntax: 'New Integer')
          Arguments(0)
          Initializer: 
            null
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'b1 As Boolean = False')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: b1 As System.Boolean) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'b1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= False')
        ILiteralOperation (OperationKind.Literal, Type: System.Boolean, Constant: False) (Syntax: 'False')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticSingleDeclarationNoType()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 = 1'BIND:"Static i1 = 1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1 = 1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = 1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 1')
        IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Object, IsImplicit) (Syntax: '1')
          Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
          Operand: 
            ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticMultipleDeclarationsNoTypes()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 = 1, i2 = 2'BIND:"Static i1 = 1, i2 = 2"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1 = 1, i2 = 2')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = 1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 1')
        IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Object, IsImplicit) (Syntax: '1')
          Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
          Operand: 
            ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = 2')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 2')
        IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Object, IsImplicit) (Syntax: '2')
          Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
          Operand: 
            ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 2) (Syntax: '2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticSingleDeclarationLocalReferenceInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 = 1
        Static i2 = i1'BIND:"Static i2 = i1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i2 = i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Object) (Syntax: 'i1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticMultipleDeclarationsLocalReferenceInitializers()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 = 1
        Static i2 = i1, i3 = i1'BIND:"Static i2 = i1, i3 = i1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i2 = i1, i3 = i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Object) (Syntax: 'i1')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i3 = i1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i3 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i3')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= i1')
        ILocalReferenceOperation: i1 (OperationKind.LocalReference, Type: System.Object) (Syntax: 'i1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticSingleDeclarationExpressionInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 = Int1()'BIND:"Static i1 = Int1()"
    End Sub

    Function Int1() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1 = Int1()')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = Int1()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= Int1()')
        IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Object, IsImplicit) (Syntax: 'Int1()')
          Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
          Operand: 
            IInvocationOperation (Function Program.Int1() As System.Int32) (OperationKind.Invocation, Type: System.Int32) (Syntax: 'Int1()')
              Instance Receiver: 
                null
              Arguments(0)
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticMultipleDeclarationsExpressionInitializers()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 = Int1(), i2 = Int1()'BIND:"Static i1 = Int1(), i2 = Int1()"
    End Sub

    Function Int1() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Static i1 = ... i2 = Int1()')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i1 = Int1()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= Int1()')
        IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Object, IsImplicit) (Syntax: 'Int1()')
          Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
          Operand: 
            IInvocationOperation (Function Program.Int1() As System.Int32) (OperationKind.Invocation, Type: System.Int32) (Syntax: 'Int1()')
              Instance Receiver: 
                null
              Arguments(0)
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'i2 = Int1()')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= Int1()')
        IConversionOperation (Implicit, TryCast: False, Unchecked) (OperationKind.Conversion, Type: System.Object, IsImplicit) (Syntax: 'Int1()')
          Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
          Operand: 
            IInvocationOperation (Function Program.Int1() As System.Int32) (OperationKind.Invocation, Type: System.Int32) (Syntax: 'Int1()')
              Instance Receiver: 
                null
              Arguments(0)
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticAsNewSingleDeclarationInvalidInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 As'BIND:"Static i1 As"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Static i1 As')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1 As')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Static i1 As'BIND:"Static i1 As"
               ~~
BC30182: Type expected.
        Static i1 As'BIND:"Static i1 As"
                    ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticAsNewMultipleDeclarationInvalidInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1, i2 As'BIND:"Static i1, i2 As"
    End Sub

    Function Int1() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Static i1, i2 As')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1, i2 As')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: i2 As ?) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Static i1, i2 As'BIND:"Static i1, i2 As"
               ~~
BC42024: Unused local variable: 'i2'.
        Static i1, i2 As'BIND:"Static i1, i2 As"
                   ~~
BC30182: Type expected.
        Static i1, i2 As'BIND:"Static i1, i2 As"
                        ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticSingleDeclarationInvalidInitializer()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 ='BIND:"Static i1 ="
    End Sub

    Function Int1() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Static i1 =')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1 =')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '=')
        IInvalidOperation (OperationKind.Invalid, Type: null, IsInvalid) (Syntax: '')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30201: Expression expected.
        Static i1 ='BIND:"Static i1 ="
                   ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticMultipleDeclarationsInvalidInitializers()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1 =, i2 ='BIND:"Static i1 =, i2 ="
    End Sub

    Function Int1() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (2 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Static i1 =, i2 =')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1 =')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '=')
        IInvalidOperation (OperationKind.Invalid, Type: null, IsInvalid) (Syntax: '')
          Children(0)
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i2 =')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i2 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i2')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '=')
        IInvalidOperation (OperationKind.Invalid, Type: null, IsInvalid) (Syntax: '')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30201: Expression expected.
        Static i1 =, i2 ='BIND:"Static i1 =, i2 ="
                   ~
BC30201: Expression expected.
        Static i1 =, i2 ='BIND:"Static i1 =, i2 ="
                         ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact(), WorkItem(17599, "https://github.com/dotnet/roslyn/issues/17599")>
        Public Sub StaticInvalidMultipleDeclaration()
            Dim source = <![CDATA[
Module Program
    Sub Main(args As String())
        Static i1,'BIND:"Static i1,"
    End Sub

    Function Int1() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Static i1,')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'i1,')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: i1 As System.Object) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'i1')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol:  As System.Object) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: '')
          Initializer: 
            null
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'i1'.
        Static i1,'BIND:"Static i1,"
               ~~
BC30203: Identifier expected.
        Static i1,'BIND:"Static i1,"
                  ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

#End Region

#Region "Initializers"
        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub TestGetOperationForEqualsValueVariableInitializer()
            Dim source = <![CDATA[
Class Test
    Sub M()
        Dim x = 1'BIND:"= 1"
    End Sub
End Class]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: '= 1')
  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of EqualsValueSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub TestGetOperationForEqualsValueVariableInitializerWithMultipleLocals()
            Dim source = <![CDATA[
Class Test
    Sub M()
        Dim x, y = 1'BIND:"= 1"
    End Sub
End Class]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '= 1')
  ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'x'.
        Dim x, y = 1'BIND:"= 1"
            ~
BC30671: Explicit initialization is not permitted with multiple variables declared with a single type specifier.
        Dim x, y = 1'BIND:"= 1"
            ~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of EqualsValueSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub TestGetOperationForEqualsValueVariableDeclarationWithMultipleLocals()
            Dim source = <![CDATA[
Class Test
    Sub M()
        Dim x, y = 1'BIND:"Dim x, y = 1"
    End Sub
End Class]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null, IsInvalid) (Syntax: 'Dim x, y = 1')
  IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x, y = 1')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As System.Object) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'x')
          Initializer: 
            null
        ISingleVariableDeclarationOperation (Symbol: y As System.Int32) (OperationKind.SingleVariableDeclaration, Type: null, IsInvalid) (Syntax: 'y')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null, IsInvalid) (Syntax: '= 1')
        ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC42024: Unused local variable: 'x'.
        Dim x, y = 1'BIND:"Dim x, y = 1"
            ~
BC30671: Explicit initialization is not permitted with multiple variables declared with a single type specifier.
        Dim x, y = 1'BIND:"Dim x, y = 1"
            ~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub TestGetOperationForAsNewVariableInitializer()
            Dim source = <![CDATA[
Class Test
    Sub M()
        Dim x As New Test'BIND:"As New Test"
    End Sub
End Class]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Test')
  IObjectCreationOperation (Constructor: Sub Test..ctor()) (OperationKind.ObjectCreation, Type: Test) (Syntax: 'New Test')
    Arguments(0)
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of AsNewClauseSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub TestGetOperationForAsNewVariableDeclaration()
            Dim source = <![CDATA[
Class Test
    Sub M()
        Dim x As New Test'BIND:"Dim x As New Test"
    End Sub
End Class]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationGroupOperation (1 declarations) (OperationKind.VariableDeclarationGroup, Type: null) (Syntax: 'Dim x As New Test')
  IMultiVariableDeclarationOperation (1 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'x As New Test')
    Declarations:
        ISingleVariableDeclarationOperation (Symbol: x As Test) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x')
          Initializer: 
            null
    Initializer: 
      IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Test')
        IObjectCreationOperation (Constructor: Sub Test..ctor()) (OperationKind.ObjectCreation, Type: Test) (Syntax: 'New Test')
          Arguments(0)
          Initializer: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub TestGetOperationForAsNewVariableInitializerWithMultipleLocals()
            Dim source = <![CDATA[
Class Test
    Sub M()
        Dim x, y As New Test'BIND:"As New Test"
    End Sub
End Class]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Test')
  IObjectCreationOperation (Constructor: Sub Test..ctor()) (OperationKind.ObjectCreation, Type: Test) (Syntax: 'New Test')
    Arguments(0)
    Initializer: 
      null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of AsNewClauseSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub TestGetOperationForAsNewVariableDeclarationWithMultipleLocals()
            Dim source = <![CDATA[
Class Test
    Sub M()
        Dim x, y As New Test'BIND:"x, y As New Test"
    End Sub
End Class]]>.Value

            Dim expectedOperationTree = <![CDATA[
IMultiVariableDeclarationOperation (2 declarations) (OperationKind.MultiVariableDeclaration, Type: null) (Syntax: 'x, y As New Test')
  Declarations:
      ISingleVariableDeclarationOperation (Symbol: x As Test) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'x')
        Initializer: 
          null
      ISingleVariableDeclarationOperation (Symbol: y As Test) (OperationKind.SingleVariableDeclaration, Type: null) (Syntax: 'y')
        Initializer: 
          null
  Initializer: 
    IVariableInitializerOperation (OperationKind.VariableInitializer, Type: null) (Syntax: 'As New Test')
      IObjectCreationOperation (Constructor: Sub Test..ctor()) (OperationKind.ObjectCreation, Type: Test) (Syntax: 'New Test')
        Arguments(0)
        Initializer: 
          null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of VariableDeclaratorSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub
#End Region
    End Class
End Namespace
