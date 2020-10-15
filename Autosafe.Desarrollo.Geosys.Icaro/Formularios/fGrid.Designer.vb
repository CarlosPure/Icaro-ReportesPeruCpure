<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fGrid
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grdDatos = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colFechaHora = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPlaca = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colVelocidad = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colRumbo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colLatitud = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colLongitud = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colEvento = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colKilometraje = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCalle = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdDatos
        '
        Me.grdDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdDatos.Location = New System.Drawing.Point(0, 0)
        Me.grdDatos.LookAndFeel.SkinName = "Office 2010 Blue"
        Me.grdDatos.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.grdDatos.MainView = Me.GridView1
        Me.grdDatos.Name = "grdDatos"
        Me.grdDatos.Size = New System.Drawing.Size(1055, 277)
        Me.grdDatos.TabIndex = 0
        Me.grdDatos.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colFechaHora, Me.colPlaca, Me.colVelocidad, Me.colRumbo, Me.colLatitud, Me.colLongitud, Me.colEvento, Me.colKilometraje, Me.colCalle})
        Me.GridView1.GridControl = Me.grdDatos
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowGroupPanel = False
        Me.GridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.PaintStyleName = "Style3D"
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colFechaHora, DevExpress.Data.ColumnSortOrder.Descending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPlaca, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colFechaHora
        '
        Me.colFechaHora.Caption = "Fecha Hora"
        Me.colFechaHora.FieldName = "Date_Time"
        Me.colFechaHora.Name = "colFechaHora"
        Me.colFechaHora.Visible = True
        Me.colFechaHora.VisibleIndex = 0
        '
        'colPlaca
        '
        Me.colPlaca.Caption = "Placa"
        Me.colPlaca.FieldName = "VID"
        Me.colPlaca.Name = "colPlaca"
        Me.colPlaca.Visible = True
        Me.colPlaca.VisibleIndex = 1
        '
        'colVelocidad
        '
        Me.colVelocidad.Caption = "Velocidad"
        Me.colVelocidad.FieldName = "Speed"
        Me.colVelocidad.Name = "colVelocidad"
        Me.colVelocidad.Visible = True
        Me.colVelocidad.VisibleIndex = 2
        '
        'colRumbo
        '
        Me.colRumbo.Caption = "Rumbo"
        Me.colRumbo.FieldName = "Heading"
        Me.colRumbo.Name = "colRumbo"
        Me.colRumbo.Visible = True
        Me.colRumbo.VisibleIndex = 3
        '
        'colLatitud
        '
        Me.colLatitud.Caption = "Latitud"
        Me.colLatitud.FieldName = "Latitude"
        Me.colLatitud.Name = "colLatitud"
        Me.colLatitud.Visible = True
        Me.colLatitud.VisibleIndex = 4
        '
        'colLongitud
        '
        Me.colLongitud.Caption = "Longitud"
        Me.colLongitud.FieldName = "Loogitude"
        Me.colLongitud.Name = "colLongitud"
        Me.colLongitud.Visible = True
        Me.colLongitud.VisibleIndex = 5
        '
        'colEvento
        '
        Me.colEvento.Caption = "Evento"
        Me.colEvento.FieldName = "DEvento"
        Me.colEvento.Name = "colEvento"
        Me.colEvento.Visible = True
        Me.colEvento.VisibleIndex = 6
        '
        'colKilometraje
        '
        Me.colKilometraje.Caption = "Kilometraje"
        Me.colKilometraje.FieldName = "Kilometraje"
        Me.colKilometraje.Name = "colKilometraje"
        Me.colKilometraje.Visible = True
        Me.colKilometraje.VisibleIndex = 8
        '
        'colCalle
        '
        Me.colCalle.Caption = "Calle"
        Me.colCalle.FieldName = "Calle"
        Me.colCalle.Name = "colCalle"
        Me.colCalle.Visible = True
        Me.colCalle.VisibleIndex = 7
        '
        'fGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1055, 277)
        Me.Controls.Add(Me.grdDatos)
        Me.Name = "fGrid"
        Me.Text = "fGrid"
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grdDatos As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colFechaHora As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVelocidad As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colRumbo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPlaca As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLatitud As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLongitud As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEvento As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKilometraje As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCalle As DevExpress.XtraGrid.Columns.GridColumn
End Class
