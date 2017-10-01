' *********************************************************************
' PLEASE DO NOT REMOVE THIS DISCLAIMER
'
' WpfPropertyGrid - By Jaime Olivares
' July 11, 2011
' Article site: http://www.codeproject.com/KB/grid/WpfPropertyGrid.aspx
' Author site: www.jaimeolivares.com
' License: Code Project Open License (CPOL)
'
' *********************************************************************

Imports System.Activities.Presentation
Imports System.Activities.Presentation.Model
Imports System.Activities.Presentation.View
Imports System.ComponentModel
Imports System.Reflection
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Namespace Global.System.Windows.Controls
    Public Enum PropertySort
        NoSort = 0
        Alphabetical = 1
        Categorized = 2
        CategorizedAlphabetical = 3
    End Enum

    ''' <summary>WPF Native PropertyGrid class, uses Workflow Foundation's PropertyInspector</summary>
    Public Class WpfPropertyGrid
        Inherits Grid
#Region "Private fields"
        Private Designer As WorkflowDesigner
        Private RefreshMethod As MethodInfo
        Private OnSelectionChangedMethod As MethodInfo
        Private IsInAlphaViewMethod As MethodInfo
        Private SelectionTypeLabel As TextBlock
        Private PropertyToolBar As Control
        Private HelpText As Border
        Private Splitter As GridSplitter
        Private HelpTextHeight As Double = 60
#End Region

#Region "Public properties"
        ''' <summary>Get or sets the selected object. Can be null.</summary>
        Public Property SelectedObject() As Object
            Get
                Return GetValue(SelectedObjectProperty)
            End Get
            Set
                SetValue(SelectedObjectProperty, Value)
            End Set
        End Property
        ''' <summary>Get or sets the selected object collection. Returns empty array by default.</summary>
        Public Property SelectedObjects() As Object()
            Get
                Return TryCast(GetValue(SelectedObjectsProperty), Object())
            End Get
            Set
                SetValue(SelectedObjectsProperty, Value)
            End Set
        End Property
        ''' <summary>XAML information with PropertyGrid's font and color information</summary>
        ''' <seealso>Documentation for WorkflowDesigner.PropertyInspectorFontAndColorData</seealso>
        Public WriteOnly Property FontAndColorData() As String
            Set
                Designer.PropertyInspectorFontAndColorData = Value
            End Set
        End Property
        ''' <summary>Shows the description area on the top of the control</summary>
        Public Property HelpVisible() As Boolean
            Get
                Return CBool(GetValue(HelpVisibleProperty))
            End Get
            Set
                SetValue(HelpVisibleProperty, Value)
            End Set
        End Property
        ''' <summary>Shows the tolbar on the top of the control</summary>
        Public Property ToolbarVisible() As Boolean
            Get
                Return CBool(GetValue(ToolbarVisibleProperty))
            End Get
            Set
                SetValue(ToolbarVisibleProperty, Value)
            End Set
        End Property
        Public Property PropertySort() As PropertySort
            Get
                Return CType(GetValue(PropertySortProperty), PropertySort)
            End Get
            Set
                SetValue(PropertySortProperty, Value)
            End Set
        End Property
#End Region

#Region "Dependency properties registration"
        Public Shared ReadOnly SelectedObjectProperty As DependencyProperty = DependencyProperty.Register("SelectedObject", GetType(Object), GetType(WpfPropertyGrid), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AddressOf SelectedObjectPropertyChanged))

        Public Shared ReadOnly SelectedObjectsProperty As DependencyProperty = DependencyProperty.Register("SelectedObjects", GetType(Object()), GetType(WpfPropertyGrid), New FrameworkPropertyMetadata(New Object(-1) {}, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AddressOf SelectedObjectsPropertyChanged, AddressOf CoerceSelectedObjects))

        Public Shared ReadOnly HelpVisibleProperty As DependencyProperty = DependencyProperty.Register("HelpVisible", GetType(Boolean), GetType(WpfPropertyGrid), New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AddressOf HelpVisiblePropertyChanged))
        Public Shared ReadOnly ToolbarVisibleProperty As DependencyProperty = DependencyProperty.Register("ToolbarVisible", GetType(Boolean), GetType(WpfPropertyGrid), New FrameworkPropertyMetadata(True, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AddressOf ToolbarVisiblePropertyChanged))
        Public Shared ReadOnly PropertySortProperty As DependencyProperty = DependencyProperty.Register("PropertySort", GetType(PropertySort), GetType(WpfPropertyGrid), New FrameworkPropertyMetadata(PropertySort.CategorizedAlphabetical, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AddressOf PropertySortPropertyChanged))
#End Region

#Region "Dependency properties events"
        Private Shared Function CoerceSelectedObject(d As DependencyObject, value As Object) As Object
            Dim pg As WpfPropertyGrid = TryCast(d, WpfPropertyGrid)

            Dim collection As Object() = TryCast(pg.GetValue(SelectedObjectsProperty), Object())

            Return If(collection.Length = 0, Nothing, value)
        End Function
        Private Shared Function CoerceSelectedObjects(d As DependencyObject, value As Object) As Object
            Dim pg As WpfPropertyGrid = TryCast(d, WpfPropertyGrid)

            Dim [single] As Object = pg.GetValue(SelectedObjectsProperty)

            Return If([single] Is Nothing, New Object(-1) {}, value)
        End Function

        Private Shared Sub SelectedObjectPropertyChanged(source As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim pg As WpfPropertyGrid = TryCast(source, WpfPropertyGrid)
            pg.CoerceValue(SelectedObjectsProperty)

            If e.NewValue Is Nothing Then
                pg.OnSelectionChangedMethod.Invoke(pg.Designer.PropertyInspectorView, New Object() {Nothing})
                pg.SelectionTypeLabel.Text = String.Empty
            Else
                Dim context = New EditingContext()
                Dim mtm = New ModelTreeManager(context)
                mtm.Load(e.NewValue)
                Dim selection__1 As Selection = Selection.[Select](context, mtm.Root)

                pg.OnSelectionChangedMethod.Invoke(pg.Designer.PropertyInspectorView, New Object() {selection__1})
                pg.SelectionTypeLabel.Text = e.NewValue.[GetType]().Name
            End If

            pg.ChangeHelpText(String.Empty, String.Empty)
        End Sub
        Private Shared Sub SelectedObjectsPropertyChanged(source As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim pg As WpfPropertyGrid = TryCast(source, WpfPropertyGrid)
            pg.CoerceValue(SelectedObjectsProperty)

            Dim collection As Object() = TryCast(e.NewValue, Object())

            If collection.Length = 0 Then
                pg.OnSelectionChangedMethod.Invoke(pg.Designer.PropertyInspectorView, New Object() {Nothing})
                pg.SelectionTypeLabel.Text = String.Empty
            Else
                Dim same As Boolean = True
                Dim first As Type = Nothing

                Dim context = New EditingContext()
                Dim mtm = New ModelTreeManager(context)
                Dim selection__1 As Selection = Nothing

                ' Accumulates the selection and determines the type to be shown in the top of the PG
                For i As Integer = 0 To collection.Length - 1
                    mtm.Load(collection(i))
                    If i = 0 Then
                        selection__1 = Selection.[Select](context, mtm.Root)
                        first = collection(0).[GetType]()
                    Else
                        selection__1 = Selection.Union(context, mtm.Root)
                        If Not collection(i).[GetType]().Equals(first) Then
                            same = False
                        End If
                    End If
                Next

                pg.OnSelectionChangedMethod.Invoke(pg.Designer.PropertyInspectorView, New Object() {selection__1})
                pg.SelectionTypeLabel.Text = If(same, first.Name & " <multiple>", "Object <multiple>")
            End If

            pg.ChangeHelpText(String.Empty, String.Empty)
        End Sub
        Private Shared Sub HelpVisiblePropertyChanged(source As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim pg As WpfPropertyGrid = TryCast(source, WpfPropertyGrid)

            If e.NewValue <> e.OldValue Then
                If e.NewValue.Equals(True) Then
                    pg.RowDefinitions(1).Height = New GridLength(5)
                    pg.RowDefinitions(2).Height = New GridLength(pg.HelpTextHeight)
                Else
                    pg.HelpTextHeight = pg.RowDefinitions(2).Height.Value
                    pg.RowDefinitions(1).Height = New GridLength(0)
                    pg.RowDefinitions(2).Height = New GridLength(0)
                End If
            End If
        End Sub
        Private Shared Sub ToolbarVisiblePropertyChanged(source As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim pg As WpfPropertyGrid = TryCast(source, WpfPropertyGrid)
            pg.PropertyToolBar.Visibility = If(e.NewValue.Equals(True), Visibility.Visible, Visibility.Collapsed)
        End Sub
        Private Shared Sub PropertySortPropertyChanged(source As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim pg As WpfPropertyGrid = TryCast(source, WpfPropertyGrid)
            Dim sort As PropertySort = CType(e.NewValue, PropertySort)

            Dim isAlpha As Boolean = (sort = PropertySort.Alphabetical OrElse sort = PropertySort.NoSort)
            pg.IsInAlphaViewMethod.Invoke(pg.Designer.PropertyInspectorView, New Object() {isAlpha})
        End Sub
#End Region

        ''' <summary>Default constructor, creates the UIElements including a PropertyInspector</summary>
        Public Sub New()
            Me.ColumnDefinitions.Add(New ColumnDefinition())
            Me.RowDefinitions.Add(New RowDefinition() With {
                .Height = New GridLength(1, GridUnitType.Star)
            })
            Me.RowDefinitions.Add(New RowDefinition() With {
                .Height = New GridLength(0)
            })
            Me.RowDefinitions.Add(New RowDefinition() With {
                .Height = New GridLength(0)
            })

            Me.Designer = New WorkflowDesigner()
            Dim title As New TextBlock() With {
                .Visibility = Windows.Visibility.Visible,
                .TextWrapping = TextWrapping.NoWrap,
                .TextTrimming = TextTrimming.CharacterEllipsis,
                .FontWeight = FontWeights.Bold
            }
            Dim descrip As New TextBlock() With {
                .Visibility = Windows.Visibility.Visible,
                .TextWrapping = TextWrapping.Wrap,
                .TextTrimming = TextTrimming.CharacterEllipsis
            }
            Dim dock__1 As New DockPanel() With {
                .Visibility = Windows.Visibility.Visible,
                .LastChildFill = True,
                .Margin = New Thickness(3, 0, 3, 0)
            }

            title.SetValue(DockPanel.DockProperty, Dock.Top)
            dock__1.Children.Add(title)
            dock__1.Children.Add(descrip)
            Me.HelpText = New Border() With {
                .Visibility = Windows.Visibility.Visible,
                .BorderBrush = SystemColors.ActiveBorderBrush,
                .Background = SystemColors.ControlBrush,
                .BorderThickness = New Thickness(1),
                .Child = dock__1
            }
            Me.Splitter = New GridSplitter() With {
                .Visibility = Windows.Visibility.Visible,
                .ResizeDirection = GridResizeDirection.Rows,
                .Height = 5,
                .HorizontalAlignment = Windows.HorizontalAlignment.Stretch
            }

            Dim inspector = Designer.PropertyInspectorView
            inspector.Visibility = Visibility.Visible
            inspector.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch)

            Me.Splitter.SetValue(Grid.RowProperty, 1)
            Me.Splitter.SetValue(Grid.ColumnProperty, 0)

            Me.HelpText.SetValue(Grid.RowProperty, 2)
            Me.HelpText.SetValue(Grid.ColumnProperty, 0)

            Dim binding As New Binding("Parent.Background")
            title.SetBinding(BackgroundProperty, binding)
            descrip.SetBinding(BackgroundProperty, binding)

            Me.Children.Add(inspector)
            Me.Children.Add(Me.Splitter)
            Me.Children.Add(Me.HelpText)

            Dim inspectorType As Type = inspector.[GetType]()
            Dim props = inspectorType.GetProperties(Reflection.BindingFlags.[Public] Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.DeclaredOnly)

            Dim methods = inspectorType.GetMethods(Reflection.BindingFlags.[Public] Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.DeclaredOnly)

            Me.RefreshMethod = inspectorType.GetMethod("RefreshPropertyList", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.DeclaredOnly)
            Me.IsInAlphaViewMethod = inspectorType.GetMethod("set_IsInAlphaView", Reflection.BindingFlags.[Public] Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.DeclaredOnly)
            Me.OnSelectionChangedMethod = inspectorType.GetMethod("OnSelectionChanged", Reflection.BindingFlags.[Public] Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.DeclaredOnly)
            Me.SelectionTypeLabel = TryCast(inspectorType.GetMethod("get_SelectionTypeLabel", Reflection.BindingFlags.[Public] Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.DeclaredOnly).Invoke(inspector, New Object(-1) {}), TextBlock)
            Me.PropertyToolBar = TryCast(inspectorType.GetMethod("get_PropertyToolBar", Reflection.BindingFlags.[Public] Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.DeclaredOnly).Invoke(inspector, New Object(-1) {}), Control)
            inspectorType.GetEvent("GotFocus").AddEventHandler(Me, [Delegate].CreateDelegate(GetType(RoutedEventHandler), Me, "GotFocusHandler", False))

            Me.SelectionTypeLabel.Text = String.Empty
        End Sub
        ''' <summary>Updates the PropertyGrid's properties</summary>
        Public Sub RefreshPropertyList()
            RefreshMethod.Invoke(Designer.PropertyInspectorView, New Object() {False})
        End Sub

        ''' <summary>Traps the change of focused property and updates the help text</summary>
        ''' <param name="sender">Not used</param>
        ''' <param name="args">Points to the source control containing the selected property</param>
        Private Sub GotFocusHandler(sender As Object, args As RoutedEventArgs)
            'if (args.OriginalSource is TextBlock)
            '{
            Dim title As String = String.Empty
            Dim descrip As String = String.Empty
            Dim theSelectedObjects = TryCast(Me.GetValue(SelectedObjectsProperty), Object())

            If theSelectedObjects IsNot Nothing AndAlso theSelectedObjects.Length > 0 Then
                Dim first As Type = theSelectedObjects(0).[GetType]()
                For i As Integer = 1 To theSelectedObjects.Length - 1
                    If Not theSelectedObjects(i).[GetType]().Equals(first) Then
                        ChangeHelpText(title, descrip)
                        Return
                    End If
                Next

                Dim data As Object = TryCast(args.OriginalSource, FrameworkElement).DataContext
                Dim propEntry As PropertyInfo = data.[GetType]().GetProperty("PropertyEntry")
                If propEntry Is Nothing Then
                    propEntry = data.[GetType]().GetProperty("ParentProperty")
                End If

                If propEntry IsNot Nothing Then
                    Dim propEntryValue As Object = propEntry.GetValue(data, Nothing)
                    Dim propName As String = TryCast(propEntryValue.[GetType]().GetProperty("PropertyName").GetValue(propEntryValue, Nothing), String)
                    title = TryCast(propEntryValue.[GetType]().GetProperty("DisplayName").GetValue(propEntryValue, Nothing), String)
                    Dim [property] As PropertyInfo = theSelectedObjects(0).[GetType]().GetProperty(propName)
                    Dim attrs As Object() = [property].GetCustomAttributes(GetType(DescriptionAttribute), True)

                    If attrs IsNot Nothing AndAlso attrs.Length > 0 Then
                        descrip = TryCast(attrs(0), DescriptionAttribute).Description
                    End If
                End If
                ChangeHelpText(title, descrip)
            End If
            '}
        End Sub
        ''' <summary>Changes the text help area contents</summary>
        ''' <param name="title">Title in bold</param>
        ''' <param name="descrip">Description with ellipsis</param>
        Private Sub ChangeHelpText(title As String, descrip As String)
            Dim dock As DockPanel = TryCast(Me.HelpText.Child, DockPanel)
            TryCast(dock.Children(0), TextBlock).Text = title
            TryCast(dock.Children(1), TextBlock).Text = descrip
        End Sub
    End Class
End Namespace
