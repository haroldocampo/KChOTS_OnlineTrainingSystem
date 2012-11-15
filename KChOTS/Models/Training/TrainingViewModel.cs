using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR;

namespace KChOTS.Models.Training {
    public class TrainingViewModel {

        public TrainingDataModel TrainingModel { get; set; }

        public TrainingViewModel() { }

        public TrainingViewModel(string username) {
            TrainingModel = new TrainingDataModel();
            PopulateData(username);
        }


        private void PopulateData(string username) {
            TrainingModel.Username = "Unkown User";
            if (username != null)
            {
                TrainingModel.Username = username;
            }
        }
    }
}