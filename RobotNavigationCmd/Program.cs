using System;

namespace RobotNavigation
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			//Checking arguement
			if (args.Length != 2)
			{
                Console.WriteLine("Invalid number of arguments!");
                System.Environment.Exit(1);
            }

			//Get search algorithm and file from commandline
			String algorithm = args[1];
			String aFile = args[0];

            //Get the CombineObject for visualizer purpose
            CombineObject result = SetUpAndProcess.InitializeAndProcess(aFile, algorithm);
		}
	}
}
