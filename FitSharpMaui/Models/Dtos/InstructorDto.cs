using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSharpMaui.Models.Dtos
{
    class InstructorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Speciality { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public int NumberOfGroupClasses { get; set; }
        public int NumberOfPersonalClasses { get; set; }
        public string GymName { get; set; }
        public string ImageFullPath { get; set; }
    }
}
