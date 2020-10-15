<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMafGrid2
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
        Me.colFechaCorte = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colHoraCorte = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMOTOR = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPLACA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMARCA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMODELO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colDistanciaRecorrida = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTiempoRecorrido = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colUbiGeo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCHASIS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFECHAHORA = New DevExpress.XtraGrid.Columns.GridColumn()
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
        Me.grdDatos.Size = New System.Drawing.Size(1055, 291)
        Me.grdDatos.TabIndex = 0
        Me.grdDatos.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colFechaCorte, Me.colHoraCorte, Me.colMOTOR, Me.colPLACA, Me.colMARCA, Me.colMODELO, Me.colDistanciaRecorrida, Me.colTiempoRecorrido, Me.colUbiGeo, Me.colCHASIS, Me.colFECHAHORA})
        Me.GridView1.GridControl = Me.grdDatos
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowGroupPanel = False
        Me.GridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.PaintStyleName = "Style3D"
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colFechaCorte, DevExpress.Data.ColumnSortOrder.Descending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colHoraCorte, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colFechaCorte
        '
        Me.colFechaCorte.Caption = "FechaCorte"
        Me.colFechaCorte.FieldName = "FechaCorte"
        Me.colFechaCorte.Name = "colFechaCorte"
        Me.colFechaCorte.Visible = True
        Me.colFechaCorte.VisibleIndex = 0
        '
        'colHoraCorte
        '
        Me.colHoraCorte.Caption = "HoraCorte"
        Me.colHoraCorte.FieldName = "HoraCorte"
        Me.colHoraCorte.Name = "colHoraCorte"
        Me.colHoraCorte.Visible = True
        Me.colHoraCorte.VisibleIndex = 1
        '
        'colMOTOR
        '
        Me.colMOTOR.Caption = "MOTOR"
        Me.colMOTOR.FieldName = "MOTOR"
        Me.colMOTOR.Name = "colMOTOR"
        Me.colMOTOR.Visible = True
        Me.colMOTOR.VisibleIndex = 2
        '
        'colPLACA
        '
        Me.colPLACA.Caption = "PLACA"
        Me.colPLACA.FieldName = "PLACA"
        Me.colPLACA.Name = "colPLACA"
        Me.colPLACA.Visible = True
        Me.colPLACA.VisibleIndex = 3
        '
        'colMARCA
        '
        Me.colMARCA.Caption = "MARCA"
        Me.colMARCA.FieldName = "MARCA"
        Me.colMARCA.Name = "colMARCA"
        Me.colMARCA.Visible = True
        Me.colMARCA.VisibleIndex = 4
        '
        'colMODELO
        '
        Me.colMODELO.Caption = "MODELO"
        Me.colMODELO.FieldName = "MODELO"
        Me.colMODELO.Name = "colMODELO"
        Me.colMODELO.Visible = True
        Me.colMODELO.VisibleIndex = 5
        '
        'colDistanciaRecorrida
        '
        Me.colDistanciaRecorrida.Caption = "DistanciaRecorrida"
        Me.colDistanciaRecorrida.FieldName = "DistanciaRecorrida"
        Me.colDistanciaRecorrida.Name = "colDistanciaRecorrida"
        Me.colDistanciaRecorrida.Visible = True
        Me.colDistanciaRecorrida.VisibleIndex = 6
        '
        'colTiempoRecorrido
        '
        Me.colTiempoRecorrido.Caption = "TiempoRecorrido"
        Me.colTiempoRecorrido.FieldName = "TiempoRecorrido"
        Me.colTiempoRecorrido.Name = "colTiempoRecorrido"
        Me.colTiempoRecorrido.Visible = True
        Me.colTiempoRecorrido.VisibleIndex = 7
        '
        'colUbiGeo
        '
        Me.colUbiGeo.Caption = "UbiGeo"
        Me.colUbiGeo.FieldName = "UbiGeo"
        Me.colUbiGeo.Name = "colUbiGeo"
        Me.colUbiGeo.Visible = True
        Me.colUbiGeo.VisibleIndex = 9
        '
        'colCHASIS
        '
        Me.colCHASIS.Caption = "CHASIS"
        Me.colCHASIS.FieldName = "CHASIS"
        Me.colCHASIS.Name = "colCHASIS"
        Me.colCHASIS.Visible = True
        Me.colCHASIS.VisibleIndex = 8
        '
        'colFECHAHORA
        '
        Me.colFECHAHORA.Caption = "FECHAHORA"
        Me.colFECHAHORA.FieldName = "FECHAHORA"
        Me.colFECHAHORA.Name = "colFECHAHORA"
        Me.colFECHAHORA.Visible = True
        Me.colFECHAHORA.VisibleIndex = 10
        '
        'fMafGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1055, 291)
        Me.Controls.Add(Me.grdDatos)
        Me.Name = "fMafGrid"
        Me.Text = "fGrid"
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grdDatos As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colFechaCorte As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMOTOR As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPLACA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colHoraCorte As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMARCA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDistanciaRecorrida As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTiempoRecorrido As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colUbiGeo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCHASIS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMODELO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFECHAHORA As DevExpress.XtraGrid.Columns.GridColumn
End Class
