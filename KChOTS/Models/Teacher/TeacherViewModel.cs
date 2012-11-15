using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KChOTS.Models.Teacher {
    public class TeacherViewModel {

        public TeacherDataModel Teacher { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }

        public TeacherViewModel() {
            _db = new KnowledgeChannelEntities();
            PopulateGenders();
            PopulateRegions();
        }

        private void PopulateRegions() {
            List<SelectListItem> regions = new List<SelectListItem>();
            regions.Add(new SelectListItem() { Text = "NCR", Value = "NCR" });
            regions.Add(new SelectListItem() { Text = "CAR", Value = "CAR" });
            regions.Add(new SelectListItem() { Text = "Region I", Value = "Region I" });
            regions.Add(new SelectListItem() { Text = "Region II", Value = "Region II" });
            regions.Add(new SelectListItem() { Text = "Region III", Value = "Region III" });
            regions.Add(new SelectListItem() { Text = "Region IV", Value = "Region IV" });
            regions.Add(new SelectListItem() { Text = "Region V", Value = "Region V" });
            regions.Add(new SelectListItem() { Text = "Region VI", Value = "Region VI" });
            regions.Add(new SelectListItem() { Text = "Region VII", Value = "Region VII" });
            regions.Add(new SelectListItem() { Text = "Region VIII", Value = "Region VIII" });
            regions.Add(new SelectListItem() { Text = "Region IX", Value = "Region IX" });
            regions.Add(new SelectListItem() { Text = "Region X", Value = "Region X" });
            regions.Add(new SelectListItem() { Text = "Region XI", Value = "Region XI" });
            regions.Add(new SelectListItem() { Text = "Region XII", Value = "Region XII" });
            regions.Add(new SelectListItem() { Text = "CARAGA", Value = "CARAGA" });
            regions.Add(new SelectListItem() { Text = "ARMM", Value = "ARMM" });
            Regions = regions;
        }

        private void PopulateGenders() {
            List<SelectListItem> genders = new List<SelectListItem>();
            genders.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            genders.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            Genders = genders;
        }

        private KnowledgeChannelEntities _db;
    }
}