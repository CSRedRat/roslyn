﻿' Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Collections.Immutable
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic.Symbols
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic.Symbols

    ''' <summary>
    ''' This class represents a base class for compiler generated methods
    ''' </summary>
    Friend Class SynthesizedSimpleMethodSymbol
        Inherits SynthesizedRegularMethodBase

        Private _parameters As ImmutableArray(Of ParameterSymbol)
        Private ReadOnly _overridenMethod As MethodSymbol
        Private ReadOnly _interfaceMethods As ImmutableArray(Of MethodSymbol)
        Private ReadOnly _isOverloads As Boolean
        Private ReadOnly _returnType As TypeSymbol

        Public Sub New(container As NamedTypeSymbol,
                       name As String,
                       returnType As TypeSymbol,
                       Optional overridenMethod As MethodSymbol = Nothing,
                       Optional interfaceMethod As MethodSymbol = Nothing,
                       Optional isOverloads As Boolean = False)

            MyBase.New(VisualBasicSyntaxTree.Dummy.GetRoot(), container, name)
            Me._returnType = returnType
            Me._overridenMethod = overridenMethod
            Me._isOverloads = isOverloads
            Me._interfaceMethods = If(interfaceMethod Is Nothing,
                                      ImmutableArray(Of MethodSymbol).Empty,
                                      ImmutableArray.Create(Of MethodSymbol)(interfaceMethod))
        End Sub

        Public Overrides ReadOnly Property IsOverloads As Boolean
            Get
                Return Me._isOverloads
            End Get
        End Property

        Public Overrides ReadOnly Property IsOverrides As Boolean
            Get
                Return Me._overridenMethod IsNot Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property OverriddenMethod As MethodSymbol
            Get
                Return Me._overridenMethod
            End Get
        End Property

        Public Overrides ReadOnly Property DeclaredAccessibility As Accessibility
            Get
                Return Accessibility.Public
            End Get
        End Property

        Public Overrides ReadOnly Property ExplicitInterfaceImplementations As ImmutableArray(Of MethodSymbol)
            Get
                Return Me._interfaceMethods
            End Get
        End Property

        Public Overrides ReadOnly Property IsSub As Boolean
            Get
                Return Me._returnType.IsVoidType
            End Get
        End Property

        Public Overrides ReadOnly Property ReturnType As TypeSymbol
            Get
                Return Me._returnType
            End Get
        End Property

        ' Note: This should be called at most once, immediately after the symbol is constructed. The parameters aren't 
        ' Note: passed to the constructor because they need to have their container set correctly.
        ''' <summary>
        ''' Sets the parameters.
        ''' </summary>
        ''' <param name="parameters">The parameters.</param>
        Friend Sub SetParameters(parameters As ImmutableArray(Of ParameterSymbol))
            Debug.Assert(Not parameters.IsDefault)
            Debug.Assert(_parameters.IsDefault)

            Me._parameters = parameters
        End Sub

        Friend NotOverridable Overrides ReadOnly Property ParameterCount As Integer
            Get
                Return Me._parameters.Length
            End Get
        End Property

        Public Overrides ReadOnly Property Parameters As ImmutableArray(Of ParameterSymbol)
            Get
                Return Me._parameters
            End Get
        End Property

    End Class

End Namespace