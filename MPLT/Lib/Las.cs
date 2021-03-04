using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using Petroware.LogIo.Las;
using Petroware.Uom;
namespace MPLT.Lib
{
    // <summary>
    //   Class for testing the LogIo.Net LAS read API.
    // </summary>
    public class LasTest
    {
  
        /*
         * 
         * //  public static void Main(string[] arguments)
       // {

            
            FileInfo file = new FileInfo("path/to/file.LAS");
            //
            // Write some meta information about the disk file
            //
            Console.WriteLine("File: " + file.FullName);
            if (!file.Exists)
            {
                Console.WriteLine("File not found.");
                return;
            }
            Console.WriteLine("Size: " + file.Length);
            //
            // Read the LAS file including bulk data. Report the performance.
            //
            LasFileReader reader = new LasFileReader(file);
            Console.WriteLine();
            Console.WriteLine("Reading...");
            int startTime = Environment.TickCount;
            IList<LasFile> lasFiles = reader.Read(true);
            int time = Environment.TickCount - startTime;
            double speed = Math.Round(file.Length / time / 1000.0); // MB/s
            Console.WriteLine("Done in " + time + "ms. (" + speed + "MB/s)");
            //
            // Report number of subfiles. Loop over each of them.
            //
            Console.WriteLine();
            Console.WriteLine("The file contains " + lasFiles.Count + " subfile(s):");
            foreach (LasFile lasFile in lasFiles)
            {
                //
                // Report subfile name
                //
                Console.WriteLine();
                Console.WriteLine("File name: " + lasFile.GetName());
                Console.WriteLine();
                //
                // Report the meta-data we can conveniently extract through LasUtil
                //
                Console.WriteLine("-- Meta-data as reported by LasUtil:");
                Console.WriteLine("Well name.........: " + LasUtil.GetWellName(lasFile));
                Console.WriteLine("Field name........: " + LasUtil.GetFieldName(lasFile));
                Console.WriteLine("Rig name..........: " + LasUtil.GetRigName(lasFile));
                Console.WriteLine("Company...........: " + LasUtil.GetCompany(lasFile));
                Console.WriteLine("Service company...: " + LasUtil.GetServiceCompany(lasFile));
                Console.WriteLine("Country...........: " + LasUtil.GetCountry(lasFile));
                Console.WriteLine("Run number........: " + LasUtil.GetRunNumber(lasFile));
                Console.WriteLine("Bit size..........: " + LasUtil.GetBitSize(lasFile));
                Console.WriteLine("Date..............: " + LasUtil.GetDate(lasFile));
                Console.WriteLine("MD................: " + LasUtil.GetMd(lasFile));
                double?[] interval = LasUtil.GetInterval(lasFile);
                if (interval != null)
                    Console.WriteLine("Interval..........: " + interval[0] + " - " + interval[1]);
                // Loop over all curves
                foreach (LasCurve curve in lasFile.GetCurves())
                {
                    Console.WriteLine("  " + curve.GetName() +
                                      curve.GetUnit() + ", " +
                                      curve.GetValueType() +
                                      curve.GetDescription());
                    //
                    // ... and the first few values
                    //
                    for (int index = 0; index < curve.GetNValues(); index++)
                    {
                        Console.Write(curve.GetValue(index) + "; ");
                        if (index == 10)
                        { // Write a few values only
                            Console.WriteLine("...");
                            break;
                        }
                    }
                }
            }
*/
       // }
    }
}
