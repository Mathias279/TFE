﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Managers;
using TFE2017.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
		public TestPage()
		{
			InitializeComponent();

            buildingInfo.Text = "ok";
            labelpath.Text = "ok";
            len.Text = "ok";


            String buidingId = "2";
            String departId = "101";
            String destinationId = "218"; // L33
            bool useStairs = true;
            bool useLift = true;


            try
            {
                Debugger.Break();
                Building building = Task.Run(() => DataBaseManager.GetBuilding(buidingId)).Result;

                Debugger.Break();
                var path = Task.Run(() => DataBaseManager.GetPath(buidingId, departId, destinationId, useStairs, useLift)).Result;

                Debugger.Break();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }
	}
}