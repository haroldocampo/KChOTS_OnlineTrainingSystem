using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KChOTS {
    public class ApplicationContext {

        public ApplicationContext() {
            Db = new KnowledgeChannelEntities();
        }

        public int TeacherID { get; set; }
        public int AdminID { get; set; }
        public int AdminType { get; set; }
        public KnowledgeChannelEntities Db { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}