﻿' Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Runtime.InteropServices
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic

    ''' <summary>
    ''' Binding info for attribute syntax and expressions that are part of a attribute.
    ''' </summary>
    Friend NotInheritable Class AttributeSemanticModel
        Inherits MemberSemanticModel

        Private Sub New(root As VisualBasicSyntaxNode, binder As Binder, Optional parentSemanticModelOpt As SyntaxTreeSemanticModel = Nothing, Optional speculatedPosition As Integer = 0)
            MyBase.New(root, binder, parentSemanticModelOpt, speculatedPosition)
        End Sub

        ''' <summary>
        ''' Creates an AttributeSemanticModel that allows asking semantic questions about an attribute node.
        ''' </summary>
        Friend Shared Function Create(binder As AttributeBinder) As AttributeSemanticModel
            Return New AttributeSemanticModel(binder.Root, binder)
        End Function

        ''' <summary>
        ''' Creates a speculative AttributeSemanticModel that allows asking semantic questions about an attribute node that did not appear in the original source code.
        ''' </summary>
        Friend Shared Function CreateSpeculative(parentSemanticModel As SyntaxTreeSemanticModel, root As VisualBasicSyntaxNode, binder As Binder, position As Integer) As AttributeSemanticModel
            Debug.Assert(parentSemanticModel IsNot Nothing)
            Debug.Assert(root IsNot Nothing)
            Debug.Assert(binder IsNot Nothing)
            Debug.Assert(binder.IsSemanticModelBinder)

            Return New AttributeSemanticModel(root, binder, parentSemanticModel, position)
        End Function

        Friend Overrides Function Bind(binder As Binder, node As VisualBasicSyntaxNode, diagnostics As DiagnosticBag) As BoundNode
            Debug.Assert(binder.IsSemanticModelBinder)

            Dim boundNode As boundNode

            Select Case node.Kind
                Case SyntaxKind.Attribute
                    boundNode = binder.BindAttribute(DirectCast(node, AttributeSyntax), diagnostics)
                    Return boundNode

                Case SyntaxKind.IdentifierName, SyntaxKind.QualifiedName
                    ' Special binding for attribute type to account for the implicit Attribute suffix.
                    If SyntaxFacts.IsAttributeName(node) Then
                        Dim name = DirectCast(node, NameSyntax)
                        boundNode = binder.BindNamespaceOrTypeExpression(name, diagnostics)
                        Return boundNode
                    End If
            End Select

            boundNode = MyBase.Bind(binder, node, diagnostics)
            Return boundNode
        End Function

        Friend Overrides Function TryGetSpeculativeSemanticModelCore(parentModel As SyntaxTreeSemanticModel, position As Integer, initializer As EqualsValueSyntax, <Out> ByRef speculativeModel As SemanticModel) As Boolean
            speculativeModel = Nothing
            Return False
        End Function

        Friend Overrides Function TryGetSpeculativeSemanticModelCore(parentModel As SyntaxTreeSemanticModel, position As Integer, statement As ExecutableStatementSyntax, <Out> ByRef speculativeModel As SemanticModel) As Boolean
            speculativeModel = Nothing
            Return False
        End Function

        Friend Overrides Function TryGetSpeculativeSemanticModelForMethodBodyCore(parentModel As SyntaxTreeSemanticModel, position As Integer, method As MethodBlockBaseSyntax, <Out> ByRef speculativeModel As SemanticModel) As Boolean
            speculativeModel = Nothing
            Return False
        End Function
    End Class
End Namespace

