Imports System.Collections.Generic

''' <summary>
''' High-performance Prefix Tree (Trie) data structure for O(K) host-anchored ad rule lookups.
''' </summary>
Public Class AdBlockTrie

    Private Class TrieNode
        Public Property Children As New Dictionary(Of String, TrieNode)(StringComparer.OrdinalIgnoreCase)
        Public Property IsBlocked As Boolean = False
    End Class

    Private ReadOnly Root As New TrieNode()
    Private _count As Integer = 0

    Public ReadOnly Property RuleCount As Integer
        Get
            Return _count
        End Get
    End Property

    Public Sub Clear()
        Root.Children.Clear()
        _count = 0
    End Sub

    ''' <summary>
    ''' Adds a domain rule (e.g. "doubleclick.net" or "adservice.google.com") into the host Trie.
    ''' </summary>
    Public Sub AddDomain(ByVal domain As String)
        If String.IsNullOrWhiteSpace(domain) Then Return

        ' Normalize domain: trim leading dots and lower-case
        Dim cleanDomain As String = domain.Trim("."c, "/"c, " "c).ToLowerInvariant()
        If String.IsNullOrEmpty(cleanDomain) Then Return

        ' Split host labels in reverse (e.g. "ad.doubleclick.net" -> ["net", "doubleclick", "ad"])
        Dim parts As String() = cleanDomain.Split("."c)
        Array.Reverse(parts)

        Dim current As TrieNode = Root
        For Each part In parts
            If Not current.Children.ContainsKey(part) Then
                current.Children(part) = New TrieNode()
            End If
            current = current.Children(part)
        Next

        If Not current.IsBlocked Then
            current.IsBlocked = True
            _count += 1
        End If
    End Sub

    ''' <summary>
    ''' Evaluates whether a host (e.g. "sub.adservice.google.com") matches any blocked host rule in O(K) time.
    ''' </summary>
    Public Function IsHostBlocked(ByVal host As String) As Boolean
        If String.IsNullOrWhiteSpace(host) OrElse _count = 0 Then Return False

        Dim cleanHost As String = host.Trim("."c, "/"c, " "c).ToLowerInvariant()
        Dim parts As String() = cleanHost.Split("."c)
        Array.Reverse(parts)

        Dim current As TrieNode = Root
        For Each part In parts
            If current.IsBlocked Then
                ' Subdomain match under a blocked parent host (e.g. "bad.com" blocks "sub.bad.com")
                Return True
            End If

            If Not current.Children.TryGetValue(part, current) Then
                Return False
            End If
        Next

        ' Exact host match
        Return current.IsBlocked
    End Function

End Class
