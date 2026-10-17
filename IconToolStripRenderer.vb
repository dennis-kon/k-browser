Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Professional tool strip renderer used for every manager-rendered strip. It draws disabled Fluent
''' icons as IconService prepared them (already muted in the theme's colours) instead of WinForms'
''' generic grey-out, and in dark mode uses dark hover/pressed/checked/menu backgrounds so light
''' icons stay visible.
''' </summary>
Public Class IconToolStripRenderer
    Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New ThemedColorTable())
    End Sub

    Protected Overrides Sub OnRenderItemImage(e As ToolStripItemImageRenderEventArgs)
        If Not e.Item.Enabled AndAlso e.Image IsNot Nothing AndAlso IconService.IsBound(e.Item) Then
            e.Graphics.DrawImage(e.Image, e.ImageRectangle)
        Else
            MyBase.OnRenderItemImage(e)
        End If
    End Sub

    ''' <summary>
    ''' Standard professional colours in light mode; dark equivalents while ThemeManager.IsDarkMode,
    ''' read at paint time so toggling the theme needs no renderer swap.
    ''' </summary>
    Private NotInheritable Class ThemedColorTable
        Inherits ProfessionalColorTable

        Private Shared ReadOnly Hover As Color = Color.FromArgb(55, 68, 95)
        Private Shared ReadOnly Pressed As Color = Color.FromArgb(70, 86, 118)
        Private Shared ReadOnly Checked As Color = Color.FromArgb(58, 74, 40)
        Private Shared ReadOnly HoverBorder As Color = Color.FromArgb(90, 110, 145)

        Private Shared Function Pick(dark As Color, light As Color) As Color
            Return If(ThemeManager.IsDarkMode, dark, light)
        End Function

        Public Overrides ReadOnly Property ButtonSelectedHighlight As Color
            Get
                Return Pick(Hover, MyBase.ButtonSelectedHighlight)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonSelectedGradientBegin As Color
            Get
                Return Pick(Hover, MyBase.ButtonSelectedGradientBegin)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle As Color
            Get
                Return Pick(Hover, MyBase.ButtonSelectedGradientMiddle)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonSelectedGradientEnd As Color
            Get
                Return Pick(Hover, MyBase.ButtonSelectedGradientEnd)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonSelectedBorder As Color
            Get
                Return Pick(HoverBorder, MyBase.ButtonSelectedBorder)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonPressedHighlight As Color
            Get
                Return Pick(Pressed, MyBase.ButtonPressedHighlight)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonPressedGradientBegin As Color
            Get
                Return Pick(Pressed, MyBase.ButtonPressedGradientBegin)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonPressedGradientMiddle As Color
            Get
                Return Pick(Pressed, MyBase.ButtonPressedGradientMiddle)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonPressedGradientEnd As Color
            Get
                Return Pick(Pressed, MyBase.ButtonPressedGradientEnd)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonPressedBorder As Color
            Get
                Return Pick(HoverBorder, MyBase.ButtonPressedBorder)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonCheckedHighlight As Color
            Get
                Return Pick(Checked, MyBase.ButtonCheckedHighlight)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonCheckedGradientBegin As Color
            Get
                Return Pick(Checked, MyBase.ButtonCheckedGradientBegin)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonCheckedGradientMiddle As Color
            Get
                Return Pick(Checked, MyBase.ButtonCheckedGradientMiddle)
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonCheckedGradientEnd As Color
            Get
                Return Pick(Checked, MyBase.ButtonCheckedGradientEnd)
            End Get
        End Property
        Public Overrides ReadOnly Property CheckBackground As Color
            Get
                Return Pick(Checked, MyBase.CheckBackground)
            End Get
        End Property
        Public Overrides ReadOnly Property CheckSelectedBackground As Color
            Get
                Return Pick(Checked, MyBase.CheckSelectedBackground)
            End Get
        End Property
        Public Overrides ReadOnly Property CheckPressedBackground As Color
            Get
                Return Pick(Checked, MyBase.CheckPressedBackground)
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelected As Color
            Get
                Return Pick(Hover, MyBase.MenuItemSelected)
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
            Get
                Return Pick(Hover, MyBase.MenuItemSelectedGradientBegin)
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
            Get
                Return Pick(Hover, MyBase.MenuItemSelectedGradientEnd)
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
            Get
                Return Pick(Pressed, MyBase.MenuItemPressedGradientBegin)
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
            Get
                Return Pick(Pressed, MyBase.MenuItemPressedGradientMiddle)
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
            Get
                Return Pick(Pressed, MyBase.MenuItemPressedGradientEnd)
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemBorder As Color
            Get
                Return Pick(HoverBorder, MyBase.MenuItemBorder)
            End Get
        End Property
        Public Overrides ReadOnly Property MenuBorder As Color
            Get
                Return Pick(ThemeManager.DarkBorder, MyBase.MenuBorder)
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
            Get
                Return Pick(ThemeManager.DarkPanelBg, MyBase.ImageMarginGradientBegin)
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
            Get
                Return Pick(ThemeManager.DarkPanelBg, MyBase.ImageMarginGradientMiddle)
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
            Get
                Return Pick(ThemeManager.DarkPanelBg, MyBase.ImageMarginGradientEnd)
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
            Get
                Return Pick(ThemeManager.DarkPanelBg, MyBase.ToolStripDropDownBackground)
            End Get
        End Property
        Public Overrides ReadOnly Property SeparatorDark As Color
            Get
                Return Pick(ThemeManager.DarkBorder, MyBase.SeparatorDark)
            End Get
        End Property
        Public Overrides ReadOnly Property SeparatorLight As Color
            Get
                Return Pick(ThemeManager.DarkBorder, MyBase.SeparatorLight)
            End Get
        End Property
    End Class

End Class
