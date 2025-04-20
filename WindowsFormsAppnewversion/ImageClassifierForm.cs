using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ImageClassifierApp
{
    public partial class ImageClassifierForm : Form
    {
        private string referenceFolderPath = @"..\..\images\reference\1";
        private string testFolderPath = @"..\..\images\test\9";
        private Dictionary<string, List<Mat>> referenceImages = new Dictionary<string, List<Mat>>();
        private List<(string path, string trueClass)> testFiles = new List<(string path, string trueClass)>();
        private Dictionary<string, List<Mat>> referenceHistograms = new Dictionary<string, List<Mat>>();
        private Dictionary<string, List<Mat>> referenceDfts = new Dictionary<string, List<Mat>>();
        private Dictionary<string, List<Mat>> referenceDcts = new Dictionary<string, List<Mat>>();
        private Dictionary<string, List<Mat>> referenceDescriptors = new Dictionary<string, List<Mat>>();
        private Dictionary<string, List<List<double>>> referenceGradients = new Dictionary<string, List<List<double>>>();
        private Dictionary<string, List<Mat>> referenceScales =
            new Dictionary<string, List<Mat>>();
        private List<double> _histogramPoints = new List<double>();
        private List<double> _DFTPoints = new List<double>();
        private List<double> _DCTPoints = new List<double>();
        private List<double> _scalePoints = new List<double>();
        private List<double> _gradientPoints = new List<double>();
        private int _scaleBlock = 2;
        private int _gradY = 5;
        private int _gradS = 3;
        private int _vectorSizeDct = 36;
        private int _vectorSizeDft = 81;
        private int _bins = 64;
        private Dictionary<string, List<(string path, Mat image)>> referenceImagesViz =
            new Dictionary<string, List<(string path, Mat image)>>();
        private Dictionary<string, List<(string path, Mat feature)>> referenceHistogramsViz =
            new Dictionary<string, List<(string path, Mat feature)>>();
        private Dictionary<string, List<(string path, Mat feature)>> referenceDftsViz =
            new Dictionary<string, List<(string path, Mat feature)>>();
        private Dictionary<string, List<(string path, Mat feature)>> referenceDctsViz =
            new Dictionary<string, List<(string path, Mat feature)>>();
        private Dictionary<string, List<(string path, Mat feature)>> referenceScalesViz =
            new Dictionary<string, List<(string path, Mat feature)>>();
        private Dictionary<string, List<(string path, List<double> feature)>> referenceGradientsViz =
            new Dictionary<string, List<(string path, List<double> feature)>>();
        public ImageClassifierForm()
        {
            InitializeComponent();
            SetupDataGridViewColumns();
            chartForResults.ChartAreas[0].AxisY.Maximum = 100;
            chartForResults.ChartAreas[0].AxisX.Maximum = 9;
            chartForResults.ChartAreas[0].AxisX.Minimum = 1; 
        }
        private async void btnStartClassification_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(referenceFolderPath) || !Directory.Exists(testFolderPath))
            {
                MessageBox.Show("Выбранные папки не существуют. Пожалуйста, выберите корректные папки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SetUIEnabled(false);
            progressBar.Value = 0;
            progressBar.Visible = true;
            ClearResultTables();

            try
            {
                int[] histSizes = { 8, 16, 32, 64, 128, 256 };
                int[] dftDctVectorSizes = { 36, 49, 64, 81, 100, 121, 144, 169, 196, 225, 256, 289, 324, 361, 400, 441, 484, 529, 576, 625, 676, 729, 784, 841, 900 };
                int[] gradientYs = { 3, 5, 7, 9 };
                int[] gradientSs = { 3, 5, 7, 9 };
                int[] blockSizes = { 2, 3, 5, 7, 9, 11 };
                UpdateProgress(0, "Загрузка эталонных изображений");

                if (!LoadReferenceImages(referenceFolderPath))
                {
                    MessageBox.Show("Не удалось загрузить эталонные изображения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<string> classNames = referenceImages.Keys.ToList();

                UpdateProgress(5, "Загрузка тестовых изображений");

                if (!LoadTestFiles(testFolderPath))
                {
                    MessageBox.Show("Не удалось загрузить тестовые файлы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int totalTestFiles = testFiles.Count;
                if (totalTestFiles == 0)
                {
                    MessageBox.Show("В тестовой папке не найдено изображений.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int totalParamRuns = histSizes.Length
                                   + dftDctVectorSizes.Length
                                   + dftDctVectorSizes.Length
                                   + (gradientYs.Length * gradientSs.Length)
                                   + blockSizes.Length;
                progressBar.Maximum = totalParamRuns * totalTestFiles;
                int currentIteration = 0;
                await Task.Run(() =>
                {
                    foreach (int histSize in histSizes)
                    {
                        string paramString = $"Bins: {histSize}";
                        string currentTask = $"Гистограмма ({paramString})";
                        UpdateProgress(currentIteration, $"Поиск лучшего параметра: {currentTask}");
                        ClearReferenceHistograms();
                        foreach (var kvp in referenceImages)
                        {
                            var features = new List<Mat>();
                            foreach (var img in kvp.Value)
                            {
                                Mat feature = ExtractHistogram(img, histSize);
                                if (!feature.Empty()) features.Add(feature);
                                else feature.Dispose();
                            }
                            if (features.Count > 0) referenceHistograms[kvp.Key] = features;
                        }
                        if (!referenceHistograms.Any(kvp => kvp.Value.Any()))
                        {
                            currentIteration += totalTestFiles;
                            UpdateProgress(currentIteration);
                            continue;
                        }
                        int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                           (testGray) => ClassifyByHistogram(testGray, histSize));
                        AddResultToTable(dgvHistogram, paramString, correctCount, totalTestFiles);
                        ClearReferenceHistograms();
                    }
                    foreach (int vectorSize in dftDctVectorSizes)
                    {
                        string paramString = $"Size: {Math.Sqrt(vectorSize)}X{Math.Sqrt(vectorSize)}";
                        string currentTask = $"DFT ({paramString})";
                        UpdateProgress(currentIteration, $"Поиск лучшего параметра: {currentTask}");
                        ClearReferenceDfts();
                        foreach (var kvp in referenceImages)
                        {
                            var features = new List<Mat>();
                            foreach (var img in kvp.Value)
                            {
                                Mat feature = ExtractDftFeatures(img, vectorSize);
                                if (!feature.Empty()) features.Add(feature);
                                else feature.Dispose();
                            }
                            if (features.Count > 0) referenceDfts[kvp.Key] = features;
                        }
                        int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                           (testGray) => ClassifyByDft(testGray, vectorSize));
                        AddResultToTable(dgvDft, paramString, correctCount, totalTestFiles);
                        ClearReferenceDfts();
                    }
                    foreach (int vectorSize in dftDctVectorSizes)
                    {
                        string paramString = $"Size: {Math.Sqrt(vectorSize)}X{Math.Sqrt(vectorSize)}";
                        string currentTask = $"DCT ({paramString})";
                        UpdateProgress(currentIteration, $"Поиск лучшего параметра: {currentTask}");
                        ClearReferenceDcts();
                        foreach (var kvp in referenceImages)
                        {
                            var features = new List<Mat>();
                            foreach (var img in kvp.Value)
                            {
                                Mat feature = ExtractDctFeatures(img, vectorSize);
                                if (!feature.Empty()) features.Add(feature);
                                else feature.Dispose();
                            }
                            if (features.Count > 0) referenceDcts[kvp.Key] = features;
                        }
                        int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                           (testGray) => ClassifyByDct(testGray, vectorSize));

                        AddResultToTable(dgvDct, paramString, correctCount, totalTestFiles);
                        ClearReferenceDcts();
                    }
                    foreach (int gradY in gradientYs)
                    {
                        foreach (int gradS in gradientSs)
                        {
                            string paramString = $"Y={gradY}, S={gradS}";
                            string currentTask = $"Градиент ({paramString})";
                            UpdateProgress(currentIteration, $"Поиск лучшего параметра: {currentTask}");
                            referenceGradients.Clear();
                            foreach (var kvp in referenceImages)
                            {
                                var features = new List<List<double>>();
                                foreach (var img in kvp.Value)
                                {
                                    List<double> feature = ExtractGradientFeatures(img, gradY, gradS);
                                    if (feature.Count > 0) features.Add(feature);
                                }
                                if (features.Count > 0) referenceGradients[kvp.Key] = features;
                            }
                            if (!referenceGradients.Any(kvp => kvp.Value.Any()))
                            {
                                currentIteration += totalTestFiles; UpdateProgress(currentIteration); continue;
                            }
                            int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                               (testGray) => ClassifyByGradient(testGray, gradY, gradS));
                            AddResultToTable(dgvGradient, paramString, correctCount, totalTestFiles);
                            referenceGradients.Clear();
                        }
                    }
                    foreach (int blockSize in blockSizes)
                    {
                        string paramString = $"Size: {119 / blockSize}X{92 / blockSize}";
                        string currentTask = $"Scale ({paramString})";
                        UpdateProgress(currentIteration, $"Поиск лучшего параметра: {currentTask}");
                        ClearReferenceScales();
                        foreach (var kvp in referenceImages)
                        {
                            var features = new List<Mat>();
                            foreach (var img in kvp.Value)
                            {
                                Mat feature = ExtractScaleFeatures(img, blockSize);
                                if (!feature.Empty()) features.Add(feature);
                                else feature.Dispose();
                            }
                            if (features.Count > 0) referenceScales[kvp.Key] = features;
                        }
                        int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                           (testGray) => ClassifyByScale(testGray, blockSize));
                        AddResultToTable(dgvScale, paramString, correctCount, totalTestFiles);
                        ClearReferenceScales();
                    }

                });
                UpdateProgress(progressBar.Maximum, "Поиск параметров завершен.");
                MessageBox.Show("Поиск параметров завершен", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка во время выполнения: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DisposeReferenceImages();
                ClearAllReferenceFeatures();
                testFiles.Clear();
                SetUIEnabled(true);
                progressBar.Visible = false;
                Text = "Классификатор изображений";
            }
        }
        private int ClassifySet(string currentTask, int totalTestFiles, ref int currentIteration, Func<Mat, string> classifyFunction)
        {
            int correctCount = 0;
            for (int i = 0; i < totalTestFiles; ++i)
            {
                var testFile = testFiles[i];
                UpdateProgress(currentIteration, $"Тестирование: {currentTask} ({i + 1}/{totalTestFiles})");

                using (Mat testGray = Cv2.ImRead(testFile.path, ImreadModes.Grayscale))
                {
                    if (testGray.Empty())
                    {
                        currentIteration++;
                        continue;
                    }
                    try
                    {
                        string predicted = classifyFunction(testGray);
                        if (predicted == testFile.trueClass) correctCount++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка{ex}");
                    }
                }
                currentIteration++;
            }
            return correctCount;
        }
        private void SetUIEnabled(bool isEnabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SetUIEnabled(isEnabled)));
            }
            else
            {
                btnStartClassification.Enabled = isEnabled;
                btnVisualizeResults.Enabled = isEnabled;
                buttonForValidation.Enabled = isEnabled;
                buttonForValidationFawkes.Enabled = isEnabled;
            }
        }
        private void UpdateProgress(int value, string message = null)
        {
            if (progressBar.IsHandleCreated && !progressBar.IsDisposed)
            {
                progressBar.Invoke(new Action(() =>
                {
                    progressBar.Value = Math.Min(Math.Max(progressBar.Minimum, value), progressBar.Maximum);
                    if (message != null) Text = $"Классификатор изображений - {message}";
                }));
            }
            else if (message != null && this.IsHandleCreated && !this.IsDisposed)
            {
                this.Invoke(new Action(() =>
                {
                    Text = $"Классификатор изображений -  {message}";
                }));
            }
        }
        private List<string> GetImageFiles(string folderPath)
        {
            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".bmp", ".tiff", ".tif" };
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(folderPath)
                                      .Where(f => allowedExtensions.Contains(Path.GetExtension(f))));
                var directories = Directory.GetDirectories(folderPath);
                foreach (var dir in directories)
                {
                    try
                    {
                        files.AddRange(Directory.GetFiles(dir)
                                            .Where(f => allowedExtensions.Contains(Path.GetExtension(f))));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка{ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске файлов в {folderPath}: {ex.Message}", "Ошибка чтения файлов", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return files;
        }
        private bool LoadReferenceImages(string folderPath)
        {
            DisposeReferenceImages();
            if (!Directory.Exists(folderPath)) return false;
            var classDirectories = Directory.GetDirectories(folderPath);
            if (classDirectories.Length == 0)
            {
                MessageBox.Show("В папке эталонов не найдено подпапок с классами.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            bool loadedAny = false;
            foreach (var classDir in classDirectories)
            {
                string className = Path.GetFileName(classDir);
                var imageFiles = GetImageFiles(classDir);

                if (imageFiles.Count > 0)
                {
                    var imageMats = new List<Mat>();
                    foreach (var file in imageFiles)
                    {
                        Mat imgGray = Cv2.ImRead(file, ImreadModes.Grayscale);
                        if (!imgGray.Empty())
                        {
                            imageMats.Add(imgGray);
                        }
                        else
                        {
                            imgGray?.Dispose();
                        }
                    }
                    if (imageMats.Count > 0)
                    {
                        referenceImages[className] = imageMats;
                        loadedAny = true;
                    }

                }
            }
            return loadedAny;
        }
        private bool LoadTestFiles(string folderPath)
        {
            testFiles.Clear();
            if (!Directory.Exists(folderPath)) return false;
            var allFiles = GetImageFiles(folderPath);
            foreach (var file in allFiles)
            {
                try
                {
                    string trueClassName = Path.GetFileName(Path.GetDirectoryName(file));
                    if (!string.IsNullOrEmpty(trueClassName) && Path.GetFullPath(Path.GetDirectoryName(file)) != Path.GetFullPath(folderPath))
                    {
                        testFiles.Add((file, trueClassName));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ошибка{ex}");
                }
            }
            return testFiles.Count > 0;
        }
        private void DisposeReferenceImages()
        {
            if (referenceImages == null) return;
            foreach (var list in referenceImages.Values)
            {
                foreach (var img in list)
                {
                    img?.Dispose();
                }
            }
            referenceImages.Clear();
        }
        private void SetupDataGridViewColumns()
        {
            var dgvList = new[] { dgvHistogram, dgvDft, dgvDct, dgvScale, dgvGradient };
            foreach (var dgv in dgvList)
            {
                if (dgv == null)
                {
                    continue;
                }
                dgv.Columns.Clear();
                dgv.Columns.Add("Parameters", "Параметры");
                dgv.Columns.Add("Correct", "Верно");
                dgv.Columns.Add("Total", "Всего");
                dgv.Columns.Add("Accuracy", "Точность (%)");
                dgv.Columns["Parameters"].FillWeight = 50;
                dgv.Columns["Correct"].FillWeight = 15;
                dgv.Columns["Total"].FillWeight = 15;
                dgv.Columns["Accuracy"].FillWeight = 20;
                dgv.Columns["Accuracy"].ValueType = typeof(double);
                dgv.Columns["Accuracy"].DefaultCellStyle.Format = "F2";
                dgv.Sort(dgv.Columns["Accuracy"], ListSortDirection.Descending);
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.AllowUserToAddRows = false;
                dgv.ReadOnly = true;
                dgv.RowHeadersVisible = false;
            }
        }
        private void ClearResultTables()
        {
            var dgvList = new[] { dgvHistogram, dgvDft, dgvDct, dgvScale, dgvGradient };
            foreach (var dgv in dgvList)
            {
                if (dgv == null) continue;
                if (dgv.InvokeRequired)
                {
                    dgv.Invoke(new Action(() => dgv.Rows.Clear()));
                }
                else
                {
                    try
                    {
                        if (!dgv.IsDisposed) dgv.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка{ex}");
                    }
                }
            }
        }
        private void AddResultToTable(DataGridView dgv, string paramString, int correct, int total)
        {
            if (dgv == null || dgv.IsDisposed)
            {
                return;
            }
            if (total == 0) return;
            double accuracy = (double)correct / total * 100.0;
            Action addRowAction = () =>
            {
                if (!dgv.IsDisposed)
                {
                    dgv.Rows.Add(paramString, correct, total, accuracy);
                    try
                    {
                        dgv.Sort(dgv.Columns["Accuracy"], ListSortDirection.Descending);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка{ex}");
                    }
                }
            };
            if (dgv.InvokeRequired)
            {
                try
                {
                    dgv.Invoke(addRowAction);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ошибка{ex}");
                }
            }
            else
            {
                try
                {
                    addRowAction();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ошибка{ex}");
                }
            }
        }
        private Mat ExtractHistogram(Mat grayImage, int histSize)
        {
            if (grayImage == null || grayImage.Empty() || grayImage.Channels() != 1 || histSize <= 0)
                return new Mat();
            Mat hist = new Mat();
            try
            {
                Cv2.CalcHist(new[] { grayImage }, new[] { 0 }, null, hist, 1, new[] { histSize }, new Rangef[] { new Rangef(0, 256) });
                Cv2.Normalize(hist, hist, 0, 1, NormTypes.MinMax);
                return hist;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка{ex}");
                hist?.Dispose();
                return new Mat();
            }
        }
        private Mat ExtractDftFeatures(Mat grayImage, int vectorSize)
        {
            if (grayImage == null || grayImage.Empty() || vectorSize <= 0)
                return new Mat();
            Mat padded = new Mat();
            Mat floatImg = new Mat();
            Mat complex = new Mat();
            Mat magnitude = new Mat();
            Mat result = new Mat();
            try
            {
                int optimalRows = Cv2.GetOptimalDFTSize(grayImage.Rows);
                int optimalCols = Cv2.GetOptimalDFTSize(grayImage.Cols);
                Cv2.CopyMakeBorder(grayImage, padded, 0, optimalRows - grayImage.Rows,
                                 0, optimalCols - grayImage.Cols, BorderTypes.Constant, Scalar.All(0));
                padded.ConvertTo(floatImg, MatType.CV_32F);
                Cv2.Normalize(floatImg, floatImg, 0, 1, NormTypes.MinMax);
                Mat[] mergeArray = { floatImg, Mat.Zeros(floatImg.Size(), MatType.CV_32F) };
                Cv2.Merge(mergeArray, complex);
                Cv2.Dft(complex, complex, DftFlags.ComplexOutput);
                Mat[] planes = Cv2.Split(complex);
                Cv2.Magnitude(planes[0], planes[1], magnitude);
                planes[0].Dispose(); planes[1].Dispose();
                Cv2.Log(magnitude + Scalar.All(1), magnitude);
                ShiftDFT(magnitude);
                int cx = magnitude.Cols / 2;
                int cy = magnitude.Rows / 2;
                int size = (int)Math.Sqrt(vectorSize) * 2;
                Mat mask = Mat.Zeros(magnitude.Size(), MatType.CV_8U);
                Cv2.Rectangle(mask, new Rect(cx - size / 2, cy - size / 2, size, size), Scalar.All(255), -1);
                Mat masked = new Mat();
                magnitude.CopyTo(masked, mask);
                Cv2.Normalize(masked, masked, 0, 1, NormTypes.MinMax);
                Mat center = new Mat(masked, new Rect(cx - size / 2, cy - size / 2, size, size));
                result = center.Clone().Reshape(1, 1);
                mask.Dispose();
                masked.Dispose();
                center.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка{ex}");
                result.Dispose();
                result = new Mat();
            }
            finally
            {
                padded.Dispose();
                floatImg.Dispose();
                complex.Dispose();
                magnitude.Dispose();
            }
            return result;
        }
        private void ShiftDFT(Mat magnitude)
        {
            int cx = magnitude.Cols / 2;
            int cy = magnitude.Rows / 2;
            Mat q0 = new Mat(magnitude, new Rect(0, 0, cx, cy));
            Mat q1 = new Mat(magnitude, new Rect(cx, 0, cx, cy));
            Mat q2 = new Mat(magnitude, new Rect(0, cy, cx, cy));
            Mat q3 = new Mat(magnitude, new Rect(cx, cy, cx, cy));
            Mat tmp = new Mat();
            try
            {
                q0.CopyTo(tmp);
                q3.CopyTo(q0);
                tmp.CopyTo(q3);

                q1.CopyTo(tmp);
                q2.CopyTo(q1);
                tmp.CopyTo(q2);
            }
            finally
            {
                tmp.Dispose();
                q0.Dispose();
                q1.Dispose();
                q2.Dispose();
                q3.Dispose();
            }
        }
        private Mat ExtractDctFeatures(Mat grayImage, int vectorSize)
        {
            if (grayImage == null || grayImage.Empty() || vectorSize <= 0)
                return new Mat();
            Mat floatImg = null;
            Mat dct = null;
            Mat result = new Mat();
            try
            {
                floatImg = new Mat();
                grayImage.ConvertTo(floatImg, MatType.CV_32F);
                Cv2.Normalize(floatImg, floatImg, 0, 1, NormTypes.MinMax);
                dct = new Mat();
                Cv2.Dct(floatImg, dct, DctFlags.None);
                Mat zigzagCoeffs = new Mat(1, vectorSize, MatType.CV_32F);
                int index = 0;
                bool direction = true;
                for (int sum = 0; sum <= dct.Rows + dct.Cols - 2; sum++)
                {
                    if (index >= vectorSize) break;

                    if (direction)
                    {
                        for (int i = Math.Min(sum, dct.Rows - 1); i >= Math.Max(0, sum - dct.Cols + 1); i--)
                        {
                            int j = sum - i;
                            if (index < vectorSize)
                            {
                                zigzagCoeffs.Set(0, index, dct.At<float>(i, j));
                                index++;
                            }
                        }
                    }
                    else
                    {
                        for (int j = Math.Min(sum, dct.Cols - 1); j >= Math.Max(0, sum - dct.Rows + 1); j--)
                        {
                            int i = sum - j;
                            if (index < vectorSize)
                            {
                                zigzagCoeffs.Set(0, index, dct.At<float>(i, j));
                                index++;
                            }
                        }
                    }
                    direction = !direction;
                }
                Cv2.Normalize(zigzagCoeffs, result, 0, 1, NormTypes.MinMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка{ex}");
                result.Dispose();
                result = new Mat();
            }
            finally
            {
                floatImg?.Dispose();
                dct?.Dispose();
            }

            return result;
        }
        private List<double> ExtractGradientFeatures(Mat grayImage, int stripY, int stepS)
        {
            if (grayImage == null || grayImage.Empty() || grayImage.Channels() != 1 || stripY < 1 || stepS < 1)
                return new List<double>();
            List<double> gradientVector = new List<double>();
            int width = grayImage.Cols; int height = grayImage.Rows;
            int stripWidth = stripY; int step = stepS;
            try
            {
                for (int x = 0; x <= width - 2 * stripWidth; x += step)
                {
                    Rect leftRect = new Rect(x, 0, stripWidth, height);
                    Rect rightRect = new Rect(x + stripWidth, 0, stripWidth, height);
                    if (rightRect.Right > width) continue;
                    using (Mat leftStrip = new Mat(grayImage, leftRect))
                    using (Mat rightStrip = new Mat(grayImage, rightRect))
                    {
                        gradientVector.Add(Cv2.Mean(rightStrip).Val0 - Cv2.Mean(leftStrip).Val0);
                    }
                }
                for (int y = 0; y <= height - 2 * stripWidth; y += step)
                {
                    Rect topRect = new Rect(0, y, width, stripWidth);
                    Rect bottomRect = new Rect(0, y + stripWidth, width, stripWidth);
                    if (bottomRect.Bottom > height) continue;
                    using (Mat topStrip = new Mat(grayImage, topRect))
                    using (Mat bottomStrip = new Mat(grayImage, bottomRect))
                    {
                        gradientVector.Add(Cv2.Mean(bottomStrip).Val0 - Cv2.Mean(topStrip).Val0);
                    }
                }
                if (gradientVector.Count > 1)
                {
                    double min = gradientVector.Min(); double max = gradientVector.Max();
                    double range = max - min;
                    if (range > 1e-9)
                    {
                        for (int i = 0; i < gradientVector.Count; ++i) gradientVector[i] = (gradientVector[i] - min) / range;
                    }
                    else
                    {
                        for (int i = 0; i < gradientVector.Count; ++i) gradientVector[i] = 0.5;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка{ex}");
                return new List<double>();
            }
            return gradientVector;
        }
        private Mat ExtractScaleFeatures(Mat grayImage, int reductionLevel)
        {
            if (grayImage == null || grayImage.Empty() || reductionLevel <= 0)
                return new Mat();
            Mat current = grayImage.Clone();
            Mat next = null;
            Mat hist = new Mat();
            try
            {
                for (int i = 0; i < reductionLevel; i++)
                {
                    next = new Mat();
                    Cv2.PyrDown(current, next);
                    current.Dispose();
                    current = next.Clone();
                    next.Dispose();
                }
                int[] histSize = { 32 };
                Rangef[] ranges = { new Rangef(0, 256) };
                Cv2.CalcHist(new[] { current }, new[] { 0 }, null, hist, 1, histSize, ranges);
                Cv2.Normalize(hist, hist, 0, 1, NormTypes.MinMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка{ex}");
                hist.Dispose();
                hist = new Mat();
            }
            finally
            {
                current?.Dispose();
                next?.Dispose();
            }

            return hist;
        }
        private string ClassifyByHistogram(Mat testGray, int histSize)
        {
            if (referenceHistograms == null || referenceHistograms.Count == 0) return "N/A";
            using (Mat testHist = ExtractHistogram(testGray, histSize))
            {
                if (testHist.Empty()) return "Unknown (Feature Error)";
                string bestMatchClass = "Unknown"; double bestScore = -1.0;
                foreach (var kvp in referenceHistograms)
                {
                    string className = kvp.Key; double classBestScore = -1.0;
                    foreach (var refHist in kvp.Value)
                    {
                        if (refHist.Empty()) continue;
                        try
                        {
                            double score = Cv2.CompareHist(testHist, refHist, HistCompMethods.Correl);
                            if (score > classBestScore) classBestScore = score;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"ошибка{ex}");
                            continue;
                        }
                    }
                    if (classBestScore > bestScore) { bestScore = classBestScore; bestMatchClass = className; }
                }
                return bestMatchClass;
            }
        }
        private string ClassifyByDft(Mat testGray, int vectorSize)
        {
            if (referenceDfts == null || !referenceDfts.Any())
                return "Unknown";
            using (Mat testFeatures = ExtractDftFeatures(testGray, vectorSize))
            {
                if (testFeatures.Empty()) return "Unknown";
                string bestClass = "Unknown";
                double minDist = double.MaxValue;
                foreach (var kvp in referenceDfts)
                {
                    foreach (var refFeature in kvp.Value)
                    {
                        double similarity = Cv2.CompareHist(testFeatures, refFeature, HistCompMethods.Correl);
                        double dist = 1 - similarity;
                        if (dist < minDist)
                        {
                            minDist = dist;
                            bestClass = kvp.Key;
                        }
                    }
                }
                return minDist < 0.3 ? bestClass : "Unknown";
            }
        }
        private string ClassifyByDct(Mat testGray, int vectorSize)
        {
            if (referenceDcts == null || referenceDcts.Count == 0)
                return "N/A";
            Mat testFeatures = ExtractDctFeatures(testGray, vectorSize);
            if (testFeatures.Empty())
            {
                testFeatures.Dispose();
                return "Unknown";
            }
            string bestClass = "Unknown";
            double minDistance = double.MaxValue;
            try
            {
                Mat weights = Mat.Ones(testFeatures.Size(), MatType.CV_32F);
                for (int i = 0; i < weights.Cols; i++)
                {
                    weights.Set(0, i, (float)(1.0 - 0.9 * i / weights.Cols));
                }
                foreach (var kvp in referenceDcts)
                {
                    foreach (var refFeature in kvp.Value)
                    {
                        Mat diff = new Mat();
                        Cv2.Absdiff(testFeatures, refFeature, diff);
                        Cv2.Multiply(diff, weights, diff);
                        double distance = Cv2.Norm(diff, NormTypes.L2);

                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            bestClass = kvp.Key;
                        }
                        diff.Dispose();
                    }
                }
                weights.Dispose();
            }
            finally
            {
                testFeatures.Dispose();
            }

            return minDistance == double.MaxValue ? "Unknown" : bestClass;
        }
        private string ClassifyByGradient(Mat testGray, int stripY, int stepS)
        {
            if (referenceGradients == null || referenceGradients.Count == 0) return "N/A";
            List<double> testGradient = ExtractGradientFeatures(testGray, stripY, stepS);
            if (testGradient.Count == 0) return "Unknown (Feature Error)";
            string bestMatchClass = "Unknown"; double minDistance = double.MaxValue;
            foreach (var kvp in referenceGradients)
            {
                string className = kvp.Key; double classMinDistance = double.MaxValue;
                foreach (var refGradient in kvp.Value)
                {
                    if (refGradient.Count != testGradient.Count) continue;
                    try
                    {
                        double distance = CalculateDistance(testGradient, refGradient);
                        if (distance < classMinDistance) classMinDistance = distance;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка{ex}");
                        continue;
                    }
                }
                if (classMinDistance < minDistance) { minDistance = classMinDistance; bestMatchClass = className; }
            }
            if (minDistance == double.MaxValue) return "Unknown (No Match)";
            return bestMatchClass;
        }
        private string ClassifyByScale(Mat testGray, int reductionLevel)
        {
            if (referenceScales == null || referenceScales.Count == 0)
                return "N/A";
            Mat testFeatures = ExtractScaleFeatures(testGray, reductionLevel);
            if (testFeatures.Empty())
            {
                testFeatures.Dispose();
                return "Unknown";
            }
            string bestClass = "Unknown";
            double minDist = double.MaxValue;
            try
            {
                foreach (var kvp in referenceScales)
                {
                    foreach (var refFeature in kvp.Value)
                    {
                        double dist = Cv2.CompareHist(testFeatures, refFeature, HistCompMethods.Chisqr);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            bestClass = kvp.Key;
                        }
                    }
                }
            }
            finally
            {
                testFeatures.Dispose();
            }
            return bestClass;
        }
        private void ClearReferenceScales()
        {
            ClearMatDictionary(referenceScales);
        }
        private double CalculateDistance(List<double> v1, List<double> v2)
        {
            double sumOfSquares = 0;
            for (int i = 0; i < v1.Count; i++)
            {
                sumOfSquares += (v1[i] - v2[i]) * (v1[i] - v2[i]);
            }
            return Math.Sqrt(sumOfSquares);
        }
        private void ClearMatDictionary(Dictionary<string, List<Mat>> dict)
        {
            if (dict == null) return;
            foreach (var list in dict.Values)
            {
                if (list == null) continue;
                foreach (var mat in list)
                {
                    mat?.Dispose();
                }
            }
            dict.Clear();
        }
        private void ClearReferenceHistograms()
        {
            ClearMatDictionary(referenceHistograms);
        }
        private void ClearReferenceDfts()
        {
            ClearMatDictionary(referenceDfts);
        }
        private void ClearReferenceDcts()
        {
            ClearMatDictionary(referenceDcts);
        }
        private void ClearReferenceDescriptors()
        {
            ClearMatDictionary(referenceDescriptors);
        }
        private void ClearAllReferenceFeatures()
        {
            ClearReferenceHistograms();
            ClearReferenceDfts();
            ClearReferenceDcts();
            ClearReferenceDescriptors();
            referenceGradients?.Clear();
        }
        private void ClearPictureBoxes()
        {
            var pictureBoxes = new[] {
                pbTestImage, pbFeatureHist, pbFeatureDFT, pbFeatureDCT, pbFeatureScale, pbFeatureGradient,
                pbResultHist, pbResultDFT, pbResultDCT, pbResultScale, pbResultGradient
            };
            foreach (var pb in pictureBoxes)
            {
                UpdatePictureBox(pb, null);
            }
        }
        private bool LoadReferenceImagesForViz(string folderPath)
        {
            DisposeReferenceImagesViz();
            if (!Directory.Exists(folderPath)) return false;
            var classDirectories = Directory.GetDirectories(folderPath);
            if (classDirectories.Length == 0) { return false; }
            bool loadedAny = false;
            foreach (var classDir in classDirectories)
            {
                string className = Path.GetFileName(classDir);
                var imageFiles = GetImageFiles(classDir);
                if (imageFiles.Count > 0)
                {
                    var imagePathMats = new List<(string path, Mat image)>();
                    foreach (var file in imageFiles)
                    {
                        Mat img = Cv2.ImRead(file, ImreadModes.Color);
                        if (!img.Empty()) { imagePathMats.Add((file, img)); }
                        else { img?.Dispose(); }
                    }
                    if (imagePathMats.Count > 0) { referenceImagesViz[className] = imagePathMats; loadedAny = true; }
                }
            }
            return loadedAny;
        }
        private void DisposeReferenceImagesViz()
        {
            if (referenceImagesViz == null) return;
            foreach (var list in referenceImagesViz.Values)
            {
                foreach (var item in list)
                {
                    item.image?.Dispose();
                }
            }
            referenceImagesViz.Clear();
        }
        private void ClearAllReferenceFeaturesViz()
        {
            ClearMatDictionaryViz(referenceHistogramsViz);
            ClearMatDictionaryViz(referenceDftsViz);
            ClearMatDictionaryViz(referenceDctsViz);
            ClearMatDictionaryViz(referenceScalesViz); ;
            referenceGradientsViz?.Clear(); ;
        }
        private bool LoadReferenceFeaturesForViz()
        {
            if (referenceImagesViz == null || referenceImagesViz.Count == 0)
            {
                return false;
            }
            ClearAllReferenceFeaturesViz();
            int histSize = _bins;
            int dftVectorSize = _vectorSizeDft;
            int dctVectorSize = _vectorSizeDct;
            int gradY = _gradY, gradS = _gradS;
            int scaleReductionLevel = _scaleBlock;
            bool featuresLoaded = false;
            foreach (var kvp in referenceImagesViz)
            {
                string className = kvp.Key;
                referenceHistogramsViz[className] = new List<(string path, Mat feature)>();
                referenceDftsViz[className] = new List<(string path, Mat feature)>();
                referenceDctsViz[className] = new List<(string path, Mat feature)>();
                referenceScalesViz[className] = new List<(string path, Mat feature)>();
                referenceGradientsViz[className] = new List<(string path, List<double> feature)>();
                foreach (var item in kvp.Value)
                {
                    if (item.image == null || item.image.Empty()) continue;
                    using (Mat gray = new Mat())
                    {
                        if (item.image.Channels() == 3) Cv2.CvtColor(item.image, gray, ColorConversionCodes.BGR2GRAY);
                        else if (item.image.Channels() == 1) item.image.CopyTo(gray);
                        else continue;
                        if (gray.Empty()) continue;
                        Mat hist = ExtractHistogram(gray, histSize);
                        if (!hist.Empty()) { referenceHistogramsViz[className].Add((item.path, hist)); featuresLoaded = true; } else { hist.Dispose(); }
                        Mat dft = ExtractDftFeatures(gray, dftVectorSize);
                        if (!dft.Empty()) { referenceDftsViz[className].Add((item.path, dft)); featuresLoaded = true; } else { dft.Dispose(); }
                        Mat dct = ExtractDctFeatures(gray, dctVectorSize);
                        if (!dct.Empty()) { referenceDctsViz[className].Add((item.path, dct)); featuresLoaded = true; } else { dct.Dispose(); }
                        List<double> grad = ExtractGradientFeatures(gray, gradY, gradS);
                        if (grad.Count > 0) { referenceGradientsViz[className].Add((item.path, grad)); featuresLoaded = true; }
                        Mat scaleFeat = ExtractScaleFeatures(gray, scaleReductionLevel);
                        if (!scaleFeat.Empty()) { referenceScalesViz[className].Add((item.path, scaleFeat)); featuresLoaded = true; } else { scaleFeat.Dispose(); }
                    }
                }
                if (referenceHistogramsViz[className].Count == 0 && referenceDftsViz[className].Count == 0 && referenceDctsViz[className].Count == 0 && referenceScalesViz[className].Count == 0 && referenceGradientsViz[className].Count == 0)
                {
                    referenceHistogramsViz.Remove(className); referenceDftsViz.Remove(className); referenceDctsViz.Remove(className);
                    referenceScalesViz.Remove(className); referenceGradientsViz.Remove(className);
                }
            }
            return featuresLoaded;
        }
        private void ClearMatDictionaryViz(Dictionary<string, List<(string path, Mat feature)>> dict)
        {
            if (dict == null) return; foreach (var list in dict.Values) { if (list == null) continue; foreach (var item in list) { item.feature?.Dispose(); } }
            dict.Clear();
        }
        private (string className, string bestRefPath) ClassifyByHistogramViz(Mat testGray, int histSize)
        {
            if (referenceHistogramsViz == null || referenceHistogramsViz.Count == 0) return ("N/A", null);
            using (Mat testHist = ExtractHistogram(testGray, histSize))
            {
                if (testHist.Empty()) return ("Unknown (Feature Error)", null);
                string bestMatchClass = "Unknown"; double bestScore = -1.0; string bestPath = null;
                foreach (var kvp in referenceHistogramsViz)
                {
                    string className = kvp.Key; double classBestScore = -1.0; string classBestPath = null;
                    foreach (var refItem in kvp.Value)
                    {
                        if (refItem.feature.Empty()) continue;
                        try { double score = Cv2.CompareHist(testHist, refItem.feature, HistCompMethods.Correl); if (score > classBestScore) { classBestScore = score; classBestPath = refItem.path; } }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"ошибка{ex}");
                            continue;
                        }
                    }
                    if (classBestScore > bestScore) { bestScore = classBestScore; bestMatchClass = className; bestPath = classBestPath; }
                }
                return (bestMatchClass, bestPath);
            }
        }
        private (string className, string bestRefPath) ClassifyByDftViz(Mat testGray, int vectorSize)
        {
            if (referenceDftsViz == null || !referenceDftsViz.Any()) return ("N/A", null);
            using (Mat testFeatures = ExtractDftFeatures(testGray, vectorSize))
            {
                if (testFeatures.Empty()) return ("Unknown (Feature Error)", null);
                string bestClass = "Unknown"; double minDist = double.MaxValue; string bestPath = null;
                foreach (var kvp in referenceDftsViz)
                {
                    string className = kvp.Key; double classMinDist = double.MaxValue; string classBestPath = null;
                    foreach (var refItem in kvp.Value)
                    {
                        if (refItem.feature.Empty() || refItem.feature.Rows != testFeatures.Rows || refItem.feature.Cols != testFeatures.Cols) continue;
                        try
                        {
                            double similarity = Cv2.CompareHist(testFeatures, refItem.feature, HistCompMethods.Correl);
                            double dist = 1.0 - similarity;
                            if (dist < classMinDist) { classMinDist = dist; classBestPath = refItem.path; }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"ошибка{ex}");
                            continue;
                        }
                    }
                    if (classMinDist < minDist) { minDist = classMinDist; bestClass = className; bestPath = classBestPath; }
                }
                return (minDist < 0.3 ? bestClass : "Unknown", bestPath);
            }
        }
        private (string className, string bestRefPath) ClassifyByDctViz(Mat testGray, int vectorSize)
        {
            if (referenceDctsViz == null || !referenceDctsViz.Any()) return ("N/A", null);
            using (Mat testFeatures = ExtractDctFeatures(testGray, vectorSize))
            {
                if (testFeatures.Empty()) return ("Unknown (Feature Error)", null);
                string bestClass = "Unknown"; double minDistance = double.MaxValue; string bestPath = null;
                using (Mat weights = Mat.Ones(testFeatures.Size(), MatType.CV_32F))
                {
                    for (int i = 0; i < weights.Cols; i++) { weights.Set(0, i, (float)(1.0 - 0.9 * i / weights.Cols)); }

                    foreach (var kvp in referenceDctsViz)
                    {
                        string className = kvp.Key; double classMinDist = double.MaxValue; string classBestPath = null;
                        foreach (var refItem in kvp.Value)
                        {
                            if (refItem.feature.Empty() || refItem.feature.Rows != testFeatures.Rows || refItem.feature.Cols != testFeatures.Cols) continue;
                            using (Mat diff = new Mat())
                            {
                                try
                                {
                                    Cv2.Absdiff(testFeatures, refItem.feature, diff);
                                    Cv2.Multiply(diff, weights, diff);
                                    double distance = Cv2.Norm(diff, NormTypes.L2);
                                    if (distance < classMinDist) { classMinDist = distance; classBestPath = refItem.path; }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"ошибка{ex}");
                                    continue;
                                }
                            }
                        }
                        if (classMinDist < minDistance) { minDistance = classMinDist; bestClass = className; bestPath = classBestPath; }
                    }
                }
                return (minDistance == double.MaxValue ? "Unknown (No Match)" : bestClass, bestPath);
            }
        }
        private (string className, string bestRefPath) ClassifyByGradientViz(Mat testGray, int stripY, int stepS)
        {
            if (referenceGradientsViz == null || referenceGradientsViz.Count == 0) return ("N/A", null);
            List<double> testGradient = ExtractGradientFeatures(testGray, stripY, stepS);
            if (testGradient.Count == 0) return ("Unknown (Feature Error)", null);
            string bestMatchClass = "Unknown"; double minDistance = double.MaxValue; string bestPath = null;
            foreach (var kvp in referenceGradientsViz)
            {
                string className = kvp.Key; double classMinDistance = double.MaxValue; string classBestPath = null;
                foreach (var refItem in kvp.Value)
                {
                    if (refItem.feature.Count != testGradient.Count) continue;
                    try { double distance = CalculateDistance(testGradient, refItem.feature); if (distance < classMinDistance) { classMinDistance = distance; classBestPath = refItem.path; } }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка{ex}");
                        continue;
                    }
                }
                if (classMinDistance < minDistance) { minDistance = classMinDistance; bestMatchClass = className; bestPath = classBestPath; }
            }
            return (minDistance == double.MaxValue ? "Unknown (No Match)" : bestMatchClass, bestPath);
        }
        private (string className, string bestRefPath) ClassifyByScaleViz(Mat testGray, int reductionLevel)
        {
            {
                if (referenceScalesViz == null || !referenceScalesViz.Any()) return ("N/A", null);
                using (Mat testFeatures = ExtractScaleFeatures(testGray, reductionLevel))
                {
                    if (testFeatures.Empty()) return ("Unknown (Feature Error)", null);
                    string bestClass = "Unknown"; double minDist = double.MaxValue; string bestPath = null;
                    foreach (var kvp in referenceScalesViz)
                    {
                        string className = kvp.Key; double classMinDist = double.MaxValue; string classBestPath = null;
                        foreach (var refItem in kvp.Value)
                        {
                            if (refItem.feature.Empty()) continue;
                            try
                            {
                                double dist = Cv2.CompareHist(testFeatures, refItem.feature, HistCompMethods.Chisqr);
                                if (dist < classMinDist) { classMinDist = dist; classBestPath = refItem.path; }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"ошибка{ex}");
                                continue;
                            }
                        }
                        if (classMinDist < minDist) { minDist = classMinDist; bestClass = className; bestPath = classBestPath; }
                    }
                    return (minDist == double.MaxValue ? "Unknown (No Match)" : bestClass, bestPath);
                }
            }
        }
        private void UpdatePictureBox(PictureBox pb, Bitmap bmp)
        {
            if (pb == null || pb.IsDisposed) return;
            Action updateAction = () =>
            {
                if (!pb.IsDisposed)
                {
                    var oldImage = pb.Image;
                    pb.Image = bmp;
                    oldImage?.Dispose();
                }
                else
                {
                    bmp?.Dispose();
                }
            };

            if (pb.InvokeRequired)
            {
                try { pb.Invoke(updateAction); } catch { bmp?.Dispose(); }
            }
            else
            {
                try { updateAction(); } catch { bmp?.Dispose(); }
            }
        }
        private Bitmap PlotHistogram(Mat hist, int width = 256, int height = 150)
        {
            if (hist == null || hist.Empty()) return new Bitmap(width, height);
            Cv2.Normalize(hist, hist, 0, height, NormTypes.MinMax);
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                Pen pen = Pens.Black;
                int histSize = (int)hist.Total();
                int binWidth = Math.Max(1, width / histSize);

                for (int i = 0; i < histSize; i++)
                {
                    int h = (int)hist.Get<float>(i);
                    g.DrawLine(pen, i * binWidth, height - 1, i * binWidth, height - 1 - h);
                }
            }
            return bmp;
        }
        private Bitmap VisualizeDftMagnitude(Mat dftMagnitude)
        {
            if (dftMagnitude == null || dftMagnitude.Empty()) return new Bitmap(100, 100);
            Mat viz = new Mat();
            Cv2.Normalize(dftMagnitude, viz, 0, 255, NormTypes.MinMax);
            viz.ConvertTo(viz, MatType.CV_8U);
            return viz.ToBitmap();
        }
        private Bitmap VisualizeDct(Mat dctCoeffs)
        {
            if (dctCoeffs == null || dctCoeffs.Empty()) return new Bitmap(100, 100);
            Mat logAbsDct = new Mat();
            using (Mat absDct = new Mat())
            {
                Cv2.Absdiff(dctCoeffs, Scalar.All(0), absDct);
                using (Mat ones = Mat.Ones(absDct.Size(), absDct.Type()))
                {
                    Cv2.Add(absDct, ones, absDct);
                }
                Cv2.Log(absDct, logAbsDct);
            }
            Cv2.Normalize(logAbsDct, logAbsDct, 0, 255, NormTypes.MinMax);
            logAbsDct.ConvertTo(logAbsDct, MatType.CV_8U);
            Bitmap bmp = logAbsDct.ToBitmap();
            logAbsDct.Dispose();
            return bmp;
        }
        private Bitmap PlotVectorAsGraph(List<double> vector, int width = 256, int height = 150)
        {
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                if (vector == null || vector.Count < 2)
                {
                    using (Font font = new Font("Arial", 8))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString("Нет данных", font, Brushes.Gray, new RectangleF(0, 0, width, height), sf);
                    }
                    return bmp;
                }
                Pen pen = new Pen(Color.Blue, 1);
                float xStep = (float)(width - 1) / (vector.Count - 1);
                double minVal = vector.Min();
                double maxVal = vector.Max();
                double range = maxVal - minVal;
                float GetY(double val)
                {
                    if (range < 1e-9) return (float)height / 2;
                    return (height - 2) - (float)((val - minVal) / range * (height - 4)) + 1;
                }
                PointF prevPoint = new PointF(0, GetY(vector[0]));
                for (int i = 1; i < vector.Count; i++)
                {
                    float x = i * xStep;
                    float y = GetY(vector[i]);
                    PointF currentPoint = new PointF(x, y);
                    try { g.DrawLine(pen, prevPoint, currentPoint); }
                    catch { }
                    prevPoint = currentPoint;
                }
                pen.Dispose();
            }
            return bmp;
        }
        private void UpdateResultUI(Label label, PictureBox pb, string className, string imagePath)
        {
            Bitmap bmp = null;
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                try
                {
                    using (Mat refImg = Cv2.ImRead(imagePath, ImreadModes.Color))
                    {
                        if (!refImg.Empty())
                        {
                            bmp = refImg.ToBitmap();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ошибка{ex}");
                    bmp = null;
                }
            }
            UpdatePictureBox(pb, bmp);
        }
        private Mat ExtractDftMagnitudeForViz(Mat grayImage)
        {
            int m = Cv2.GetOptimalDFTSize(grayImage.Rows);
            int n = Cv2.GetOptimalDFTSize(grayImage.Cols);
            Mat padded = new Mat();
            Mat floatPadded = new Mat();
            Mat complexI = new Mat();
            Mat magnitude = new Mat();
            try
            {
                Cv2.CopyMakeBorder(grayImage, padded, 0, m - grayImage.Rows, 0, n - grayImage.Cols, BorderTypes.Constant, Scalar.All(0));
                padded.ConvertTo(floatPadded, MatType.CV_32F);
                using (Mat zeros = Mat.Zeros(floatPadded.Size(), MatType.CV_32F))
                {
                    Cv2.Merge(new[] { floatPadded, zeros }, complexI);
                }
                Cv2.Dft(complexI, complexI, DftFlags.ComplexOutput);
                Mat[] planes = Cv2.Split(complexI);
                using (planes[0])
                using (planes[1])
                {
                    Cv2.Magnitude(planes[0], planes[1], magnitude);
                }
                using (Mat ones = Mat.Ones(magnitude.Size(), magnitude.Type()))
                {
                    Cv2.Add(magnitude, ones, magnitude);
                }
                Cv2.Log(magnitude, magnitude);
                int cx = magnitude.Cols / 2;
                int cy = magnitude.Rows / 2;
                Mat q0 = new Mat(magnitude, new Rect(0, 0, cx, cy));
                Mat q1 = new Mat(magnitude, new Rect(cx, 0, cx, cy));
                Mat q2 = new Mat(magnitude, new Rect(0, cy, cx, cy));
                Mat q3 = new Mat(magnitude, new Rect(cx, cy, cx, cy));
                using (q0)
                using (q1)
                using (q2)
                using (q3)
                using (Mat tmp = new Mat())
                {
                    q0.CopyTo(tmp);
                    q3.CopyTo(q0);
                    tmp.CopyTo(q3);

                    q1.CopyTo(tmp);
                    q2.CopyTo(q1);
                    tmp.CopyTo(q2);
                }

                return magnitude.Clone();
            }
            finally
            {
                padded?.Dispose();
                floatPadded?.Dispose();
                complexI?.Dispose();
                magnitude?.Dispose();
            }
        }
        private Mat ExtractDctForViz(Mat grayImage)
        {
            Mat floatGray = new Mat();
            Mat dctResult = new Mat();

            try
            {
                grayImage.ConvertTo(floatGray, MatType.CV_32F);
                Cv2.Dct(floatGray, dctResult, DctFlags.None);
                return dctResult.Clone();
            }
            finally
            {
                floatGray?.Dispose();
                dctResult?.Dispose();
            }
        }

        private async void btnVisualizeResults_Click_1(object sender, EventArgs e)
        {
            if (!Directory.Exists(referenceFolderPath) || !Directory.Exists(testFolderPath))
            {
                MessageBox.Show("Выберите папки эталонов и тестов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            ClearPictureBoxes();
            SetUIEnabled(false);
            progressBar.Value = 0; progressBar.Visible = true;
            int delayMilliseconds = 30;
            try
            {
                UpdateProgress(0, "Загрузка эталонных изображений...");
                bool refsLoaded = await Task.Run(() => LoadReferenceImagesForViz(referenceFolderPath));
                if (!refsLoaded) { MessageBox.Show("Не удалось загрузить эталонные изображения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                UpdateProgress(30, "Загрузка эталонных признаков...");
                bool featuresLoaded = await Task.Run(() => LoadReferenceFeaturesForViz());
                if (!featuresLoaded) { MessageBox.Show("Не удалось извлечь признаки эталонов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                UpdateProgress(60, "Загрузка списка тестовых файлов...");
                if (!LoadTestFiles(testFolderPath) || testFiles.Count == 0) { MessageBox.Show("Тестовые файлы не найдены/не загружены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                int totalTestFiles = testFiles.Count;
                progressBar.Maximum = totalTestFiles; progressBar.Value = 0;
                UpdateProgress(0, "Начало визуализации...");
                await Task.Run(async () =>
                {
                    for (int i = 0; i < totalTestFiles; i++)
                    {
                        string filename = Path.GetFileName(testFiles[i].path);
                        UpdateProgress(i, $"Классификация: {i + 1}/{totalTestFiles} ({filename})");
                        await DisplayImageResults(i);
                        UpdateProgress(i + 1, $"Классификация: {i + 1}/{totalTestFiles} ({filename})");
                        if (i < totalTestFiles - 1) { await Task.Delay(delayMilliseconds); }
                    }
                });
                UpdateProgress(totalTestFiles, "Классификация завершена.");
                MessageBox.Show($"Классификация для {totalTestFiles} изображений завершена.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка визуализации: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUIEnabled(true); progressBar.Visible = false; Text = "Классификатор изображений";
            }
        }
        private async Task DisplayImageResults(int index)
        {
            if (index < 0 || index >= testFiles.Count) return;
            var currentFile = testFiles[index];
            string filePath = currentFile.path;
            string trueClass = currentFile.trueClass;
            Mat testImageColor = Cv2.ImRead(filePath, ImreadModes.Color);
            Mat testImageGray = new Mat();
            if (testImageColor.Empty())
            {
                ClearPictureBoxes();
                testImageColor?.Dispose();
                return;
            }
            if (testImageColor.Channels() == 3) Cv2.CvtColor(testImageColor, testImageGray, ColorConversionCodes.BGR2GRAY); else testImageColor.CopyTo(testImageGray);
            UpdatePictureBox(pbTestImage, testImageColor.ToBitmap());
            int histSize = _bins; int dftVectorSize = _vectorSizeDft; int dctVectorSize = _vectorSizeDct;
            int gradY = _gradY, gradS = _gradS;
            int scaleBlockSizeL = _scaleBlock;
            int scaleReductionLevelClassify = _scaleBlock;
            var featureTasks = new List<Task>();
            var classificationTasks = new List<Task>();
            featureTasks.Add(Task.Run(() => { using (Mat hist = ExtractHistogram(testImageGray, histSize)) { UpdatePictureBox(pbFeatureHist, PlotHistogram(hist)); } }));
            featureTasks.Add(Task.Run(() => { using (Mat dft = ExtractDftMagnitudeForViz(testImageGray)) { UpdatePictureBox(pbFeatureDFT, VisualizeDftMagnitude(dft)); } }));
            featureTasks.Add(Task.Run(() => { using (Mat dct = ExtractDctForViz(testImageGray)) { UpdatePictureBox(pbFeatureDCT, VisualizeDct(dct)); } }));
            featureTasks.Add(Task.Run(() =>
            {
                using (Mat downscaled = DownscaleByAveragePooling(testImageGray, scaleBlockSizeL))
                {
                    if (downscaled != null && !downscaled.Empty())
                    {
                        Mat upscaledForViz = new Mat();
                        Cv2.Resize(downscaled, upscaledForViz, testImageGray.Size(), 0, 0, InterpolationFlags.Nearest);
                        UpdatePictureBox(pbFeatureScale, upscaledForViz.ToBitmap());
                        upscaledForViz.Dispose();
                    }
                    else
                    {
                        UpdatePictureBox(pbFeatureScale, null);
                    }
                }
            }));
            featureTasks.Add(Task.Run(() => { List<double> gradVector = ExtractGradientFeatures(testImageGray, gradY, gradS); UpdatePictureBox(pbFeatureGradient, PlotVectorAsGraph(gradVector)); }));
            classificationTasks.Add(Task.Run(() => { var r = ClassifyByHistogramViz(testImageGray, histSize); UpdateResultUI(lblResultHistClass, pbResultHist, r.className, r.bestRefPath); }));
            classificationTasks.Add(Task.Run(() => { var r = ClassifyByDftViz(testImageGray, dftVectorSize); UpdateResultUI(lblResultDftClass, pbResultDFT, r.className, r.bestRefPath); }));
            classificationTasks.Add(Task.Run(() => { var r = ClassifyByDctViz(testImageGray, dctVectorSize); UpdateResultUI(lblResultDctClass, pbResultDCT, r.className, r.bestRefPath); }));
            classificationTasks.Add(Task.Run(() => { var r = ClassifyByGradientViz(testImageGray, gradY, gradS); UpdateResultUI(lblResultGradientClass, pbResultGradient, r.className, r.bestRefPath); }));
            classificationTasks.Add(Task.Run(() =>
            {
                var r = ClassifyByScaleViz(testImageGray, scaleReductionLevelClassify);
                UpdateResultUI(lblResultScaleClass, pbResultScale, r.className, r.bestRefPath);
            }));
            try
            {
                await Task.WhenAll(featureTasks); await Task.WhenAll(classificationTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка{ex}");
            }
            testImageColor.Dispose(); testImageGray.Dispose();
        }
        private Mat DownscaleByAveragePooling(Mat grayImage, int blockSizeL)
        {
            if (grayImage == null || grayImage.Empty() || grayImage.Channels() != 1 || blockSizeL <= 0)
            {
                return null;
            }
            if (grayImage.Type() != MatType.CV_8UC1)
            {
                if (grayImage.Channels() == 1)
                {
                    Mat temp8U = new Mat();
                    try
                    {
                        grayImage.ConvertTo(temp8U, MatType.CV_8UC1);
                        grayImage = temp8U;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка{ex}");
                        temp8U.Dispose();
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            int M = grayImage.Rows;
            int N = grayImage.Cols;
            int newHeight = M / blockSizeL;
            int newWidth = N / blockSizeL;
            if (newHeight <= 0 || newWidth <= 0)
            {
                return null;
            }
            Mat result = null;
            try
            {
                result = new Mat(newHeight, newWidth, MatType.CV_8UC1);
                for (int r = 0; r < newHeight; r++)
                {
                    for (int c = 0; c < newWidth; c++)
                    {
                        int rowStart = r * blockSizeL;
                        int colStart = c * blockSizeL;
                        int actualBlockHeight = Math.Min(blockSizeL, M - rowStart);
                        int actualBlockWidth = Math.Min(blockSizeL, N - colStart);
                        if (actualBlockWidth <= 0 || actualBlockHeight <= 0) continue;
                        Rect blockRect = new Rect(colStart, rowStart, actualBlockWidth, actualBlockHeight);
                        using (Mat block = new Mat(grayImage, blockRect))
                        {
                            Scalar meanScalar = Cv2.Mean(block);
                            byte averageValue = (byte)Math.Round(meanScalar.Val0);
                            result.Set<byte>(r, c, averageValue);
                        }
                    }
                }
                return result.Clone();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка{ex}");
                return null;
            }
            finally
            {
                result?.Dispose();
                if (grayImage != null && grayImage.Type() != MatType.CV_8UC1 && !grayImage.IsDisposed)
                {
                }
            }
        }
        public void FindAccuracy(string testFolder, string referenceFolder)
        {
            SetUIEnabled(false);
            progressBar.Value = 0;
            progressBar.Visible = true;
            try
            {
                int[] histSizes = { _bins };
                int[] dftVectorSizes = { _vectorSizeDft };
                int[] gradientYs = { _gradY };
                int[] gradientSs = { _gradS };
                int[] dctVectorSizes = { _vectorSizeDct };
                int[] scaleBlocks = { _scaleBlock };
                UpdateProgress(0, "Загрузка эталонных изображений...");
                if (!LoadReferenceImages(referenceFolder))
                {
                    MessageBox.Show("Не удалось загрузить эталонные изображения. Проверьте структуру папки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<string> classNames = referenceImages.Keys.ToList();
                UpdateProgress(5, "Загрузка тестовых файлов...");
                if (!LoadTestFiles(testFolder))
                {
                    MessageBox.Show("Не удалось загрузить тестовые файлы. Проверьте папку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int totalTestFiles = testFiles.Count;
                if (totalTestFiles == 0)
                {
                    MessageBox.Show("В тестовой папке не найдено изображений.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int totalParamRuns = histSizes.Length
                                   + dftVectorSizes.Length
                                   + dctVectorSizes.Length
                                   + (gradientYs.Length * gradientSs.Length)
                                   + scaleBlocks.Length;
                progressBar.Maximum = totalParamRuns * totalTestFiles;
                int currentIteration = 0;
                foreach (int histSize in histSizes)
                {
                    string paramString = $"Bins: {histSize}";
                    string currentTask = $"Гистограмма ({paramString})";
                    UpdateProgress(currentIteration, $"Тестирование: {currentTask}");
                    ClearReferenceHistograms();
                    foreach (var kvp in referenceImages)
                    {
                        var features = new List<Mat>();
                        foreach (var img in kvp.Value)
                        {
                            Mat feature = ExtractHistogram(img, histSize);
                            if (!feature.Empty()) features.Add(feature);
                            else feature.Dispose();
                        }
                        if (features.Count > 0) referenceHistograms[kvp.Key] = features;
                    }
                    if (!referenceHistograms.Any(kvp => kvp.Value.Any()))
                    {
                        currentIteration += totalTestFiles; UpdateProgress(currentIteration); continue;
                    }

                    int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                       (testGray) => ClassifyByHistogram(testGray, histSize));
                    _histogramPoints.Add((double)correctCount / totalTestFiles * 100.0);
                    ClearReferenceHistograms();
                }
                foreach (int vectorSize in dftVectorSizes)
                {
                    string paramString = $"VectorSize: {vectorSize}";
                    string currentTask = $"DFT ({paramString})";
                    UpdateProgress(currentIteration, $"Тестирование: {currentTask}");
                    ClearReferenceDfts();
                    foreach (var kvp in referenceImages)
                    {
                        var features = new List<Mat>();
                        foreach (var img in kvp.Value)
                        {
                            Mat feature = ExtractDftFeatures(img, vectorSize);
                            if (!feature.Empty()) features.Add(feature);
                            else feature.Dispose();
                        }
                        if (features.Count > 0) referenceDfts[kvp.Key] = features;
                    }
                    int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                       (testGray) => ClassifyByDft(testGray, vectorSize));
                    _DFTPoints.Add((double)correctCount / totalTestFiles * 100.0);
                    ClearReferenceDfts();
                }
                foreach (int vectorSize in dctVectorSizes)
                {
                    string paramString = $"VectorSize: {vectorSize}";
                    string currentTask = $"DCT ({paramString})";
                    UpdateProgress(currentIteration, $"Тестирование: {currentTask}");
                    ClearReferenceDcts();
                    foreach (var kvp in referenceImages)
                    {
                        var features = new List<Mat>();
                        foreach (var img in kvp.Value)
                        {
                            Mat feature = ExtractDctFeatures(img, vectorSize);
                            if (!feature.Empty()) features.Add(feature);
                            else feature.Dispose();
                        }
                        if (features.Count > 0) referenceDcts[kvp.Key] = features;
                    }
                    int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                       (testGray) => ClassifyByDct(testGray, vectorSize));
                    _DCTPoints.Add((double)correctCount / totalTestFiles * 100.0);
                    ClearReferenceDcts();
                }
                foreach (int gradY in gradientYs)
                {
                    foreach (int gradS in gradientSs)
                    {
                        string paramString = $"Y={gradY}, S={gradS}";
                        string currentTask = $"Градиент ({paramString})";
                        UpdateProgress(currentIteration, $"Тестирование: {currentTask}");
                        referenceGradients.Clear();
                        foreach (var kvp in referenceImages)
                        {
                            var features = new List<List<double>>();
                            foreach (var img in kvp.Value)
                            {
                                List<double> feature = ExtractGradientFeatures(img, gradY, gradS);
                                if (feature.Count > 0) features.Add(feature);
                            }
                            if (features.Count > 0) referenceGradients[kvp.Key] = features;
                        }
                        if (!referenceGradients.Any(kvp => kvp.Value.Any()))
                        {
                            currentIteration += totalTestFiles; UpdateProgress(currentIteration); continue;
                        }
                        int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                           (testGray) => ClassifyByGradient(testGray, gradY, gradS));
                        _gradientPoints.Add((double)correctCount / totalTestFiles * 100.0);
                        referenceGradients.Clear();
                    }
                }
                foreach (int blockSize in new[] { 2 })
                {
                    string paramString = $"Block Size: {blockSize}";
                    string currentTask = $"Scale ({paramString})";
                    UpdateProgress(currentIteration, $"Тестирование: {currentTask}");

                    ClearReferenceScales();
                    foreach (var kvp in referenceImages)
                    {
                        var features = new List<Mat>();
                        foreach (var img in kvp.Value)
                        {
                            Mat feature = ExtractScaleFeatures(img, blockSize);
                            if (!feature.Empty()) features.Add(feature);
                            else feature.Dispose();
                        }
                        if (features.Count > 0) referenceScales[kvp.Key] = features;
                    }

                    int correctCount = ClassifySet(currentTask, totalTestFiles, ref currentIteration,
                       (testGray) => ClassifyByScale(testGray, blockSize));

                    _scalePoints.Add((double)correctCount / totalTestFiles * 100.0);
                    ClearReferenceScales();
                }
                UpdateProgress(progressBar.Maximum, "Тестирование завершено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла критическая ошибка во время выполнения: {ex.Message}\n{ex.StackTrace}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DisposeReferenceImages();
                ClearAllReferenceFeatures();
                testFiles.Clear();
                SetUIEnabled(true);
                progressBar.Visible = false;
                Text = "Классификатор изображений";
            }
        }
        private void buttonForValidation_Click(object sender, EventArgs e)
        {
            chartForResults.Series[0].Points.Clear();
            chartForResults.Series[1].Points.Clear();
            chartForResults.Series[2].Points.Clear();
            chartForResults.Series[3].Points.Clear();
            chartForResults.Series[4].Points.Clear();
            numericForReference.Value = 1;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\9", @"..\..\images\reference\1");
            numericForReference.Value = 2;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\8", @"..\..\images\reference\2");
            numericForReference.Value = 3;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\7", @"..\..\images\reference\3");
            numericForReference.Value = 4;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\6", @"..\..\images\reference\4");
            numericForReference.Value = 5;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\5", @"..\..\images\reference\5");
            numericForReference.Value = 6;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\4", @"..\..\images\reference\6");
            numericForReference.Value = 7;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\3", @"..\..\images\reference\7");
            numericForReference.Value = 8;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\2", @"..\..\images\reference\8");
            numericForReference.Value = 9;
            ChangeStates();
            FindAccuracy(@"..\..\images\test\1", @"..\..\images\reference\9");
            DrawGraph(1, 0);
            DrawGraph(2, 1);
            DrawGraph(3, 2);
            DrawGraph(4, 3);
            DrawGraph(5, 4);
            DrawGraph(6, 5);
            DrawGraph(7, 6);
            DrawGraph(8, 7);
            DrawGraph(9, 8);
            MessageBox.Show("Тестирование завершено", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void DrawGraph(double testCount, int counter)
        {
            chartForResults.Series[0].Points.AddXY(testCount, _histogramPoints[counter]);
            chartForResults.Series[1].Points.AddXY(testCount, _DFTPoints[counter]);
            chartForResults.Series[2].Points.AddXY(testCount, _DCTPoints[counter]);
            chartForResults.Series[3].Points.AddXY(testCount, _scalePoints[counter]);
            chartForResults.Series[4].Points.AddXY(testCount, _gradientPoints[counter]);
        }

        private void buttonForValidationFawkes_Click(object sender, EventArgs e)
        {
            chartForResults.Series[0].Points.Clear();
            chartForResults.Series[1].Points.Clear();
            chartForResults.Series[2].Points.Clear();
            chartForResults.Series[3].Points.Clear();
            chartForResults.Series[4].Points.Clear();
            numericForReference.Value = 1;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\9", @"..\..\images\reference\1");
            numericForReference.Value = 2;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\8", @"..\..\images\reference\2");
            numericForReference.Value = 3;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\7", @"..\..\images\reference\3");
            numericForReference.Value = 4;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\6", @"..\..\images\reference\4");
            numericForReference.Value = 5;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\5", @"..\..\images\reference\5");
            numericForReference.Value = 6;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\4", @"..\..\images\reference\6");
            numericForReference.Value = 7;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\3", @"..\..\images\reference\7");
            numericForReference.Value = 8;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\2", @"..\..\images\reference\8");
            numericForReference.Value = 9;
            ChangeStates();
            FindAccuracy(@"..\..\images\fawkes\1", @"..\..\images\reference\9");
            DrawGraph(1, 0);
            DrawGraph(2, 1);
            DrawGraph(3, 2);
            DrawGraph(4, 3);
            DrawGraph(5, 4);
            DrawGraph(6, 5);
            DrawGraph(7, 6);
            DrawGraph(8, 7);
            DrawGraph(9, 8);
            MessageBox.Show("Тестирование завершено", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void numericForReference_ValueChanged(object sender, EventArgs e)
        {
            ChangeStates();
        }
        private void ChangeStates()
        {
            if (numericForReference.Value == 1 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\1";
                testFolderPath = @"..\..\images\test\9";
                _bins = 64;
                _vectorSizeDft = 81;
                _vectorSizeDct = 36;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 3;
            }
            if (numericForReference.Value == 2 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\2";
                testFolderPath = @"..\..\images\test\8";
                _bins = 64;
                _vectorSizeDft = 49;
                _vectorSizeDct = 49;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 3;
            }
            if (numericForReference.Value == 3 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\3";
                testFolderPath = @"..\..\images\test\7";
                _bins = 64;
                _vectorSizeDft = 49;
                _vectorSizeDct = 100;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 3;
            }
            if (numericForReference.Value == 4 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\4";
                testFolderPath = @"..\..\images\test\6";
                _bins = 64;
                _vectorSizeDft = 49;
                _vectorSizeDct = 81;
                _scaleBlock = 2;
                _gradY = 7;
                _gradS = 3;
            }
            if (numericForReference.Value == 5 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\5";
                testFolderPath = @"..\..\images\test\5";
                _bins = 256;
                _vectorSizeDft = 49;
                _vectorSizeDct = 64;
                _scaleBlock = 2;
                _gradY = 7;
                _gradS = 3;
            }
            if (numericForReference.Value == 6 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\6";
                testFolderPath = @"..\..\images\test\4";
                _bins = 256;
                _vectorSizeDft = 49;
                _vectorSizeDct = 36;
                _scaleBlock = 2;
                _gradY = 7;
                _gradS = 3;
            }
            if (numericForReference.Value == 7 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\7";
                testFolderPath = @"..\..\images\test\3";
                _bins = 256;
                _vectorSizeDft = 49;
                _vectorSizeDct = 49;
                _scaleBlock = 2;
                _gradY = 7;
                _gradS = 3;
            }
            if (numericForReference.Value == 8 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\8";
                testFolderPath = @"..\..\images\test\2";
                _bins = 64;
                _vectorSizeDft = 81;
                _vectorSizeDct = 36;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 5;
            }
            if (numericForReference.Value == 9 && checkBoxForFawkes.Checked == false)
            {
                referenceFolderPath = @"..\..\images\reference\9";
                testFolderPath = @"..\..\images\test\1";
                _bins = 16;
                _vectorSizeDft = 64;
                _vectorSizeDct = 36;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 3;
            }
            if (numericForReference.Value == 1 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\1";
                testFolderPath = @"..\..\images\fawkes\9";
                _bins = 64;
                _vectorSizeDft = 81;
                _vectorSizeDct = 36;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 3;
            }
            if (numericForReference.Value == 2 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\2";
                testFolderPath = @"..\..\images\fawkes\8";
                _bins = 32;
                _vectorSizeDft = 49;
                _vectorSizeDct = 49;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 5;
            }
            if (numericForReference.Value == 3 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\3";
                testFolderPath = @"..\..\images\fawkes\7";
                _bins = 64;
                _vectorSizeDft = 49;
                _vectorSizeDct = 49;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 3;
            }
            if (numericForReference.Value == 4 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\4";
                testFolderPath = @"..\..\images\fawkes\6";
                _bins = 32;
                _vectorSizeDft = 49;
                _vectorSizeDct = 64;
                _scaleBlock = 2;
                _gradY = 7;
                _gradS = 3;
            }
            if (numericForReference.Value == 5 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\5";
                testFolderPath = @"..\..\images\fawkes\5";
                _bins = 256;
                _vectorSizeDft = 36;
                _vectorSizeDct = 49;
                _scaleBlock = 2;
                _gradY = 7;
                _gradS = 3;
            }
            if (numericForReference.Value == 6 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\6";
                testFolderPath = @"..\..\images\fawkes\4";
                _bins = 256;
                _vectorSizeDft = 49;
                _vectorSizeDct = 49;
                _scaleBlock = 2;
                _gradY = 7;
                _gradS = 3;
            }
            if (numericForReference.Value == 7 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\7";
                testFolderPath = @"..\..\images\fawkes\3";
                _bins = 64;
                _vectorSizeDft = 36;
                _vectorSizeDct = 49;
                _scaleBlock = 2;
                _gradY = 7;
                _gradS = 3;
            }
            if (numericForReference.Value == 8 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\8";
                testFolderPath = @"..\..\images\fawkes\2";
                _bins = 64;
                _vectorSizeDft = 81;
                _vectorSizeDct = 36;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 5;
            }
            if (numericForReference.Value == 9 && checkBoxForFawkes.Checked == true)
            {
                referenceFolderPath = @"..\..\images\reference\9";
                testFolderPath = @"..\..\images\fawkes\1";
                _bins = 16;
                _vectorSizeDft = 81;
                _vectorSizeDct = 36;
                _scaleBlock = 2;
                _gradY = 5;
                _gradS = 3;
            }
        }
        private void checkBoxForFawkes_CheckedChanged(object sender, EventArgs e)
        {
            ChangeStates();
        }
    }
}
