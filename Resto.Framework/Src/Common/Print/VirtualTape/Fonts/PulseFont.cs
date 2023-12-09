﻿namespace Resto.Framework.Common.Print.VirtualTape.Fonts
 {
     public sealed class PulseFont : Font
     {
         public delegate string ConvertToPulseDelegate();
         public ConvertToPulseDelegate ConvertToPulse { get; set; }
     }
 }