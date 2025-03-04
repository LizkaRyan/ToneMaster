using ToneMaster.Audio;

namespace ToneMaster
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            
            string inputFile = "C:\\Users\\ryrab\\Desktop\\Ryan\\Etudes\\S6\\audio\\tel_amplified.wav";
            string outputFile = "C:\\Users\\ryrab\\Desktop\\Ryan\\Etudes\\S6\\audio\\tel_amplified_distortion.wav";
            //float amplificationFactor = 5f; // Augmenter le volume de 50%
            WavReader wavReader = new WavReader(inputFile);
            wavReader.AntiDistortion(outputFile,0.5f);
            
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
        }
    }
}