namespace ImageClassifierApp
{
    partial class ImageClassifierForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnStartClassification = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvHistogram = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvDft = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvDct = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgvScale = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dgvGradient = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chartForResults = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnVisualizeResults = new System.Windows.Forms.Button();
            this.pbTestImage = new System.Windows.Forms.PictureBox();
            this.pbFeatureHist = new System.Windows.Forms.PictureBox();
            this.pbFeatureDFT = new System.Windows.Forms.PictureBox();
            this.pbFeatureDCT = new System.Windows.Forms.PictureBox();
            this.pbFeatureScale = new System.Windows.Forms.PictureBox();
            this.pbFeatureGradient = new System.Windows.Forms.PictureBox();
            this.pbResultHist = new System.Windows.Forms.PictureBox();
            this.pbResultDFT = new System.Windows.Forms.PictureBox();
            this.pbResultDCT = new System.Windows.Forms.PictureBox();
            this.pbResultScale = new System.Windows.Forms.PictureBox();
            this.pbResultGradient = new System.Windows.Forms.PictureBox();
            this.lblResultHistClass = new System.Windows.Forms.Label();
            this.lblResultDftClass = new System.Windows.Forms.Label();
            this.lblResultDctClass = new System.Windows.Forms.Label();
            this.lblResultScaleClass = new System.Windows.Forms.Label();
            this.lblResultGradientClass = new System.Windows.Forms.Label();
            this.numericForReference = new System.Windows.Forms.NumericUpDown();
            this.buttonForValidation = new System.Windows.Forms.Button();
            this.buttonForValidationFawkes = new System.Windows.Forms.Button();
            this.checkBoxForFawkes = new System.Windows.Forms.CheckBox();
            this.labelForReferenceCount = new System.Windows.Forms.Label();
            this.labelForHistogram = new System.Windows.Forms.Label();
            this.labelForDFT = new System.Windows.Forms.Label();
            this.labelForDCT = new System.Windows.Forms.Label();
            this.labelForScale = new System.Windows.Forms.Label();
            this.labelForGradient = new System.Windows.Forms.Label();
            this.labelForTestImage = new System.Windows.Forms.Label();
            this.tabControlResults.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistogram)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDft)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDct)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScale)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGradient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartForResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTestImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureHist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureDFT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureDCT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureGradient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultHist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultDFT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultDCT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultGradient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericForReference)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartClassification
            // 
            this.btnStartClassification.Location = new System.Drawing.Point(607, 25);
            this.btnStartClassification.Name = "btnStartClassification";
            this.btnStartClassification.Size = new System.Drawing.Size(106, 46);
            this.btnStartClassification.TabIndex = 4;
            this.btnStartClassification.Text = "Поиск лучших параметров";
            this.btnStartClassification.UseVisualStyleBackColor = true;
            this.btnStartClassification.Click += new System.EventHandler(this.btnStartClassification_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(42, 612);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(526, 44);
            this.progressBar.TabIndex = 5;
            this.progressBar.Visible = false;
            // 
            // tabControlResults
            // 
            this.tabControlResults.Controls.Add(this.tabPage1);
            this.tabControlResults.Controls.Add(this.tabPage2);
            this.tabControlResults.Controls.Add(this.tabPage3);
            this.tabControlResults.Controls.Add(this.tabPage4);
            this.tabControlResults.Controls.Add(this.tabPage5);
            this.tabControlResults.Location = new System.Drawing.Point(603, 88);
            this.tabControlResults.Name = "tabControlResults";
            this.tabControlResults.SelectedIndex = 0;
            this.tabControlResults.Size = new System.Drawing.Size(349, 229);
            this.tabControlResults.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvHistogram);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(341, 203);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Гистограмма";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvHistogram
            // 
            this.dgvHistogram.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistogram.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvHistogram.Location = new System.Drawing.Point(6, 6);
            this.dgvHistogram.Name = "dgvHistogram";
            this.dgvHistogram.Size = new System.Drawing.Size(331, 191);
            this.dgvHistogram.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Класс";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Верно классифицировано";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Всего";
            this.Column3.Name = "Column3";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvDft);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(341, 203);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "dft";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvDft
            // 
            this.dgvDft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDft.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgvDft.Location = new System.Drawing.Point(6, 6);
            this.dgvDft.Name = "dgvDft";
            this.dgvDft.Size = new System.Drawing.Size(321, 197);
            this.dgvDft.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Класс";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Верно классифицировано";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Всего";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvDct);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(341, 203);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "dct";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvDct
            // 
            this.dgvDct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dgvDct.Location = new System.Drawing.Point(3, 3);
            this.dgvDct.Name = "dgvDct";
            this.dgvDct.Size = new System.Drawing.Size(321, 197);
            this.dgvDct.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Класс";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Верно классифицировано";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Всего";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgvScale);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(341, 203);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "scale";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvScale
            // 
            this.dgvScale.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScale.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            this.dgvScale.Location = new System.Drawing.Point(3, 3);
            this.dgvScale.Name = "dgvScale";
            this.dgvScale.Size = new System.Drawing.Size(321, 197);
            this.dgvScale.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Класс";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Верно классифицировано";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Всего";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dgvGradient);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(341, 203);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Градиент";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // dgvGradient
            // 
            this.dgvGradient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGradient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12});
            this.dgvGradient.Location = new System.Drawing.Point(3, 3);
            this.dgvGradient.Name = "dgvGradient";
            this.dgvGradient.Size = new System.Drawing.Size(321, 197);
            this.dgvGradient.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Класс";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Верно классифицировано";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Всего";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // chartForResults
            // 
            chartArea3.Name = "ChartArea1";
            this.chartForResults.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartForResults.Legends.Add(legend3);
            this.chartForResults.Location = new System.Drawing.Point(1032, 170);
            this.chartForResults.Name = "chartForResults";
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.Legend = "Legend1";
            series11.Name = "Гистограмма";
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series12.Legend = "Legend1";
            series12.Name = "DFT";
            series13.ChartArea = "ChartArea1";
            series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series13.Legend = "Legend1";
            series13.Name = "DCT";
            series14.ChartArea = "ChartArea1";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series14.Legend = "Legend1";
            series14.Name = "Scale";
            series15.ChartArea = "ChartArea1";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series15.Legend = "Legend1";
            series15.Name = "Градиент";
            this.chartForResults.Series.Add(series11);
            this.chartForResults.Series.Add(series12);
            this.chartForResults.Series.Add(series13);
            this.chartForResults.Series.Add(series14);
            this.chartForResults.Series.Add(series15);
            this.chartForResults.Size = new System.Drawing.Size(518, 433);
            this.chartForResults.TabIndex = 7;
            this.chartForResults.Text = "chart1";
            // 
            // btnVisualizeResults
            // 
            this.btnVisualizeResults.Location = new System.Drawing.Point(44, 69);
            this.btnVisualizeResults.Name = "btnVisualizeResults";
            this.btnVisualizeResults.Size = new System.Drawing.Size(125, 37);
            this.btnVisualizeResults.TabIndex = 8;
            this.btnVisualizeResults.Text = "Начать классификацию";
            this.btnVisualizeResults.UseVisualStyleBackColor = true;
            this.btnVisualizeResults.Click += new System.EventHandler(this.btnVisualizeResults_Click_1);
            // 
            // pbTestImage
            // 
            this.pbTestImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTestImage.Location = new System.Drawing.Point(42, 271);
            this.pbTestImage.Name = "pbTestImage";
            this.pbTestImage.Size = new System.Drawing.Size(134, 134);
            this.pbTestImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTestImage.TabIndex = 9;
            this.pbTestImage.TabStop = false;
            // 
            // pbFeatureHist
            // 
            this.pbFeatureHist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbFeatureHist.Location = new System.Drawing.Point(249, 271);
            this.pbFeatureHist.Name = "pbFeatureHist";
            this.pbFeatureHist.Size = new System.Drawing.Size(134, 134);
            this.pbFeatureHist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFeatureHist.TabIndex = 10;
            this.pbFeatureHist.TabStop = false;
            // 
            // pbFeatureDFT
            // 
            this.pbFeatureDFT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbFeatureDFT.Location = new System.Drawing.Point(434, 271);
            this.pbFeatureDFT.Name = "pbFeatureDFT";
            this.pbFeatureDFT.Size = new System.Drawing.Size(134, 134);
            this.pbFeatureDFT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFeatureDFT.TabIndex = 11;
            this.pbFeatureDFT.TabStop = false;
            // 
            // pbFeatureDCT
            // 
            this.pbFeatureDCT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbFeatureDCT.Location = new System.Drawing.Point(42, 457);
            this.pbFeatureDCT.Name = "pbFeatureDCT";
            this.pbFeatureDCT.Size = new System.Drawing.Size(134, 134);
            this.pbFeatureDCT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFeatureDCT.TabIndex = 12;
            this.pbFeatureDCT.TabStop = false;
            // 
            // pbFeatureScale
            // 
            this.pbFeatureScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbFeatureScale.Location = new System.Drawing.Point(249, 457);
            this.pbFeatureScale.Name = "pbFeatureScale";
            this.pbFeatureScale.Size = new System.Drawing.Size(134, 134);
            this.pbFeatureScale.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFeatureScale.TabIndex = 13;
            this.pbFeatureScale.TabStop = false;
            // 
            // pbFeatureGradient
            // 
            this.pbFeatureGradient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbFeatureGradient.Location = new System.Drawing.Point(434, 457);
            this.pbFeatureGradient.Name = "pbFeatureGradient";
            this.pbFeatureGradient.Size = new System.Drawing.Size(134, 134);
            this.pbFeatureGradient.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFeatureGradient.TabIndex = 14;
            this.pbFeatureGradient.TabStop = false;
            // 
            // pbResultHist
            // 
            this.pbResultHist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbResultHist.Location = new System.Drawing.Point(603, 357);
            this.pbResultHist.Name = "pbResultHist";
            this.pbResultHist.Size = new System.Drawing.Size(134, 134);
            this.pbResultHist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbResultHist.TabIndex = 15;
            this.pbResultHist.TabStop = false;
            // 
            // pbResultDFT
            // 
            this.pbResultDFT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbResultDFT.Location = new System.Drawing.Point(743, 357);
            this.pbResultDFT.Name = "pbResultDFT";
            this.pbResultDFT.Size = new System.Drawing.Size(134, 134);
            this.pbResultDFT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbResultDFT.TabIndex = 16;
            this.pbResultDFT.TabStop = false;
            // 
            // pbResultDCT
            // 
            this.pbResultDCT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbResultDCT.Location = new System.Drawing.Point(883, 357);
            this.pbResultDCT.Name = "pbResultDCT";
            this.pbResultDCT.Size = new System.Drawing.Size(134, 134);
            this.pbResultDCT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbResultDCT.TabIndex = 17;
            this.pbResultDCT.TabStop = false;
            // 
            // pbResultScale
            // 
            this.pbResultScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbResultScale.Location = new System.Drawing.Point(603, 510);
            this.pbResultScale.Name = "pbResultScale";
            this.pbResultScale.Size = new System.Drawing.Size(134, 134);
            this.pbResultScale.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbResultScale.TabIndex = 18;
            this.pbResultScale.TabStop = false;
            // 
            // pbResultGradient
            // 
            this.pbResultGradient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbResultGradient.Location = new System.Drawing.Point(743, 510);
            this.pbResultGradient.Name = "pbResultGradient";
            this.pbResultGradient.Size = new System.Drawing.Size(134, 134);
            this.pbResultGradient.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbResultGradient.TabIndex = 19;
            this.pbResultGradient.TabStop = false;
            // 
            // lblResultHistClass
            // 
            this.lblResultHistClass.AutoSize = true;
            this.lblResultHistClass.Location = new System.Drawing.Point(600, 332);
            this.lblResultHistClass.Name = "lblResultHistClass";
            this.lblResultHistClass.Size = new System.Drawing.Size(134, 13);
            this.lblResultHistClass.TabIndex = 20;
            this.lblResultHistClass.Text = "Результат гистограммы:";
            // 
            // lblResultDftClass
            // 
            this.lblResultDftClass.AutoSize = true;
            this.lblResultDftClass.Location = new System.Drawing.Point(740, 332);
            this.lblResultDftClass.Name = "lblResultDftClass";
            this.lblResultDftClass.Size = new System.Drawing.Size(86, 13);
            this.lblResultDftClass.TabIndex = 21;
            this.lblResultDftClass.Text = "Результат DFT:";
            // 
            // lblResultDctClass
            // 
            this.lblResultDctClass.AutoSize = true;
            this.lblResultDctClass.Location = new System.Drawing.Point(880, 332);
            this.lblResultDctClass.Name = "lblResultDctClass";
            this.lblResultDctClass.Size = new System.Drawing.Size(87, 13);
            this.lblResultDctClass.TabIndex = 22;
            this.lblResultDctClass.Text = "Результат DCT:";
            // 
            // lblResultScaleClass
            // 
            this.lblResultScaleClass.AutoSize = true;
            this.lblResultScaleClass.Location = new System.Drawing.Point(600, 492);
            this.lblResultScaleClass.Name = "lblResultScaleClass";
            this.lblResultScaleClass.Size = new System.Drawing.Size(92, 13);
            this.lblResultScaleClass.TabIndex = 23;
            this.lblResultScaleClass.Text = "Результат Scale:";
            // 
            // lblResultGradientClass
            // 
            this.lblResultGradientClass.AutoSize = true;
            this.lblResultGradientClass.Location = new System.Drawing.Point(740, 492);
            this.lblResultGradientClass.Name = "lblResultGradientClass";
            this.lblResultGradientClass.Size = new System.Drawing.Size(117, 13);
            this.lblResultGradientClass.TabIndex = 24;
            this.lblResultGradientClass.Text = "Результат градиента:";
            // 
            // numericForReference
            // 
            this.numericForReference.Location = new System.Drawing.Point(460, 93);
            this.numericForReference.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericForReference.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericForReference.Name = "numericForReference";
            this.numericForReference.Size = new System.Drawing.Size(120, 20);
            this.numericForReference.TabIndex = 25;
            this.numericForReference.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericForReference.ValueChanged += new System.EventHandler(this.numericForReference_ValueChanged);
            // 
            // buttonForValidation
            // 
            this.buttonForValidation.Location = new System.Drawing.Point(896, 510);
            this.buttonForValidation.Name = "buttonForValidation";
            this.buttonForValidation.Size = new System.Drawing.Size(121, 47);
            this.buttonForValidation.TabIndex = 26;
            this.buttonForValidation.Text = "Статистика";
            this.buttonForValidation.UseVisualStyleBackColor = true;
            this.buttonForValidation.Click += new System.EventHandler(this.buttonForValidation_Click);
            // 
            // buttonForValidationFawkes
            // 
            this.buttonForValidationFawkes.Location = new System.Drawing.Point(896, 574);
            this.buttonForValidationFawkes.Name = "buttonForValidationFawkes";
            this.buttonForValidationFawkes.Size = new System.Drawing.Size(121, 47);
            this.buttonForValidationFawkes.TabIndex = 27;
            this.buttonForValidationFawkes.Text = "Статистика(Fawkes)";
            this.buttonForValidationFawkes.UseVisualStyleBackColor = true;
            this.buttonForValidationFawkes.Click += new System.EventHandler(this.buttonForValidationFawkes_Click);
            // 
            // checkBoxForFawkes
            // 
            this.checkBoxForFawkes.AutoSize = true;
            this.checkBoxForFawkes.Location = new System.Drawing.Point(460, 153);
            this.checkBoxForFawkes.Name = "checkBoxForFawkes";
            this.checkBoxForFawkes.Size = new System.Drawing.Size(63, 17);
            this.checkBoxForFawkes.TabIndex = 29;
            this.checkBoxForFawkes.Text = "Fawkes";
            this.checkBoxForFawkes.UseVisualStyleBackColor = true;
            this.checkBoxForFawkes.CheckedChanged += new System.EventHandler(this.checkBoxForFawkes_CheckedChanged);
            // 
            // labelForReferenceCount
            // 
            this.labelForReferenceCount.AutoSize = true;
            this.labelForReferenceCount.Location = new System.Drawing.Point(246, 93);
            this.labelForReferenceCount.Name = "labelForReferenceCount";
            this.labelForReferenceCount.Size = new System.Drawing.Size(197, 13);
            this.labelForReferenceCount.TabIndex = 30;
            this.labelForReferenceCount.Text = "Количество эталонных изображений:";
            // 
            // labelForHistogram
            // 
            this.labelForHistogram.AutoSize = true;
            this.labelForHistogram.Location = new System.Drawing.Point(246, 241);
            this.labelForHistogram.Name = "labelForHistogram";
            this.labelForHistogram.Size = new System.Drawing.Size(78, 13);
            this.labelForHistogram.TabIndex = 31;
            this.labelForHistogram.Text = "Гистограмма:";
            // 
            // labelForDFT
            // 
            this.labelForDFT.AutoSize = true;
            this.labelForDFT.Location = new System.Drawing.Point(431, 241);
            this.labelForDFT.Name = "labelForDFT";
            this.labelForDFT.Size = new System.Drawing.Size(31, 13);
            this.labelForDFT.TabIndex = 32;
            this.labelForDFT.Text = "DFT:";
            // 
            // labelForDCT
            // 
            this.labelForDCT.AutoSize = true;
            this.labelForDCT.Location = new System.Drawing.Point(39, 431);
            this.labelForDCT.Name = "labelForDCT";
            this.labelForDCT.Size = new System.Drawing.Size(32, 13);
            this.labelForDCT.TabIndex = 33;
            this.labelForDCT.Text = "DCT:";
            // 
            // labelForScale
            // 
            this.labelForScale.AutoSize = true;
            this.labelForScale.Location = new System.Drawing.Point(246, 431);
            this.labelForScale.Name = "labelForScale";
            this.labelForScale.Size = new System.Drawing.Size(37, 13);
            this.labelForScale.TabIndex = 34;
            this.labelForScale.Text = "Scale:";
            // 
            // labelForGradient
            // 
            this.labelForGradient.AutoSize = true;
            this.labelForGradient.Location = new System.Drawing.Point(431, 431);
            this.labelForGradient.Name = "labelForGradient";
            this.labelForGradient.Size = new System.Drawing.Size(57, 13);
            this.labelForGradient.TabIndex = 35;
            this.labelForGradient.Text = "Градиент:";
            // 
            // labelForTestImage
            // 
            this.labelForTestImage.AutoSize = true;
            this.labelForTestImage.Location = new System.Drawing.Point(39, 241);
            this.labelForTestImage.Name = "labelForTestImage";
            this.labelForTestImage.Size = new System.Drawing.Size(130, 13);
            this.labelForTestImage.TabIndex = 36;
            this.labelForTestImage.Text = "Исходное изображение:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1562, 668);
            this.Controls.Add(this.labelForTestImage);
            this.Controls.Add(this.labelForGradient);
            this.Controls.Add(this.labelForScale);
            this.Controls.Add(this.labelForDCT);
            this.Controls.Add(this.labelForDFT);
            this.Controls.Add(this.labelForHistogram);
            this.Controls.Add(this.labelForReferenceCount);
            this.Controls.Add(this.checkBoxForFawkes);
            this.Controls.Add(this.buttonForValidationFawkes);
            this.Controls.Add(this.buttonForValidation);
            this.Controls.Add(this.numericForReference);
            this.Controls.Add(this.lblResultGradientClass);
            this.Controls.Add(this.lblResultScaleClass);
            this.Controls.Add(this.lblResultDctClass);
            this.Controls.Add(this.lblResultDftClass);
            this.Controls.Add(this.lblResultHistClass);
            this.Controls.Add(this.pbResultGradient);
            this.Controls.Add(this.pbResultScale);
            this.Controls.Add(this.pbResultDCT);
            this.Controls.Add(this.pbResultDFT);
            this.Controls.Add(this.pbResultHist);
            this.Controls.Add(this.pbFeatureGradient);
            this.Controls.Add(this.pbFeatureScale);
            this.Controls.Add(this.pbFeatureDCT);
            this.Controls.Add(this.pbFeatureDFT);
            this.Controls.Add(this.pbFeatureHist);
            this.Controls.Add(this.pbTestImage);
            this.Controls.Add(this.btnVisualizeResults);
            this.Controls.Add(this.chartForResults);
            this.Controls.Add(this.tabControlResults);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnStartClassification);
            this.Name = "Form1";
            this.Text = "Классификатор изображений";
            this.tabControlResults.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistogram)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDft)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDct)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScale)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGradient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartForResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTestImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureHist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureDFT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureDCT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFeatureGradient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultHist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultDFT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultDCT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResultGradient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericForReference)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStartClassification;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TabControl tabControlResults;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvHistogram;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridView dgvDft;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridView dgvDct;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridView dgvScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridView dgvGradient;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartForResults;
        private System.Windows.Forms.Button btnVisualizeResults;
        private System.Windows.Forms.PictureBox pbTestImage;
        private System.Windows.Forms.PictureBox pbFeatureHist;
        private System.Windows.Forms.PictureBox pbFeatureDFT;
        private System.Windows.Forms.PictureBox pbFeatureDCT;
        private System.Windows.Forms.PictureBox pbFeatureScale;
        private System.Windows.Forms.PictureBox pbFeatureGradient;
        private System.Windows.Forms.PictureBox pbResultHist;
        private System.Windows.Forms.PictureBox pbResultDFT;
        private System.Windows.Forms.PictureBox pbResultDCT;
        private System.Windows.Forms.PictureBox pbResultScale;
        private System.Windows.Forms.PictureBox pbResultGradient;
        private System.Windows.Forms.Label lblResultHistClass;
        private System.Windows.Forms.Label lblResultDftClass;
        private System.Windows.Forms.Label lblResultDctClass;
        private System.Windows.Forms.Label lblResultScaleClass;
        private System.Windows.Forms.Label lblResultGradientClass;
        private System.Windows.Forms.NumericUpDown numericForReference;
        private System.Windows.Forms.Button buttonForValidation;
        private System.Windows.Forms.Button buttonForValidationFawkes;
        private System.Windows.Forms.CheckBox checkBoxForFawkes;
        private System.Windows.Forms.Label labelForReferenceCount;
        private System.Windows.Forms.Label labelForHistogram;
        private System.Windows.Forms.Label labelForDFT;
        private System.Windows.Forms.Label labelForDCT;
        private System.Windows.Forms.Label labelForScale;
        private System.Windows.Forms.Label labelForGradient;
        private System.Windows.Forms.Label labelForTestImage;
    }
}

