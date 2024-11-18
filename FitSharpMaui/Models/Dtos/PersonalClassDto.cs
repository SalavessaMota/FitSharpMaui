﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSharpMaui.Models.Dtos
{
    public class PersonalClassDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Gym { get; set; }
        public string ClassType { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Instructor { get; set; }
        public double InstructorScore { get; set; }
    }
}