Imports System.Windows.Forms

''' <summary>
''' Professional tool strip renderer that draws disabled Fluent icons as IconService prepared them
''' (already muted in the theme's colours) instead of WinForms' generic grey-out.
''' </summary>
Public Class IconToolStripRenderer
    Inherits ToolStripProfessionalRenderer

    Protected Overrides Sub OnRenderItemImage(e As ToolStripItemImageRenderEventArgs)
        If Not e.Item.Enabled AndAlso e.Image IsNot Nothing AndAlso IconService.IsBound(e.Item) Then
            e.Graphics.DrawImage(e.Image, e.ImageRectangle)
        Else
            MyBase.OnRenderItemImage(e)
        End If
    End Sub

End Class
