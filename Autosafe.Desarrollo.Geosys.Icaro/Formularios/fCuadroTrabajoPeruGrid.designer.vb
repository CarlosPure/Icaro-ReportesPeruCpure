<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fCuadroTrabajoPeruGrid
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
        Me.colFECHA_OT = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPRODUCTO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colVID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colID_ENTIDAD = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colENTIDAD = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTELEFONO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colEMAIL = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPLACA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMOTOR = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCHASIS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colID_VEHICULO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMARCA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMODELO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ANIO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTIPO_DISPOSITIVO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCELULAR_VID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colID_ENT_HUNTERSYS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colENT_HUNTERSYS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFINANCIERA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCONCESIONARIO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFECHA_ENVIO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colEMAIL_ENVIO = New DevExpress.XtraGrid.Columns.GridColumn()
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
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colFECHA_OT, Me.colPRODUCTO, Me.colVID, Me.colID_ENTIDAD, Me.colENTIDAD, Me.colTELEFONO, Me.colEMAIL, Me.colPLACA, Me.colMOTOR, Me.colCHASIS, Me.colID_VEHICULO, Me.colMARCA, Me.colMODELO, Me.ANIO, Me.colTIPO_DISPOSITIVO, Me.colCELULAR_VID, Me.colID_ENT_HUNTERSYS, Me.colENT_HUNTERSYS, Me.colFINANCIERA, Me.colCONCESIONARIO, Me.colFECHA_ENVIO, Me.colEMAIL_ENVIO})
        Me.GridView1.GridControl = Me.grdDatos
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowGroupPanel = False
        Me.GridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.PaintStyleName = "Style3D"
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colFECHA_OT, DevExpress.Data.ColumnSortOrder.Descending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPRODUCTO, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colFECHA_OT
        '
        Me.colFECHA_OT.Caption = "FECHA_OT"
        Me.colFECHA_OT.FieldName = "FECHA_OT"
        Me.colFECHA_OT.Name = "colFECHA_OT"
        Me.colFECHA_OT.Visible = True
        Me.colFECHA_OT.VisibleIndex = 0
        '
        'colPRODUCTO
        '
        Me.colPRODUCTO.Caption = "PRODUCTO"
        Me.colPRODUCTO.FieldName = "PRODUCTO"
        Me.colPRODUCTO.Name = "colPRODUCTO"
        Me.colPRODUCTO.Visible = True
        Me.colPRODUCTO.VisibleIndex = 1
        '
        'colVID
        '
        Me.colVID.Caption = "VID"
        Me.colVID.FieldName = "VID"
        Me.colVID.Name = "colVID"
        Me.colVID.Visible = True
        Me.colVID.VisibleIndex = 2
        '
        'colID_ENTIDAD
        '
        Me.colID_ENTIDAD.Caption = "ID_ENTIDAD"
        Me.colID_ENTIDAD.FieldName = "ID_ENTIDAD"
        Me.colID_ENTIDAD.Name = "colID_ENTIDAD"
        Me.colID_ENTIDAD.Visible = True
        Me.colID_ENTIDAD.VisibleIndex = 3
        '
        'colENTIDAD
        '
        Me.colENTIDAD.Caption = "ENTIDAD"
        Me.colENTIDAD.FieldName = "ENTIDAD"
        Me.colENTIDAD.Name = "colENTIDAD"
        Me.colENTIDAD.Visible = True
        Me.colENTIDAD.VisibleIndex = 4
        '
        'colTELEFONO
        '
        Me.colTELEFONO.Caption = "TELEFONO"
        Me.colTELEFONO.FieldName = "TELEFONO"
        Me.colTELEFONO.Name = "colTELEFONO"
        '
        'colEMAIL
        '
        Me.colEMAIL.Caption = "EMAIL"
        Me.colEMAIL.FieldName = "EMAIL"
        Me.colEMAIL.Name = "colEMAIL"
        Me.colEMAIL.Visible = True
        Me.colEMAIL.VisibleIndex = 5
        '
        'colPLACA
        '
        Me.colPLACA.Caption = "PLACA"
        Me.colPLACA.FieldName = "PLACA"
        Me.colPLACA.Name = "colPLACA"
        Me.colPLACA.Visible = True
        Me.colPLACA.VisibleIndex = 6
        '
        'colMOTOR
        '
        Me.colMOTOR.Caption = "MOTOR"
        Me.colMOTOR.FieldName = "MOTOR"
        Me.colMOTOR.Name = "colMOTOR"
        Me.colMOTOR.Visible = True
        Me.colMOTOR.VisibleIndex = 8
        '
        'colCHASIS
        '
        Me.colCHASIS.Caption = "CHASIS"
        Me.colCHASIS.FieldName = "CHASIS"
        Me.colCHASIS.Name = "colCHASIS"
        Me.colCHASIS.Visible = True
        Me.colCHASIS.VisibleIndex = 7
        '
        'colID_VEHICULO
        '
        Me.colID_VEHICULO.Caption = "ID_VEHICULO"
        Me.colID_VEHICULO.FieldName = "ID_VEHICULO"
        Me.colID_VEHICULO.Name = "colID_VEHICULO"
        Me.colID_VEHICULO.Visible = True
        Me.colID_VEHICULO.VisibleIndex = 9
        '
        'colMARCA
        '
        Me.colMARCA.Caption = "MARCA"
        Me.colMARCA.FieldName = "MARCA"
        Me.colMARCA.Name = "colMARCA"
        Me.colMARCA.Visible = True
        Me.colMARCA.VisibleIndex = 10
        '
        'colMODELO
        '
        Me.colMODELO.Caption = "MODELO"
        Me.colMODELO.FieldName = "MODELO"
        Me.colMODELO.Name = "colMODELO"
        '
        'ANIO
        '
        Me.ANIO.Caption = "ANIO"
        Me.ANIO.FieldName = "ANIO"
        Me.ANIO.Name = "ANIO"
        '
        'colTIPO_DISPOSITIVO
        '
        Me.colTIPO_DISPOSITIVO.Caption = "TIPO_DISPOSITIVO"
        Me.colTIPO_DISPOSITIVO.FieldName = "TIPO_DISPOSITIVO"
        Me.colTIPO_DISPOSITIVO.Name = "colTIPO_DISPOSITIVO"
        '
        'colCELULAR_VID
        '
        Me.colCELULAR_VID.Caption = "CELULAR_VID"
        Me.colCELULAR_VID.FieldName = "CELULAR_VID"
        Me.colCELULAR_VID.Name = "colCELULAR_VID"
        '
        'colID_ENT_HUNTERSYS
        '
        Me.colID_ENT_HUNTERSYS.Caption = "ID_ENT_HUNTERSYS"
        Me.colID_ENT_HUNTERSYS.FieldName = "ID_ENT_HUNTERSYS"
        Me.colID_ENT_HUNTERSYS.Name = "colID_ENT_HUNTERSYS"
        Me.colID_ENT_HUNTERSYS.Visible = True
        Me.colID_ENT_HUNTERSYS.VisibleIndex = 11
        '
        'colENT_HUNTERSYS
        '
        Me.colENT_HUNTERSYS.Caption = "ENT_HUNTERSYS"
        Me.colENT_HUNTERSYS.FieldName = "ENT_HUNTERSYS"
        Me.colENT_HUNTERSYS.Name = "colENT_HUNTERSYS"
        Me.colENT_HUNTERSYS.Visible = True
        Me.colENT_HUNTERSYS.VisibleIndex = 12
        '
        'colFINANCIERA
        '
        Me.colFINANCIERA.Caption = "FINANCIERA"
        Me.colFINANCIERA.FieldName = "FINANCIERA"
        Me.colFINANCIERA.Name = "colFINANCIERA"
        Me.colFINANCIERA.Visible = True
        Me.colFINANCIERA.VisibleIndex = 13
        '
        'colCONCESIONARIO
        '
        Me.colCONCESIONARIO.Caption = "CONCESIONARIO"
        Me.colCONCESIONARIO.FieldName = "CONCESIONARIO"
        Me.colCONCESIONARIO.Name = "colCONCESIONARIO"
        Me.colCONCESIONARIO.Visible = True
        Me.colCONCESIONARIO.VisibleIndex = 14
        '
        'colFECHA_ENVIO
        '
        Me.colFECHA_ENVIO.Caption = "FECHA_ENVIO"
        Me.colFECHA_ENVIO.FieldName = "FECHA_ENVIO"
        Me.colFECHA_ENVIO.Name = "colFECHA_ENVIO"
        Me.colFECHA_ENVIO.Visible = True
        Me.colFECHA_ENVIO.VisibleIndex = 15
        '
        'colEMAIL_ENVIO
        '
        Me.colEMAIL_ENVIO.Caption = "EMAIL_ENVIO"
        Me.colEMAIL_ENVIO.FieldName = "EMAIL_ENVIO"
        Me.colEMAIL_ENVIO.Name = "colEMAIL_ENVIO"
        Me.colEMAIL_ENVIO.Visible = True
        Me.colEMAIL_ENVIO.VisibleIndex = 16
        '
        'fCuadroTrabajoPeruGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1055, 291)
        Me.Controls.Add(Me.grdDatos)
        Me.Name = "fCuadroTrabajoPeruGrid"
        Me.Text = "fGrid"
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grdDatos As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colFECHA_OT As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colID_ENTIDAD As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPRODUCTO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colENTIDAD As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEMAIL As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPLACA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMOTOR As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCHASIS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTELEFONO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colID_VEHICULO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMARCA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ANIO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTIPO_DISPOSITIVO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCELULAR_VID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colID_ENT_HUNTERSYS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colENT_HUNTERSYS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFINANCIERA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCONCESIONARIO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMODELO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFECHA_ENVIO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEMAIL_ENVIO As DevExpress.XtraGrid.Columns.GridColumn
End Class
